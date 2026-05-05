using ContatosService.Api.Mappers;
using ContatosService.Api.ViewModels.Contatos;
using ContatosService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContatosService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ContatosController : ControllerBacse
{
    private readonly IContatoService _contatoService;

    public ContatosController(IContatoService contatoService)
    {
        _contatoService = contatoService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ContatoViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ContatoViewModel>> Criar(
        CriarContatoViewModel viewModel,
        CancellationToken cancellationToken)
    {
        var contato = await _contatoService.CriarAsync(viewModel.ParaDto(), cancellationToken);
        var response = contato.ParaViewModel();

        return CreatedAtAction(nameof(ObterPorId), new { id = response.Id }, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<ContatoViewModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<ContatoViewModel>>> Listar(
        CancellationToken cancellationToken)
    {
        var contatos = await _contatoService.ListarAtivosAsync(cancellationToken);
        var response = contatos
            .Select(contato => contato.ParaViewModel())
            .ToList();

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ContatoViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContatoViewModel>> ObterPorId(
        Guid id,
        CancellationToken cancellationToken)
    {
        var contato = await _contatoService.ObterAtivoPorIdAsync(id, cancellationToken);

        return Ok(contato.ParaViewModel());
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(
        Guid id,
        AtualizarContatoViewModel viewModel,
        CancellationToken cancellationToken)
    {
        await _contatoService.AtualizarAsync(id, viewModel.ParaDto(), cancellationToken);

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
