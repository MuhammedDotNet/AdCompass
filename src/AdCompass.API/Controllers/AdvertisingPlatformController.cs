using AdCompass.Core.Models;
using AdCompass.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AdCompass.API.Controllers
{
    /// <summary>
    /// Контроллер для работы с рекламными площадками
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AdvertisingPlatformController : ControllerBase
    {
        private readonly IAdvertisingPlatformService _platformService;
        private readonly ILogger<AdvertisingPlatformController> _logger;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="platformService">Сервис для работы с площадками</param>
        /// <param name="logger">Логгер</param>
        public AdvertisingPlatformController(
            IAdvertisingPlatformService platformService,
            ILogger<AdvertisingPlatformController> logger)
        {
            _platformService = platformService;
            _logger = logger;
        }

        /// <summary>
        /// Загружает рекламные площадки из файла
        /// </summary>
        /// <param name="request">Запрос с содержимым файла</param>
        /// <returns>Результат загрузки</returns>
        [HttpPost("upload")]
        public async Task<ActionResult<ApiResponse<object>>> UploadPlatforms([FromBody] FileUploadRequest request)
        {
            try
            {
                _logger.LogInformation("Начало загрузки рекламных площадок");

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    
                    return BadRequest(new ApiResponse<object>($"Ошибки валидации: {string.Join(", ", errors)}"));
                }

                var loadedCount = await _platformService.LoadPlatformsFromFileAsync(request.Content);
                
                _logger.LogInformation("Загружено {Count} рекламных площадок", loadedCount);

                return Ok(new ApiResponse<object>(new { 
                    Message = $"Успешно загружено {loadedCount} рекламных площадок",
                    LoadedCount = loadedCount 
                }));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Ошибка валидации при загрузке файла");
                return BadRequest(new ApiResponse<object>(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке рекламных площадок");
                return StatusCode(500, new ApiResponse<object>("Внутренняя ошибка сервера"));
            }
        }

        /// <summary>
        /// Находит рекламные площадки для заданной локации
        /// </summary>
        /// <param name="request">Запрос с локацией</param>
        /// <returns>Список рекламных площадок</returns>
        [HttpPost("search")]
        public async Task<ActionResult<ApiResponse<LocationSearchResponse>>> SearchPlatforms([FromBody] LocationSearchRequest request)
        {
            try
            {
                _logger.LogInformation("Поиск рекламных площадок для локации: {Location}", request.Location);

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    
                    return BadRequest(new ApiResponse<LocationSearchResponse>($"Ошибки валидации: {string.Join(", ", errors)}"));
                }

                var platforms = await _platformService.FindPlatformsByLocationAsync(request.Location);
                var response = new LocationSearchResponse(request.Location, platforms);
                
                _logger.LogInformation("Найдено {Count} рекламных площадок для локации {Location}", platforms.Count, request.Location);

                return Ok(new ApiResponse<LocationSearchResponse>(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при поиске рекламных площадок для локации {Location}", request.Location);
                return StatusCode(500, new ApiResponse<LocationSearchResponse>("Внутренняя ошибка сервера"));
            }
        }

        /// <summary>
        /// Получает все загруженные рекламные площадки
        /// </summary>
        /// <returns>Список всех площадок</returns>
        [HttpGet("all")]
        public async Task<ActionResult<ApiResponse<List<AdvertisingPlatform>>>> GetAllPlatforms()
        {
            try
            {
                _logger.LogInformation("Получение всех рекламных площадок");

                var platforms = await _platformService.GetAllPlatformsAsync();
                
                _logger.LogInformation("Получено {Count} рекламных площадок", platforms.Count);

                return Ok(new ApiResponse<List<AdvertisingPlatform>>(platforms));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении всех рекламных площадок");
                return StatusCode(500, new ApiResponse<List<AdvertisingPlatform>>("Внутренняя ошибка сервера"));
            }
        }

        /// <summary>
        /// Очищает все данные
        /// </summary>
        /// <returns>Результат операции</returns>
        [HttpDelete("clear")]
        public async Task<ActionResult<ApiResponse<object>>> ClearAll()
        {
            try
            {
                _logger.LogInformation("Очистка всех данных");

                await _platformService.ClearAllAsync();
                
                _logger.LogInformation("Все данные очищены");

                return Ok(new ApiResponse<object>(new { Message = "Все данные успешно очищены" }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при очистке данных");
                return StatusCode(500, new ApiResponse<object>("Внутренняя ошибка сервера"));
            }
        }
    }
}
