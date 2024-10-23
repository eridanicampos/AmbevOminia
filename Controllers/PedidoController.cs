using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTest.Application.DTO.Pedido;
using ProjectTest.Application.Interfaces;
using ProjectTest.Domain.Helpers;

namespace ProjectTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PedidoController : BaseController
    {
        private readonly IPedidoService _pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<PedidoDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPedidosAsync()
        {
            var pedidos = await _pedidoService.GetAsync();
            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PedidoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPedidoByIdAsync(string id)
        {
            var pedido = await _pedidoService.GetById(id);
            if (pedido == null)
            {
                return NotFound();
            }

            return Ok(pedido);
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(PedidoDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] PedidoParamDTO pedidoParamDTO)
        {
            if (pedidoParamDTO == null)
            {
                return BadRequest();
            }

            var pedido = await _pedidoService.CreateAsync(pedidoParamDTO);
            return CreatedAtAction(nameof(GetPedidoByIdAsync), new { id = pedido.Id }, pedido);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePedidoAsync(string id, [FromBody] PedidoModifyDTO pedidoModifyDTO)
        {
            if (id != pedidoModifyDTO.Id.ToString())
            {
                return BadRequest();
            }

            var updated = await _pedidoService.UpdateAsync(pedidoModifyDTO);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePedidoAsync(string id)
        {
            var deleted = await _pedidoService.Delete(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
