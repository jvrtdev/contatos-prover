using ContatosService.Domain.Enums;

namespace ContatosService.Api.ViewModels.Contatos;

public sealed record ContatoViewModel(
    Guid Id,
    string Nome,
    DateOnly DataNascimento,
    Sexo Sexo,
    int Idade,
    StatusContato Status);
