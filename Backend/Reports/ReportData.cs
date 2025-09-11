namespace Backend.Reports
{
    /// <summary>
    /// DTO plano con todos los campos requeridos por el template PDF de Contrato Pasantía Estudiante.
    /// </summary>
    public class ReportData
    {
        // Convenio
        public string? ConvenioRepresentanteEmpresa { get; set; }
        public string? ConvenioDNIRepresentante { get; set; }
        public string? ConvenioRepresentanteFacultad { get; set; }
        public string? ConvenioDNIRepresentanteFacultad { get; set; }
        public string? ConvenioDomicilioLegal { get; set; }
        public string? NumeroConvenio { get; set; }

        // Pasantía
        public string? AsignacionMensual { get; set; }
        public string? ObraSocial { get; set; }
        public string? Art { get; set; }
        public string? TutorEmpresa { get; set; }
        public string? TutorFacultad { get; set; }
        public string? DniTutorFacultad { get; set; }
        public string? Tramite { get; set; }
        public string? Observaciones { get; set; }
        public string? FechaInicio { get; set; }
        public string? FechaFin { get; set; }
        public string? TipoAcuerdo { get; set; }
        public string? HorasSemanales { get; set; }    

        // Estudiante
        public string? EstudianteApellido { get; set; }
        public string? EstudianteNombre { get; set; }
        public string? EstudianteDNI { get; set; }
        public string? EmpresaNombre { get; set; }
        public string? EstudianteCarrera { get; set; }
        public string? EstudianteEmail { get; set; }
        public string? EstudianteAreaDeTrabajo { get; set; }
        public string? EmpresaEncargado { get; set; }
        public string? EmpresaCelular { get; set; }
        public string? EmpresaCorreoElectronico { get; set; }
        public string? EmpresaTipoContrato { get; set; }
        public string? EstudianteDomicilio { get; set; }
    }
}
