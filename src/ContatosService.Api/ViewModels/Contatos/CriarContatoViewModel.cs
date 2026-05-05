using ContatosService.Domain.Enums;

namespace ContatosService.Api.ViewModels.Contatos;

public sealed record CriarContatoViewModel(
    string Nome,
    DateOnly DataNascimento,
    Sexo Sexo);
