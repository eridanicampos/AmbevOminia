using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTest.Application.DTO.Produtos;
using ProjectTest.Application.Produtos.Commands.CreateProduto;
using ProjectTest.Application.Produtos.Commands.DeletarProduto;
using ProjectTest.Application.Produtos.Commands.UpdateProduto;
using ProjectTest.Application.Produtos.Queries.GetAll;
using ProjectTest.Domain.Helpers;

namespace ProjectTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProdutoController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly Serilog.ILogger _logger;

        public ProdutoController(IMediator mediator, Serilog.ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProdutoDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateProdutoCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.Warning("Dados inválidos fornecidos para criação do produto.");
                    return BadRequest(ModelState);
                }

                _logger.Information("Criando um novo produto.");
                var produto = await _mediator.Send(command);

                _logger.Information("Produto criado com sucesso.");

                return CreatedAtAction(nameof(CreateAsync), new { id = produto.Id }, produto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao criar o produto.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao criar o produto.");
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(ProdutoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateProdutoCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.Warning("Dados inválidos fornecidos para atualização do produto.");
                    return BadRequest(ModelState);
                }

                _logger.Information("Atualizando produto.");
                var produto = await _mediator.Send(command);

                if (produto == null)
                {
                    _logger.Warning("Produto não encontrado. {NomeProduto}", command.NomeProduto);
                    return NotFound();
                }

                _logger.Information("Produto atualizado com sucesso. ID: {NomeProduto}", command.NomeProduto);
                return Ok(produto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao atualizar o produto.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao atualizar o produto.");
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ProdutoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                _logger.Information("Obtendo todos os produtos.");
                var query = new GetAllProdutoQuery();
                var produtos = await _mediator.Send(query);

                _logger.Information("Produtos obtidos com sucesso.");
                return Ok(produtos);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao obter todos os produtos.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter todos os produtos.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                _logger.Information("Deletando produto. ID: {ProdutoId}", id);
                var command = new DeletarProdutoCommand { Id = id };
                var result = await _mediator.Send(command);

                _logger.Information("Produto deletado com sucesso. ID: {ProdutoId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao deletar o produto. ID: {ProdutoId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao deletar o produto.");
            }
        }
    }
}
