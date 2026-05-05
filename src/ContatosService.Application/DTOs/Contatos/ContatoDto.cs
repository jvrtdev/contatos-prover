using ContatosService.Domain.Enums;

namespace ContatosService.Application.DTOs.Contatos;

public sealed record ContatoDto(
    Guid Id,
    string Nome,
    DateOnly DataNascimento,
    Sexo Sexo,
    int Idade,
    StatusContato Status);
