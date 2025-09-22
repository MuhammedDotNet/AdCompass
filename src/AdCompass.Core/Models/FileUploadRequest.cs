using System.ComponentModel.DataAnnotations;

namespace AdCompass.Core.Models
{
    /// <summary>
    /// Запрос на загрузку файла с рекламными площадками
    /// </summary>
    public class FileUploadRequest
    {
        /// <summary>
        /// Содержимое файла в виде строки
        /// </summary>
        [Required(ErrorMessage = "Содержимое файла обязательно")]
        public string Content { get; set; } = string.Empty;
    }
}
