using ContatosService.Domain.Enums;

namespace ContatosService.Application.DTOs.Contatos;

public sealed record AtualizarContatoDto(
    string Nome,
    DateOnly DataNascimento,
    Sexo Sexo);
