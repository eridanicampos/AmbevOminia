using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ProjectTest.Application.Compra.Commands.CancelarCompra;
using ProjectTest.Application.Compra.Commands.CancelarItem;
using ProjectTest.Application.Compra.Commands.CreateCompra;
using ProjectTest.Application.Compra.Commands.UpdateCompra;
using ProjectTest.Application.Compra.Queries.GetAllCompra;
using ProjectTest.Application.DTO.Venda;
using ProjectTest.Application.Interfaces;
using ProjectTest.Domain.Helpers;

namespace ProjectTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompraController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly Serilog.ILogger _logger;

        public CompraController(IMediator mediator, Serilog.ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }


        [HttpPost]
        [ProducesResponseType(typeof(VendaDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCompraCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.Warning("Dados inválidos fornecidos para a criação da venda.");
                    return BadRequest(ModelState);
                }

                _logger.Information("Criando uma nova venda.");
                var obj = await _mediator.Send(command);

                _logger.Information("Fim da Criação de venda.");

                return CreatedAtAction(nameof(CreateAsync), new { id = obj.Id }, obj);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao criar a venda.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao criar a venda.");
            }
        }


        [HttpPut]
        [ProducesResponseType(typeof(VendaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateCompraCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.Warning("Dados inválidos fornecidos para a atualização da venda.");
                    return BadRequest(ModelState);
                }

                _logger.Information("Atualizando venda.");
                var obj = await _mediator.Send(command);

                if (obj == null)
                {
                    _logger.Warning("Venda não encontrada para atualização. ID: {VendaId}", command.VendaId);
                    return NotFound();
                }

                _logger.Information("Fim da atualização da venda. ID: {VendaId}", command.VendaId);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao atualizar a venda.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao atualizar a venda.");
            }
        }

        [HttpPut("{id}/cancelar")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelarVendaAsync(Guid id)
        {
            try
            {
                _logger.Information("Cancelando venda. ID da venda: {VendaId}", id);
                var command = new CancelarCompraCommand { VendaId = id };
                await _mediator.Send(command);

                _logger.Information("Venda cancelada com sucesso. ID da venda: {VendaId}", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao cancelar a venda. ID: {VendaId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao cancelar a venda.");
            }
        }

        [HttpPut("{vendaId}/itens/{itemId}/cancelar")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelarItemAsync(Guid vendaId, Guid itemId)
        {
            try
            {
                _logger.Information("Cancelando item da venda. ID da venda: {VendaId}, ID do item: {ItemId}", vendaId, itemId);
                var command = new CancelarItemCommand { VendaId = vendaId, ItemId = itemId };
                await _mediator.Send(command);

                _logger.Information("Item da venda cancelado com sucesso. ID da venda: {VendaId}, ID do item: {ItemId}", vendaId, itemId);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao cancelar o item da venda. ID da venda: {VendaId}, ID do item: {ItemId}", vendaId, itemId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao cancelar o item da venda.");
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<VendaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                _logger.Information("Obtendo todas as vendas do banco de dados.");
                var query = new GetAllComprasQuery();
                var vendas = await _mediator.Send(query);

                _logger.Information("Todas as vendas foram obtidas com sucesso.");
                return Ok(vendas);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao obter todas as vendas.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter todas as vendas.");
            }
        }

    }
}
