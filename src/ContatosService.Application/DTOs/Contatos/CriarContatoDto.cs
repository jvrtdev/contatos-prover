using ContatosService.Domain.Enums;

namespace ContatosService.Application.DTOs.Contatos;

public sealed record CriarContatoDto(
    string Nome,
    DateOnly DataNascimento,
    Sexo Sexo);
