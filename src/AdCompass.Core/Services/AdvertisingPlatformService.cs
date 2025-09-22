using AdCompass.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdCompass.Core.Services
{
    /// <summary>
    /// Сервис для работы с рекламными площадками
    /// </summary>
    public class AdvertisingPlatformService : IAdvertisingPlatformService
    {
        private readonly List<AdvertisingPlatform> _platforms;
        private readonly object _lock = new object();

        /// <summary>
        /// Конструктор
        /// </summary>
        public AdvertisingPlatformService()
        {
            _platforms = new List<AdvertisingPlatform>();
        }

        /// <summary>
        /// Загружает рекламные площадки из файла, полностью перезаписывая существующие данные
        /// </summary>
        /// <param name="fileContent">Содержимое файла</param>
        /// <returns>Количество загруженных площадок</returns>
        public async Task<int> LoadPlatformsFromFileAsync(string fileContent)
        {
            if (string.IsNullOrWhiteSpace(fileContent))
            {
                throw new ArgumentException("Содержимое файла не может быть пустым", nameof(fileContent));
            }

            return await Task.Run(() =>
            {
                lock (_lock)
                {
                    _platforms.Clear();

                    var lines = fileContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    var loadedCount = 0;

                    foreach (var line in lines)
                    {
                        try
                        {
                            var trimmedLine = line.Trim();
                            if (string.IsNullOrWhiteSpace(trimmedLine))
                                continue;

                            var parts = trimmedLine.Split(':', 2);
                            if (parts.Length != 2)
                            {
                                continue;
                            }

                            var platformName = parts[0].Trim();
                            var locationsString = parts[1].Trim();

                            if (string.IsNullOrWhiteSpace(platformName) || string.IsNullOrWhiteSpace(locationsString))
                                continue;

                            var locations = locationsString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(loc => loc.Trim())
                                .Where(loc => !string.IsNullOrWhiteSpace(loc))
                                .ToList();

                            if (locations.Any())
                            {
                                _platforms.Add(new AdvertisingPlatform(platformName, locations));
                                loadedCount++;
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    return loadedCount;
                }
            });
        }

        /// <summary>
        /// Находит рекламные площадки для заданной локации
        /// </summary>
        /// <param name="location">Локация для поиска</param>
        /// <returns>Список названий рекламных площадок</returns>
        public async Task<List<string>> FindPlatformsByLocationAsync(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                return new List<string>();
            }

            return await Task.Run(() =>
            {
                lock (_lock)
                {
                    var result = new List<string>();

                    foreach (var platform in _platforms)
                    {
                        foreach (var platformLocation in platform.Locations)
                        {
                            if (IsLocationMatch(location, platformLocation))
                            {
                                result.Add(platform.Name);
                                break; // Добавляем площадку только один раз
                            }
                        }
                    }

                    return result;
                }
            });
        }

        /// <summary>
        /// Получает все загруженные рекламные площадки
        /// </summary>
        /// <returns>Список всех площадок</returns>
        public async Task<List<AdvertisingPlatform>> GetAllPlatformsAsync()
        {
            return await Task.Run(() =>
            {
                lock (_lock)
                {
                    return _platforms.ToList();
                }
            });
        }

        /// <summary>
        /// Очищает все данные
        /// </summary>
        public async Task ClearAllAsync()
        {
            await Task.Run(() =>
            {
                lock (_lock)
                {
                    _platforms.Clear();
                }
            });
        }

        /// <summary>
        /// Проверяет, подходит ли запрашиваемая локация к локации площадки
        /// </summary>
        /// <param name="requestedLocation">Запрашиваемая локация</param>
        /// <param name="platformLocation">Локация площадки</param>
        /// <returns>True, если локации совпадают</returns>
        private static bool IsLocationMatch(string requestedLocation, string platformLocation)
        {
            var normalizedRequested = NormalizeLocation(requestedLocation);
            var normalizedPlatform = NormalizeLocation(platformLocation);

            if (normalizedRequested.Equals(normalizedPlatform, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (normalizedRequested.StartsWith(normalizedPlatform + "/", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (normalizedPlatform.StartsWith(normalizedRequested + "/", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Нормализует локацию (убирает лишние слеши, приводит к единому формату)
        /// </summary>
        /// <param name="location">Локация для нормализации</param>
        /// <returns>Нормализованная локация</returns>
        private static string NormalizeLocation(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                return string.Empty;

            var normalized = location.Trim();
            
            normalized = normalized.Trim('/');
            
            if (!normalized.StartsWith("/"))
            {
                normalized = "/" + normalized;
            }

            return normalized;
        }
    }
}
