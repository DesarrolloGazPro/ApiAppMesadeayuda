using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ApiAppMesaDeAyuda.Modelos
{
    public class SolicitudTicket
    {
        
        public string id { get; set; }
        public string estatus { get; set; }
        public string atendio { get; set; }
        public string fecha { get; set; }
        public string hora { get; set; }
        public string fechacreado { get; set; }
        public string tiemporespuesta { get; set; }
        public string solicitudreabrir { get; set; }
        public string reasignarticket { get; set; }

        //datos necesarios para la reasignacion

        public string tiempoFalla { get; set; }
        public string servicio {  get; set; }
        public string falla { get; set; }
        public string prioridad { get; set; }
        public string usuarioasignado { get; set; }
        
        


    }
}
