using Newtonsoft.Json;

namespace ApiAppMesaDeAyuda.Modelos
{
    public class TicketMensajeArchivo
    {
        public List<Tickets> tickets { get; set; }
        public List<TicketsMensajes> ticketsMensajes { get; set; }
        public List<ArchivosTickets> archivosTickets  { get; set; }

    }
}
