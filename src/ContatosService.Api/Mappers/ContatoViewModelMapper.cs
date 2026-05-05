using ContatosService.Api.ViewModels.Contatos;
using ContatosService.Application.DTOs.Contatos;

namespace ContatosService.Api.Mappers;

public static class ContatoViewModelMapper
{
    public static CriarContatoDto ParaDto(this CriarContatoViewModel viewModel)
    {
        return new CriarContatoDto(
            viewModel.Nome,
            viewModel.DataNascimento,
            viewModel.Sexo);
    }

    public static AtualizarContatoDto ParaDto(this AtualizarContatoViewModel viewModel)
    {
        return new AtualizarContatoDto(
            viewModel.Nome,
            viewModel.DataNascimento,
            viewModel.Sexo);
    }

    public static ContatoViewModel ParaViewModel(this ContatoDto dto)
    {
        return new ContatoViewModel(
            dto.Id,
            dto.Nome,
            dto.DataNascimento,
            dto.Sexo,
            dto.Idade,
            dto.Status);
    }
}
