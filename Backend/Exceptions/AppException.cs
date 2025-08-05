using System;

namespace Backend.Exceptions
{
    /// <summary>
    /// Excepción genérica para errores de negocio, validación o lógica de aplicación.
    /// </summary>
    public class AppException : Exception
    {
        public AppException() { }
        public AppException(string message) : base(message) { }
        public AppException(string message, Exception inner) : base(message, inner) { }
    }
}
