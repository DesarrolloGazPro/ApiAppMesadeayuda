using System.ComponentModel.DataAnnotations;

namespace ApiAppMesaDeAyuda.Modelos
{
    public class RespuestaUsuario
    {
        public Usuarios usuarios { get; set; }
        public string Token { get; set; }
    }
}
