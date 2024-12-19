using ApiAppMesaDeAyuda.Modelos;

namespace ApiAppMesaDeAyuda.Repositorio.Irepositorio
{
    public interface IUsuarioRepositorio
    {
        Task<RespuestaUsuario> Login(string usuario, string password);

        Task<bool> CreateUser(DatosUsuario dataUser);
    }
}
