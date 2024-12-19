using ApiAppMesaDeAyuda.Modelos;
using ApiAppMesaDeAyuda.Repositorio.Irepositorio;
using Microsoft.AspNetCore.Mvc;

namespace ApiAppMesaDeAyuda.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUsuarioRepositorio _ipost;

        public UserController(IUsuarioRepositorio ipost)
        {
            _ipost = ipost;
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] DatosUsuario datosUsuario)
        {
            try
            {
                if (datosUsuario == null)
                {
                    return BadRequest("No se proporcionó un objeto de usuario válido.");
                }

                var respuesta = await _ipost.CreateUser(datosUsuario);

                if (respuesta)
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(500, "No se pudo crear el usuario. Verifica los datos proporcionados.");

                }

            }
            catch (Exception ex) {
                return StatusCode(500, $"Error del servidor: {ex.Message}");
            }
            
        }
    }
}
