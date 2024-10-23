using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTest.Application.DTO.EnderecoEntrega;
using ProjectTest.Application.Interfaces;
using ProjectTest.Domain.Helpers;

namespace ProjectTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EnderecoEntregaController : BaseController
    {
        private readonly IEnderecoEntregaService _enderecoEntregaService;

        public EnderecoEntregaController(IEnderecoEntregaService enderecoEntregaService)
        {
            _enderecoEntregaService = enderecoEntregaService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<EnderecoEntregaDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEnderecosAsync()
        {
            var enderecos = await _enderecoEntregaService.GetAsync();
            return Ok(enderecos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EnderecoEntregaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEnderecoByIdAsync(string id)
        {
            var endereco = await _enderecoEntregaService.GetById(id);
            if (endereco == null)
            {
                return NotFound();
            }

            return Ok(endereco);
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(EnderecoEntregaDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] EnderecoEntregaParamDTO enderecoParamDTO)
        {
            if (enderecoParamDTO == null)
            {
                return BadRequest();
            }

            var endereco = await _enderecoEntregaService.CreateAsync(enderecoParamDTO);
            return CreatedAtAction(nameof(GetEnderecoByIdAsync), new { id = endereco.Id }, endereco);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEnderecoAsync(string id, [FromBody] EnderecoEntregaModifyDTO enderecoModifyDTO)
        {
            if (id != enderecoModifyDTO.Id.ToString())
            {
                return BadRequest();
            }

            var updated = await _enderecoEntregaService.UpdateAsync(enderecoModifyDTO);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEnderecoAsync(string id)
        {
            var deleted = await _enderecoEntregaService.Delete(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("cep/{cep}")]
        [ProducesResponseType(typeof(EnderecoEntregaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ObterEnderecoPorCepAsync(string cep)
        {
            try
            {
                var endereco = await _enderecoEntregaService.ObterEnderecoPorCepAsync(cep);
                return Ok(endereco);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
