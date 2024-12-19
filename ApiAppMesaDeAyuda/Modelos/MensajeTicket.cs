namespace ApiAppMesaDeAyuda.Modelos
{
    public class MensajeTicket
    {
      
        public string Mensaje { get; set; }
        public string fecha_creado { get; set; }
        public string ticket_id { get; set; }
        public string esMensajeSoporte { get; set; }
        public string usuario { get; set; }
        public string usuario_id { get; set; }
        public string usuario_nombre { get; set; }

        public string archivo { get; set; }
        public string? archivo_nombre { get; set; }
       
    }
}
