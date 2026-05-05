using ContatosService.Domain.Common;
using ContatosService.Domain.Entities;
using ContatosService.Domain.Enums;
using FluentAssertions;

namespace ContatosService.UnitTests.Domain;

public sealed class ContatoTests
{
    private static readonly DateOnly DataAtual = new(2026, 5, 5);

    [Fact]
    public void DeveCriarContatoValido()
    {
        var contato = CriarContatoValido();

        contato.Id.Should().NotBeEmpty();
        contato.Nome.Should().Be("Maria Silva");
        contato.DataNascimento.Should().Be(new DateOnly(1990, 3, 10));
        contato.Sexo.Should().Be(Sexo.Feminino);
        contato.EstaAtivo.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void NaoDeveCriarContatoComNomeVazio(string nome)
    {
        var act = () => new Contato(nome, new DateOnly(1990, 3, 10), Sexo.Feminino, DataAtual);

        act.Should().Throw<DomainException>()
            .WithMessage("Nome do contato é obrigatório.");
    }

    [Fact]
    public void NaoDeveCriarContatoComDataNascimentoFutura()
    {
        var act = () => new Contato("Maria Silva", DataAtual.AddDays(1), Sexo.Feminino, DataAtual);

        act.Should().Throw<DomainException>()
            .WithMessage("Data de nascimento não pode ser maior que a data atual.");
    }

    [Fact]
    public void NaoDeveCriarContatoComIdadeZero()
    {
        var act = () => new Contato("Maria Silva", new DateOnly(2026, 1, 1), Sexo.Feminino, DataAtual);

        act.Should().Throw<DomainException>()
            .WithMessage("Idade não pode ser igual a 0.");
    }

    [Fact]
    public void NaoDeveCriarContatoMenorDeIdade()
    {
        var act = () => new Contato("Maria Silva", new DateOnly(2010, 5, 6), Sexo.Feminino, DataAtual);

        act.Should().Throw<DomainException>()
            .WithMessage("Contato deve ser maior de idade.");
    }

    [Fact]
    public void DeveCalcularIdadeCorretamenteQuandoAniversarioJaPassouNoAno()
    {
        var contato = new Contato("Maria Silva", new DateOnly(2000, 5, 4), Sexo.Feminino, DataAtual);

        contato.CalcularIdade(DataAtual).Should().Be(26);
    }

    [Fact]
    public void DeveCalcularIdadeCorretamenteQuandoAniversarioAindaNaoPassouNoAno()
    {
        var contato = new Contato("Maria Silva", new DateOnly(2000, 5, 6), Sexo.Feminino, DataAtual);

        contato.CalcularIdade(DataAtual).Should().Be(25);
    }

    [Fact]
    public void DeveAtualizarContatoAtivo()
    {
        var contato = CriarContatoValido();

        contato.Atualizar("João Silva", new DateOnly(1988, 10, 1), Sexo.Masculino, DataAtual);

        contato.Nome.Should().Be("João Silva");
        contato.DataNascimento.Should().Be(new DateOnly(1988, 10, 1));
        contato.Sexo.Should().Be(Sexo.Masculino);
    }

    [Fact]
    public void NaoDeveAtualizarContatoInativo()
    {
        var contato = CriarContatoValido();
        contato.Desativar();

        var act = () => contato.Atualizar("João Silva", new DateOnly(1988, 10, 1), Sexo.Masculino, DataAtual);

        act.Should().Throw<DomainException>()
            .WithMessage("Contato inativo não pode ser editado.");
    }

    [Fact]
    public void NaoDeveAlterarDadosDoContatoQuandoAtualizacaoForInvalida()
    {
        var contato = CriarContatoValido();

        var act = () => contato.Atualizar("João Silva", new DateOnly(2010, 5, 6), Sexo.Masculino, DataAtual);

        act.Should().Throw<DomainException>()
            .WithMessage("Contato deve ser maior de idade.");
        contato.Nome.Should().Be("Maria Silva");
        contato.DataNascimento.Should().Be(new DateOnly(1990, 3, 10));
        contato.Sexo.Should().Be(Sexo.Feminino);
    }

    [Fact]
    public void DeveDesativarContatoAtivo()
    {
        var contato = CriarContatoValido();

        contato.Desativar();

        contato.EstaAtivo.Should().BeFalse();
    }

    [Fact]
    public void NaoDeveDesativarContatoJaInativo()
    {
        var contato = CriarContatoValido();
        contato.Desativar();

        var act = contato.Desativar;

        act.Should().Throw<DomainException>()
            .WithMessage("Contato já está inativo.");
    }

    [Fact]
    public void DeveAtivarContatoInativo()
    {
        var contato = CriarContatoValido();
        contato.Desativar();

        contato.Ativar();

        contato.EstaAtivo.Should().BeTrue();
    }

    [Fact]
    public void NaoDeveAtivarContatoJaAtivo()
    {
        var contato = CriarContatoValido();

        var act = contato.Ativar;

        act.Should().Throw<DomainException>()
            .WithMessage("Contato já está ativo.");
    }

    private static Contato CriarContatoValido()
    {
        return new Contato("Maria Silva", new DateOnly(1990, 3, 10), Sexo.Feminino, DataAtual);
    }
}
