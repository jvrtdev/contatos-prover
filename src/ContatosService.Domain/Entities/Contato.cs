using ContatosService.Domain.Common;
using ContatosService.Domain.Enums;

namespace ContatosService.Domain.Entities;

public sealed class Contato : Entity
{
    private const int IdadeMinima = 18;

    private Contato()
    {
        Nome = string.Empty;
    }

    public Contato(string nome, DateOnly dataNascimento, Sexo sexo, DateOnly dataAtual)
    {
        var nomeValidado = ValidarNome(nome);
        ValidarDataNascimento(dataNascimento, dataAtual);
        ValidarMaioridade(dataNascimento, dataAtual);

        Id = Guid.NewGuid();
        Nome = nomeValidado;
        DataNascimento = dataNascimento;
        Sexo = sexo;
        Status = StatusContato.Ativo;
    }

    public string Nome { get; private set; }
    public DateOnly DataNascimento { get; private set; }
    public Sexo Sexo { get; private set; }
    public StatusContato Status { get; private set; }
    public bool EstaAtivo => Status == StatusContato.Ativo;

    public void Atualizar(string nome, DateOnly dataNascimento, Sexo sexo, DateOnly dataAtual)
    {
        if (!EstaAtivo)
        {
            throw new DomainException("Contato inativo não pode ser editado.");
        }

        var nomeValidado = ValidarNome(nome);
        ValidarDataNascimento(dataNascimento, dataAtual);
        ValidarMaioridade(dataNascimento, dataAtual);

        Nome = nomeValidado;
        DataNascimento = dataNascimento;
        Sexo = sexo;
    }

    public void Ativar()
    {
        if (EstaAtivo)
        {
            throw new DomainException("Contato já está ativo.");
        }

        Status = StatusContato.Ativo;
    }

    public void Desativar()
    {
        if (!EstaAtivo)
        {
            throw new DomainException("Contato já está inativo.");
        }

        Status = StatusContato.Inativo;
    }

    public int CalcularIdade(DateOnly dataAtual)
    {
        var idade = dataAtual.Year - DataNascimento.Year;

        if (dataAtual < DataNascimento.AddYears(idade))
        {
            idade--;
        }

        return idade;
    }

    private static string ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new DomainException("Nome do contato é obrigatório.");
        }

        return nome.Trim();
    }

    private static void ValidarDataNascimento(DateOnly dataNascimento, DateOnly dataAtual)
    {
        if (dataNascimento == default)
        {
            throw new DomainException("Data de nascimento é obrigatória.");
        }

        if (dataNascimento > dataAtual)
        {
            throw new DomainException("Data de nascimento não pode ser maior que a data atual.");
        }
    }

    private static void ValidarMaioridade(DateOnly dataNascimento, DateOnly dataAtual)
    {
        var idade = CalcularIdade(dataNascimento, dataAtual);

        if (idade == 0)
        {
            throw new DomainException("Idade não pode ser igual a 0.");
        }

        if (idade < IdadeMinima)
        {
            throw new DomainException("Contato deve ser maior de idade.");
        }
    }

    private static int CalcularIdade(DateOnly dataNascimento, DateOnly dataAtual)
    {
        var idade = dataAtual.Year - dataNascimento.Year;

        if (dataAtual < dataNascimento.AddYears(idade))
        {
            idade--;
        }

        return idade;
    }
}
