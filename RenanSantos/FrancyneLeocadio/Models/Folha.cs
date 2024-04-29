namespace FrancyneLeocadio.Models;

using System.ComponentModel.DataAnnotations;

public class Folha
{
    public Folha(double valor, int quantidade, int mes, int ano, int funcionarioId)
    {
        Valor = valor;
        Quantidade = quantidade;
        Mes = mes;
        Ano = ano;
        FuncionarioId = funcionarioId;
    }

    public int FolhaId { get; set; }
    public double Valor { get; set; }
    public int Quantidade { get; set; }
    public int Mes { get; set; }
    public int Ano { get; set; }
    public double SalarioBruto { get; set; }
    public double ImpostoIrrf { get; set; }
    public double ImpostoInns { get; set; }
    public double ImpostFgts { get; set; }
    public double SalarioLiquido { get; set; }
    public int FuncionarioId { get; set; }
    public Funcionario? Funcionario { get; set; }
}