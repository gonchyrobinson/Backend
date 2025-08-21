# DTOValidations - Estructura de Validaciones

Esta carpeta contiene todas las clases de validación para los DTOs del proyecto, centralizando la lógica de validación y mejorando la mantenibilidad del código.

## Estructura

### BaseValidator.cs
Clase base abstracta que proporciona métodos comunes para todas las validaciones:
- `CreateValidationResult()` - Crea un ValidationResult con formato estándar
- `IsNullOrWhiteSpace()` - Valida si un string es nulo o vacío
- `IsValidEmail()` - Valida formato de email
- `IsValidPhone()` - Valida formato de teléfono

### CommonValidations.cs
Contiene validaciones y constantes compartidas entre múltiples DTOs:
- **CarrerasValidas** - HashSet con todas las carreras válidas
- **TiposAcuerdoValidos** - Tipos de acuerdo válidos (Pasantia, PPS, otro)
- **FrecuenciasPagoValidas** - Frecuencias de pago (Mensual, Trimestral, etc.)
- **TiposContratoValidos** - Tipos de contrato válidos
- **Métodos de validación** - DNI, fechas, montos, rangos, etc.

### Validadores específicos

#### StudentDtoValidator.cs
- `ValidateStudent()` - Validación completa para estudiantes (create/update)
- `ValidateStudentSearch()` - Validación para búsquedas avanzadas

#### PasantiaDtoValidator.cs
- `ValidatePasantia()` - Validación completa con reglas de negocio
- `ValidatePasantiaBusqueda()` - Validación para filtros de búsqueda

#### PagoDtoValidator.cs
- `ValidatePago()` - Validación de pagos con límites de montos y fechas
- `ValidateMarcarPago()` - Validación específica para marcar pagos

#### ConvenioDtoValidator.cs
- `ValidateConvenio()` - Validación de convenios con fechas y representantes
- `ValidateConvenioEmpresaFiltro()` - Validación para filtros de búsqueda
- `ValidateAsignarEmpresa()` - Validación para asignación de empresas
- `ValidateCaducarConvenio()` - Validación para caducar convenios

#### EmpresaDtoValidator.cs
- `ValidateEmpresa()` - Validación completa de empresas
- `ValidateEmpresaBusqueda()` - Validación para búsquedas avanzadas

#### AuthDtoValidator.cs
- `ValidateLogin()` - Validación de login
- `ValidateRegister()` - Validación de registro con complejidad de contraseña

#### AuditoriaDtoValidator.cs
- `ValidateAuditoriaBuscar()` - Validación para búsquedas de auditoría

## Uso en DTOs

Todos los DTOs ahora implementan `IValidatableObject` y usan los validadores correspondientes:

```csharp
public class StudentCreateDto : IValidatableObject
{
    // propiedades...

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return StudentDtoValidator.ValidateStudent(
            Nombre, Apellido, Documento, Email, Carrera, Domicilio, isCreate: true);
    }
}
```

## Beneficios

1. **Centralización** - Todas las validaciones en un lugar
2. **Reutilización** - Validaciones comunes compartidas
3. **Mantenibilidad** - Cambios en un solo lugar
4. **Testabilidad** - Cada validador puede testearse independientemente
5. **Consistencia** - Mensajes y reglas uniformes
6. **Escalabilidad** - Fácil agregar nuevas validaciones

## Reglas de Negocio Implementadas

- **Pasantías**: Duración máxima 12 meses, mínima 2 meses, remuneradas
- **PPS**: Duración máxima 4 meses
- **Horas semanales**: Máximo 20 horas
- **DNI**: 7 u 8 dígitos numéricos
- **Fechas**: Rangos válidos, no futuras donde corresponde
- **Montos**: Límites razonables, no negativos
- **Emails y teléfonos**: Formatos válidos
