using ContatosService.Application.DTOs.Contatos;
using ContatosService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContatosService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ContatosController : ControllerBase
{
    private readonly IContatoService _contatoService;

    public ContatosController(IContatoService contatoService)
    {
        _contatoService = contatoService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ContatoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ContatoDto>> Criar(
        [FromBody] CriarContatoDto dto,
        CancellationToken cancellationToken)
    {
        var contato = await _contatoService.CriarAsync(dto, cancellationToken);

        return CreatedAtAction(nameof(ObterPorId), new { id = contato.Id }, contato);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<ContatoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<ContatoDto>>> Listar(
        CancellationToken cancellationToken)
    {
        var contatos = await _contatoService.ListarAtivosAsync(cancellationToken);

        return Ok(contatos);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ContatoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContatoDto>> ObterPorId(
        Guid id,
        CancellationToken cancellationToken)
    {
        var contato = await _contatoService.ObterAtivoPorIdAsync(id, cancellationToken);

        return Ok(contato);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(
        Guid id,
        [FromBody] AtualizarContatoDto dto,
        CancellationToken cancellationToken)
    {
        await _contatoService.AtualizarAsync(id, dto, cancellationToken);

        return NoContent();
    }

    [HttpPatch("{id:guid}/ativar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Ativar(Guid id, CancellationToken cancellationToken)
    {
        await _contatoService.AtivarAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpPatch("{id:guid}/desativar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Desativar(Guid id, CancellationToken cancellationToken)
    {
        await _contatoService.DesativarAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Excluir(Guid id, CancellationToken cancellationToken)
    {
        await _contatoService.ExcluirAsync(id, cancellationToken);

        return NoContent();
    }
}
