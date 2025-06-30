# 应收款缓存系统说明

## 概述

应收款服务已集成Redis缓存系统，用于提高查询性能。缓存系统支持根据不同的查询条件动态生成缓存键，确保不同查询条件返回不同的缓存结果。

## 主要功能

### 1. 动态缓存键生成

缓存键会根据以下查询条件动态生成：
- 页码 (PageIndex)
- 页大小 (PageSize)
- 应收款编号 (ReceivableCode)
- 开始时间 (StartTime)
- 结束时间 (EndTime)
- 用户ID (UserId)
- 客户ID (CustomerId)
- 合同ID (ContractId)
- 应收款金额 (ReceivablePay)

### 2. 缓存键格式

缓存键格式示例：
```
receivables:page:1:size:10
receivables:page:1:size:10:code:M20241201-1234
receivables:page:1:size:10:start:20241201:end:20241231
receivables:page:1:size:10:user:12345678-1234-1234-1234-123456789012
```

### 3. 缓存管理

#### 缓存生命周期
- 缓存过期时间：10分钟（滑动过期）
- 数据变更时自动清除相关缓存

#### 缓存清除策略
- 新增应收款时：清除所有分页查询缓存
- 更新应收款时：清除相关缓存
- 删除应收款时：清除相关缓存

### 4. 缓存键管理器

系统使用内存中的缓存键管理器来跟踪活跃的缓存键：
- 自动记录新创建的缓存键
- 数据变更时批量清除所有活跃缓存键
- 提供缓存统计信息接口

## 使用方法

### 1. 分页查询（自动缓存）

```csharp
// 查询会自动使用缓存
var result = await receivablesService.GetPageAsync(new ReceivablesSearchDto
{
    PageIndex = 1,
    PageSize = 10,
    ReceivableCode = "M20241201-1234"
});
```

### 2. 获取缓存统计信息

```csharp
// 获取当前活跃的缓存键信息
var cacheStats = await receivablesService.GetCacheStatsAsync();
```

### 3. 数据变更（自动清除缓存）

```csharp
// 新增、更新、删除操作会自动清除相关缓存
await receivablesService.InsertAsync(createDto);
await receivablesService.UpdateAsync(id, updateDto);
await receivablesService.DeleteAsync(id);
```

## 性能优化

### 1. 缓存命中率优化
- 使用查询条件生成唯一缓存键
- 避免缓存穿透和缓存雪崩

### 2. 内存使用优化
- 滑动过期时间，减少内存占用
- 数据变更时及时清除过期缓存

### 3. 并发安全
- 使用锁机制保护缓存键管理器
- 异步操作避免阻塞

## 监控和调试

### 1. 日志记录
- 缓存清除操作会记录日志
- 缓存操作异常会记录警告日志

### 2. 统计信息
- 提供缓存统计接口
- 可监控活跃缓存键数量

## 注意事项

1. **缓存一致性**：数据变更时会自动清除相关缓存，确保数据一致性
2. **内存使用**：缓存键管理器使用内存存储，需要注意内存使用量
3. **并发安全**：缓存键管理器使用锁机制，确保线程安全
4. **异常处理**：缓存操作异常不会影响主业务流程

## 扩展建议

1. **缓存预热**：可以在系统启动时预热常用查询的缓存
2. **缓存分层**：可以考虑使用多级缓存策略
3. **缓存监控**：可以集成更详细的缓存监控和告警系统
4. **缓存策略**：可以根据业务需求调整缓存过期时间和清除策略 