using Newtonsoft.Json;

namespace ApiAppMesaDeAyuda.Modelos
{
    public class TicketsMensajes
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int id { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string mensaje { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime fecha_creado { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ticket_id { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string esMensajeSoporte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string usuario { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int usuario_id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string usuario_nombre { get; set; }
    }
}
