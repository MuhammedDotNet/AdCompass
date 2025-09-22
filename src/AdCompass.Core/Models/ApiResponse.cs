namespace AdCompass.Core.Models
{
    /// <summary>
    /// Базовый класс для API ответов
    /// </summary>
    /// <typeparam name="T">Тип данных в ответе</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Успешность операции
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Данные ответа
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Конструктор для успешного ответа
        /// </summary>
        /// <param name="data">Данные</param>
        public ApiResponse(T data)
        {
            Success = true;
            Data = data;
        }

        /// <summary>
        /// Конструктор для ответа с ошибкой
        /// </summary>
        /// <param name="errorMessage">Сообщение об ошибке</param>
        public ApiResponse(string errorMessage)
        {
            Success = false;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public ApiResponse()
        {
        }
    }
}
