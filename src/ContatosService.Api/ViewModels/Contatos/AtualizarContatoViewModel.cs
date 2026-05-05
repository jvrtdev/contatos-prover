using ContatosService.Domain.Enums;

namespace ContatosService.Api.ViewModels.Contatos;

public sealed record AtualizarContatoViewModel(
    string Nome,
    DateOnly DataNascimento,
    Sexo Sexo);
