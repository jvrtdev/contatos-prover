using ContatosService.Application.Common.Exceptions;
using ContatosService.Application.Common.Interfaces;
using ContatosService.Application.DTOs.Contatos;
using ContatosService.Application.Services;
using ContatosService.Domain.Entities;
using ContatosService.Domain.Enums;
using ContatosService.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace ContatosService.UnitTests.Application;

public sealed class ContatoServiceTests
{
    private static readonly DateOnly DataAtual = new(2026, 5, 5);

    [Fact]
    public async Task DeveCriarContatoValido()
    {
        var repo = new Mock<IContatoRepository>();
        var service = CriarService(repo);
        Contato? contatoAdicionado = null;

        repo.Setup(x => x.AdicionarAsync(It.IsAny<Contato>(), It.IsAny<CancellationToken>()))
            .Callback<Contato, CancellationToken>((contato, _) => contatoAdicionado = contato)
            .Returns(Task.CompletedTask);
        repo.Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var dto = new CriarContatoDto("Maria Silva", new DateOnly(1990, 3, 10), Sexo.Feminino);

        var resultado = await service.CriarAsync(dto);

        contatoAdicionado.Should().NotBeNull();
        resultado.Nome.Should().Be("Maria Silva");
        resultado.Idade.Should().Be(36);
        resultado.Status.Should().Be(StatusContato.Ativo);
        repo.Verify(x => x.AdicionarAsync(It.IsAny<Contato>(), It.IsAny<CancellationToken>()), Times.Once);
        repo.Verify(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeveListarApenasContatosAtivos()
    {
        var repo = new Mock<IContatoRepository>();
        var service = CriarService(repo);
        var contato = CriarContatoValido();

        repo.Setup(x => x.ListarAtivosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Contato> { contato });

        var resultado = await service.ListarAtivosAsync();

        resultado.Should().ContainSingle();
        resultado.Should().OnlyContain(x => x.Status == StatusContato.Ativo);
        repo.Verify(x => x.ListarAtivosAsync(It.IsAny<CancellationToken>()), Times.Once);
        repo.Verify(x => x.ListarAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeveObterContatoAtivoPorId()
    {
        var repo = new Mock<IContatoRepository>();
        var service = CriarService(repo);
        var contato = CriarContatoValido();

        repo.Setup(x => x.ObterAtivoPorIdAsync(contato.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contato);

        var resultado = await service.ObterAtivoPorIdAsync(contato.Id);

        resultado.Id.Should().Be(contato.Id);
        resultado.Idade.Should().Be(36);
        repo.Verify(x => x.ObterAtivoPorIdAsync(contato.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeveLancarNotFoundExceptionAoObterContatoInexistente()
    {
        var repo = new Mock<IContatoRepository>();
        var service = CriarService(repo);
        var id = Guid.NewGuid();

        repo.Setup(x => x.ObterAtivoPorIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Contato?)null);

        var act = () => service.ObterAtivoPorIdAsync(id);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Contato não encontrado.");
    }

    [Fact]
    public async Task DeveAtualizarContatoAtivo()
    {
        var repo = new Mock<IContatoRepository>();
        var service = CriarService(repo);
        var contato = CriarContatoValido();
        var dto = new AtualizarContatoDto("João Silva", new DateOnly(1988, 10, 1), Sexo.Masculino);

        repo.Setup(x => x.ObterAtivoPorIdAsync(contato.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contato);
        repo.Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        await service.AtualizarAsync(contato.Id, dto);

        contato.Nome.Should().Be("João Silva");
        contato.DataNascimento.Should().Be(new DateOnly(1988, 10, 1));
        contato.Sexo.Should().Be(Sexo.Masculino);
        repo.Verify(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeveLancarNotFoundExceptionAoAtualizarContatoInexistenteOuInativo()
    {
        var repo = new Mock<IContatoRepository>();
        var service = CriarService(repo);
        var id = Guid.NewGuid();
        var dto = new AtualizarContatoDto("João Silva", new DateOnly(1988, 10, 1), Sexo.Masculino);

        repo.Setup(x => x.ObterAtivoPorIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Contato?)null);

        var act = () => service.AtualizarAsync(id, dto);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Contato não encontrado.");
        repo.Verify(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeveDesativarContatoAtivo()
    {
        var repo = new Mock<IContatoRepository>();
        var service = CriarService(repo);
        var contato = CriarContatoValido();

        repo.Setup(x => x.ObterPorIdAsync(contato.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contato);
        repo.Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        await service.DesativarAsync(contato.Id);

        contato.EstaAtivo.Should().BeFalse();
        repo.Verify(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeveAtivarContatoInativo()
    {
        var repo = new Mock<IContatoRepository>();
        var service = CriarService(repo);
        var contato = CriarContatoValido();
        contato.Desativar();

        repo.Setup(x => x.ObterPorIdAsync(contato.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contato);
        repo.Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        await service.AtivarAsync(contato.Id);

        contato.EstaAtivo.Should().BeTrue();
        repo.Verify(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeveExcluirContatoExistente()
    {
        var repo = new Mock<IContatoRepository>();
        var service = CriarService(repo);
        var contato = CriarContatoValido();

        repo.Setup(x => x.ObterPorIdAsync(contato.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contato);
        repo.Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        await service.ExcluirAsync(contato.Id);

        repo.Verify(x => x.Remover(contato), Times.Once);
        repo.Verify(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeveLancarNotFoundExceptionAoExcluirContatoInexistente()
    {
        var repo = new Mock<IContatoRepository>();
        var service = CriarService(repo);
        var id = Guid.NewGuid();

        repo.Setup(x => x.ObterPorIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Contato?)null);

        var act = () => service.ExcluirAsync(id);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Contato não encontrado.");
        repo.Verify(x => x.Remover(It.IsAny<Contato>()), Times.Never);
        repo.Verify(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    private static ContatoService CriarService(Mock<IContatoRepository> contatoRepository)
    {
        var dateTimeProvider = new Mock<IDateTimeProvider>();
        dateTimeProvider.SetupGet(x => x.Hoje).Returns(DataAtual);

        return new ContatoService(contatoRepository.Object, dateTimeProvider.Object);
    }

    private static Contato CriarContatoValido()
    {
        return new Contato("Maria Silva", new DateOnly(1990, 3, 10), Sexo.Feminino, DataAtual);
    }
}
