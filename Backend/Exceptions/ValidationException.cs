using System;

namespace Backend.Exceptions
{
    /// <summary>
    /// Excepción para errores de validación de datos de entrada.
    /// </summary>
    public class ValidationException : AppException
    {
        public string? Entidad { get; set; }

        public ValidationException() { }
        public ValidationException(string message, string? entidad = null) : base(message)
        {
            Entidad = entidad;
        }
        public ValidationException(string message, Exception inner, string? entidad = null) : base(message, inner)
        {
            Entidad = entidad;
        }
    }
}
