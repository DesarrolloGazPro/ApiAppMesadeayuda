using ApiAppMesaDeAyuda.Modelos;
using ApiAppMesaDeAyuda.Repositorio.Irepositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ApiAppMesaDeAyuda.Controllers
{
    [Route("api/tickets")]
    [ApiController]
    public class TicketsController : Controller
    {
        private readonly ITicketsRepositorio _ipost;

        public TicketsController(ITicketsRepositorio ipost)
        {
            _ipost = ipost; 
        }

        [HttpPut("updateTicket")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> updateTicket([FromBody] SolicitudTicket ticket)
        {


            var update = await _ipost.updateTicket(ticket);

            if (!update)
            {
                return NotFound(new
                {
                    message = "No se pudo actulizar el registro",
                });
            }

            return Ok();

        }

        [HttpGet("prioridades")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> consultaPrioridades()
        {
            var fallas = await _ipost.consultaPrioridades();

            if (fallas == null || !fallas.Any())
            {
                return NotFound(new
                {
                    message = "No se encontraron datos.",
                });
            }

            return Ok(fallas);

        }

        [HttpGet("fallas/{clasificacionID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> consultaFallas(int clasificacionID)
        {
            var fallas = await _ipost.consultaFallas(clasificacionID);

            if (fallas == null || !fallas.Any())
            {
                return NotFound(new
                {
                    message = "No se encontraron datos.",
                });
            }

            return Ok(fallas);
        }

        [HttpGet("area")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> consultaServicios()
        {
            var servicios = await _ipost.consultaServicios();

            if (servicios == null || !servicios.Any())
            {
                return NotFound(new
                {
                    message = "No se encontraron datos.",
                });
            }

            return Ok(servicios);
        }

        [HttpGet("personal/{departamento}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> consultaPersonal(string departamento)
        {
            var personal = await _ipost.consultaPersonal(departamento);

            if (personal == null || !personal.Any())
            {
                return NotFound(new
                {
                    message = "No se encontraron datos.",
                });
            }

            return Ok(personal);
        }

        [HttpGet("consultaTiket/{ticket_id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> consultaTicket(string ticket_id)
        {
           var ticket = await _ipost.consultaTicket(ticket_id);

            if (ticket.tickets == null && ticket.ticketsMensajes == null && ticket.archivosTickets == null)
            {
                return NotFound(new
                {
                    message = "No se encontraron datos.",                    
                });
            }

           return Ok(ticket);
        }

        [HttpGet("consultaArchivo/{idMensaje}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> consultaArchivo(string idMensaje)
        {
            var archivo = await _ipost.consultaArchivo(idMensaje);

            if (archivo == null)
            {
                return NotFound(new
                {
                    message = "No se encontraron datos.",
                });
            }

            return Ok(archivo);
        }
        [HttpGet("consultaareAignada/{clave}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> consultaareAignada(string clave)
        {
            string area = await _ipost.consultaareAignada(clave);

            if (area == "")
            {
                return NotFound(new
                {
                    message = "No se encontraron datos.",
                });
            }

            return Ok(area);
        }

        
        [HttpGet("consultaTikets/{departamento}/{usuarioClavePerfil}/{usuarioId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> consultaTikets(string departamento, string usuarioClavePerfil, string usuarioId)
        {
            var ticket = await _ipost.consultaTickets(departamento, usuarioClavePerfil, usuarioId);

            if (ticket == null || !ticket.Any())
            {
                return NotFound(new
                {
                    message = "No se encontraron datos.",
                });
            }

            return Ok(ticket);
        }

        [HttpPost("crearMensaje")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> crearMensaje([FromBody] MensajeTicket mensaje)
        {
            try
            {
                if (mensaje == null)
                {
                    return BadRequest("No se proporcionó un mensaje válido.");
                }

                var respuesta = await _ipost.crearMensaje(mensaje);

                if (respuesta)
                {
                    return Ok("Mensaje Agregado correctamente");
                }
                else
                {
                    return StatusCode(500, "No se pudo crear el mensaje. Verifica los datos proporcionados.");

                }
              
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error del servidor: {ex.Message}");
            }
        }
    }
}
