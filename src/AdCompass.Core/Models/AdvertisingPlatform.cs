using System.Collections.Generic;

namespace AdCompass.Core.Models
{
    /// <summary>
    /// Модель рекламной площадки
    /// </summary>
    public class AdvertisingPlatform
    {
        /// <summary>
        /// Название рекламной площадки
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Список локаций, в которых действует площадка
        /// </summary>
        public List<string> Locations { get; set; } = new List<string>();

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public AdvertisingPlatform()
        {
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="name">Название площадки</param>
        /// <param name="locations">Список локаций</param>
        public AdvertisingPlatform(string name, List<string> locations)
        {
            Name = name;
            Locations = locations;
        }
    }
}
