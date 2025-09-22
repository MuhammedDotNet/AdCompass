using System.ComponentModel.DataAnnotations;

namespace AdCompass.Core.Models
{
    /// <summary>
    /// Запрос на поиск рекламных площадок по локации
    /// </summary>
    public class LocationSearchRequest
    {
        /// <summary>
        /// Локация для поиска (например, /ru/msk)
        /// </summary>
        [Required(ErrorMessage = "Локация обязательна для заполнения")]
        [StringLength(500, ErrorMessage = "Длина локации не должна превышать 500 символов")]
        public string Location { get; set; } = string.Empty;
    }
}
