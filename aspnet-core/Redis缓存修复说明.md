# Redis缓存修复说明

## 修复的问题

### 1. 缓存键名问题
**原问题**: 缓存键名过于简单，所有查询都使用相同的键名 `"customer"`，导致不同查询条件返回相同结果。

**修复方案**: 
- 构建包含搜索参数的缓存键，确保不同查询条件有不同的缓存
- 格式：`receivables:v{版本号}:{搜索参数1}:{搜索参数2}:...`

### 2. 缓存过期时间参数类型错误
**原问题**: `expiration` 参数使用了 `int` 类型，但应该使用 `TimeSpan` 类型。

**修复方案**: 
- 将 `expiration:300` 改为 `expiration: TimeSpan.FromMinutes(5)`

### 3. 缺少返回值
**原问题**: `GetOrAddAsync` 的工厂方法中缺少 `return pageInfo;` 语句。

**修复方案**: 
- 在工厂方法末尾添加 `return pageInfo;` 语句

### 4. Redis配置未启用
**原问题**: Redis配置被注释掉，导致Redis缓存无法正常工作。

**修复方案**: 
- 取消注释 `AddStackExchangeRedisCache` 配置
- 添加默认的Redis连接字符串

### 5. 缓存失效管理
**原问题**: 当数据发生变化时，缓存不会自动失效，导致数据不一致。

**修复方案**: 
- 实现缓存版本号机制
- 在数据变化时更新版本号，使所有相关缓存失效
- 在 `InsertAsync` 方法中添加缓存清理逻辑

## 修复后的功能

### 1. 智能缓存键生成
```csharp
string cacheKey = $"receivables:v{cacheVersion}:{receivablesSearchDto.ReceivableCode}:{receivablesSearchDto.StartTime}:{receivablesSearchDto.EndTime}:{receivablesSearchDto.UserId}:{receivablesSearchDto.CustomerId}:{receivablesSearchDto.ContractId}:{receivablesSearchDto.ReceivablePay}:{receivablesSearchDto.PageIndex}:{receivablesSearchDto.PageSize}";
```

### 2. 缓存版本号管理
```csharp
private async Task ClearReceivablesCache()
{
    var cacheVersionKey = "receivables:cache:version";
    var currentVersion = await redisCacheService.GetAsync<int>(cacheVersionKey);
    await redisCacheService.SetAsync(cacheVersionKey, currentVersion + 1, TimeSpan.FromDays(30));
}
```

### 3. 测试方法
添加了 `TestRedisCacheAsync()` 方法来验证Redis缓存是否正常工作。

## 配置要求

### 1. appsettings.json
确保Redis配置正确：
```json
{
  "Redis": {
    "Configuration": "localhost:6379,defaultDatabase=0"
  }
}
```

### 2. 模块配置
确保在 `CustomerRelationshipManagementHttpApiHostModule` 中启用了Redis：
```csharp
context.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration["Redis:Configuration"] ?? "localhost:6379";
});
```

## 使用说明

1. **启动Redis服务**: 确保Redis服务器正在运行
2. **测试连接**: 调用 `TestRedisCacheAsync()` 方法验证Redis连接
3. **正常使用**: 应收款查询会自动使用缓存，数据变化时缓存会自动失效

## 性能优化

- 缓存过期时间设置为5分钟，平衡性能和实时性
- 使用缓存版本号机制，避免缓存穿透
- 不同查询条件使用不同缓存键，提高缓存命中率 