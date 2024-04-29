namespace FrancyneLeocadio.Models;

using System.ComponentModel.DataAnnotations;

public class Funcionario
{
    public Funcionario(string nome, string cpf)
    {
        Nome = nome;
        Cpf = cpf;
    }

    public int FuncionarioId { get; set; }
    public string? Nome { get; set; }
    public string? Cpf { get; set; }
}