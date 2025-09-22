using AdCompass.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdCompass.Core.Services
{
    /// <summary>
    /// Интерфейс сервиса для работы с рекламными площадками
    /// </summary>
    public interface IAdvertisingPlatformService
    {
        /// <summary>
        /// Загружает рекламные площадки из файла, полностью перезаписывая существующие данные
        /// </summary>
        /// <param name="fileContent">Содержимое файла</param>
        /// <returns>Количество загруженных площадок</returns>
        Task<int> LoadPlatformsFromFileAsync(string fileContent);

        /// <summary>
        /// Находит рекламные площадки для заданной локации
        /// </summary>
        /// <param name="location">Локация для поиска</param>
        /// <returns>Список названий рекламных площадок</returns>
        Task<List<string>> FindPlatformsByLocationAsync(string location);

        /// <summary>
        /// Получает все загруженные рекламные площадки
        /// </summary>
        /// <returns>Список всех площадок</returns>
        Task<List<AdvertisingPlatform>> GetAllPlatformsAsync();

        /// <summary>
        /// Очищает все данные
        /// </summary>
        Task ClearAllAsync();
    }
}
