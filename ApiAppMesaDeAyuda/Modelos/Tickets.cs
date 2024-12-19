using Newtonsoft.Json;

namespace ApiAppMesaDeAyuda.Modelos
{
    public class Tickets
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public int Id { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string Clave { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string estatus { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string Servicio { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string Falla { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string Prioridad { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string Asunto { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string Mensaje { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string? fecha_creado { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string? fecha_atendido { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string? fecha_cerrado { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string? tiempo_respuesta { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string? tiempo_atencion { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string Usuario { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string? usuario_id { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string usuario_sucursal_clave { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string usuario_sucursal_nombre { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string creado_por_perfil { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string esta_asignado { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string usuario_asignado { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string? solicitud_reabrir { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string cliente_nombre { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string cliente_correo { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string Reabierto { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string Atendio { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string Condicion { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string Nivel { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string En_Estacion { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string? Latitud { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string? Longitud { get; set; }

    }
}
