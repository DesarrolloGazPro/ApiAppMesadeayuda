namespace ApiAppMesaDeAyuda.Modelos
{
    public class Usuarios
    {
        public int? Id { get; set; }
        public int? EmpleadoID { get; set; }
        public string Usuario { get; set; }
        public string? Nombre { get; set; }
        public string Contrasena { get; set; }
        public string? Correo { get; set; }
        public string? Estatus { get; set; }
        public string? departamento_clave { get; set; }
        public string? perfil_clave { get; set; }
        public string? Depto_biotime { get; set; }
        

    }
}
