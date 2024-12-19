namespace ApiAppMesaDeAyuda.Modelos
{
    public class ArchivosTickets
    {
        public int Id { get; set; }
        public byte[] Archivo { get; set; }
        public string archivo_nombre { get; set; }
        public int ticket_id { get; set; }
        public int ticket_mensaje_id { get; set; }
    }
}
