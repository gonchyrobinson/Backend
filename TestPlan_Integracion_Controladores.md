# Test Plan para Integración de Controladores ASP.NET Core

Este documento describe el plan de pruebas para la cobertura de integración de los controladores del backend. El objetivo es asegurar que todos los endpoints funcionen correctamente, validen la entrada, manejen errores y respeten la autorización.

## Alcance
- Pruebas de integración para todos los controladores en `Backend/Controllers/`.
- Cobertura de escenarios CRUD, validaciones, errores y autorización JWT.

## Estructura Recomendada de los Tests
- Usar xUnit como framework de testing.
- Usar Moq para mocks de dependencias.
- Usar WebApplicationFactory para pruebas end-to-end.
- Cada archivo de test debe llamarse `[NombreControlador]Tests.cs`.

## Detalle de Pruebas por Controlador



### 2. StudentsController
- **GET /students**: Con JWT válido, esperar lista de estudiantes y status 200.
- **GET /students/{id}**: Con JWT válido, esperar estudiante específico o 404 si no existe.
- **POST /students**: Crear estudiante con datos válidos, esperar status 201.
- **POST /students**: Datos inválidos, esperar status 400.
- **PUT /students/{id}**: Actualizar estudiante existente, esperar status 200.
- **PUT /students/{id}**: ID inexistente, esperar status 404.
- **DELETE /students/{id}**: Borrado lógico, esperar status 204.
- **Sin JWT**: Cualquier endpoint protegido, esperar status 401.

### 3. EmpresasController
- **GET /empresas**: Listar empresas, status 200.
- **POST /empresas**: Crear empresa, status 201.
- **Validaciones**: Datos inválidos, status 400.
- **PUT/DELETE**: Actualizar/eliminar empresa, status 200/204 o 404 si no existe.

### 4. ConveniosController
- **GET /convenios**: Listar convenios, status 200.
- **POST /convenios**: Crear convenio, status 201.
- **Validaciones**: Datos inválidos, status 400.
- **PUT/DELETE**: Actualizar/eliminar convenio, status 200/204 o 404 si no existe.

### 5. Otros Controladores (Pasantias, Pagos, Auditoria, Health)
- **CRUD básico**: GET, POST, PUT, DELETE según corresponda.
- **Validaciones**: Datos inválidos, status 400.
- **Errores**: Recursos inexistentes, status 404.
- **HealthController**: GET /health debe devolver status 200 y mensaje de ok.

## Consideraciones Generales
- **Autorización**: Probar endpoints protegidos con y sin JWT.
- **Validaciones**: Probar datos faltantes, tipos incorrectos, límites de longitud.
- **Errores**: Probar manejo de excepciones y mensajes de error.
- **Datos de prueba**: Usar una base de datos en memoria o transacciones rollback.

## Ejemplo de Caso de Prueba (xUnit)
```csharp
[Fact]
public async Task GetStudent_ReturnsStudent_WhenExists()
{
    // Arrange: crear estudiante de prueba en la DB
    // Act: llamar GET /students/{id} con JWT válido
    // Assert: status 200, datos correctos
}
```

## Checklist de Cobertura
- [ ] Todos los endpoints CRUD cubiertos
- [ ] Validaciones de entrada
- [ ] Errores y excepciones
- [ ] Autorización JWT
- [ ] Casos de éxito y fallo

---

**Nota:** Adaptar los casos según la lógica específica de cada controlador y DTO. Consultar los métodos y rutas reales en cada archivo de controlador.
