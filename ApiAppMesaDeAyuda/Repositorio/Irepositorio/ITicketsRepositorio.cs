using ApiAppMesaDeAyuda.Modelos;

namespace ApiAppMesaDeAyuda.Repositorio.Irepositorio
{
    public interface ITicketsRepositorio
    {
        Task<TicketMensajeArchivo> consultaTicket(string ticket_id);
        Task<bool> crearMensaje(MensajeTicket mensaje);
        Task<List<Personal>> consultaPersonal(string departamento);
        Task<List<Tickets>> consultaTickets(string departamento, string usuarioClavePerfil, string usuarioId);
        Task<List<Servicios>> consultaServicios();
        Task<List<Fallas>> consultaFallas(int clasificacionID);
        Task<List<Prioridades>> consultaPrioridades();

        Task<bool> updateTicket(SolicitudTicket ticket);
        Task<List<ArchivosTickets>> consultaArchivo(string idMensaje);

        Task<string> consultaareAignada(string ticket);


    }
}
