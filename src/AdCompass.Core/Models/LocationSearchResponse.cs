using System.Collections.Generic;

namespace AdCompass.Core.Models
{
    /// <summary>
    /// Ответ с результатами поиска рекламных площадок
    /// </summary>
    public class LocationSearchResponse
    {
        /// <summary>
        /// Запрашиваемая локация
        /// </summary>
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Список найденных рекламных площадок
        /// </summary>
        public List<string> Platforms { get; set; } = new List<string>();

        /// <summary>
        /// Количество найденных площадок
        /// </summary>
        public int Count => Platforms.Count;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public LocationSearchResponse()
        {
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="location">Запрашиваемая локация</param>
        /// <param name="platforms">Список площадок</param>
        public LocationSearchResponse(string location, List<string> platforms)
        {
            Location = location;
            Platforms = platforms;
        }
    }
}
