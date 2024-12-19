using ApiAppMesaDeAyuda.Modelos;
using ApiAppMesaDeAyuda.Repositorio.Irepositorio;
using Microsoft.AspNetCore.Mvc;

namespace ApiAppMesaDeAyuda.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUsuarioRepositorio _ipost;

        public LoginController(IUsuarioRepositorio ipost)
        {
            _ipost = ipost;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] Usuarios usuarios)
        {

            if (usuarios == null)
            {
                return BadRequest("No se proporcionó un objeto de usuario válido.");
            }

            var respuesta = await _ipost.Login(usuarios.Usuario!, usuarios.Contrasena!);

            if (respuesta.Token == null)
            {
                return NotFound("No se encontró un usuario con las credenciales proporcionadas.");
            }

            return Ok(respuesta);
        }
    }
}
