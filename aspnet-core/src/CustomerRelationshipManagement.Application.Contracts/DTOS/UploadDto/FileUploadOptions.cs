using System.Collections.Generic;

namespace CustomerRelationshipManagement.DTOS.UploadDto
{
    /// <summary>
    /// 上传文件配置
    /// </summary>
    public class FileUploadOptions
    {
        /// <summary>
        /// 基础路径
        /// </summary>
        public string BasePath { get; set; }
        /// <summary>
        /// 文件最大大小
        /// </summary>
        public int MaxFileSize { get; set; }
        /// <summary>
        /// 允许的扩展名
        /// </summary>
        public string[] AllowedExtensions { get; set; }
        /// <summary>
        // 图片质量
        /// </summary>
        public int CompressionQuality { get; set; }
        /// <summary>
        /// 是否生成缩略图
        /// </summary>
        public bool GenerateThumbnail { get; set; }
        /// <summary>
        /// 缩略图大小
        /// </summary>
        public ThumbnailSizeOptions ThumbnailSize { get; set; }

        /// <summary>
        /// 图片类型
        /// </summary>
        public Dictionary<string, ImageTypeOptions> ImageTypes { get; set; }
    }
    /// <summary>
    /// 缩略图尺寸
    /// </summary>
    public class ThumbnailSizeOptions
    {
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; set; }
    }
    /// <summary>
    /// 图片类型
    /// </summary>
    public class ImageTypeOptions
    {
        /// <summary>
        /// 最大尺寸
        /// </summary>
        public int MaxSize { get; set; }
        /// <summary>
        /// 是否压缩
        /// </summary>
        public bool Compress { get; set; }
        /// <summary>
        /// 存储路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public int? ExpireHours { get; set; }
    }
}
