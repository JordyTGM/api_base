using Application.CoberturaPlan;
using Domain.DTO.CoberturaPlan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sac_core.autorizacion;

namespace WebApiRetencionClientes.Controllers
{
    //[AuthorizeSac]
    [Route("api/Coberturas")]
    [ApiController]
    public class CoberturasController : BaseApiController
    {

        [HttpPost]
        [Route("ObtenerImagenesCoberturas")]
        public async Task<IActionResult> SolicitarTicket([FromBody] SolicitudCoberturasDto solicitudCobertura)
        {
            try
            {
                var resultado = await Mediator.Send(new CoberturaPlanQuery.Query { SolicitudCoberturasDto = solicitudCobertura });
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }


    }
}
