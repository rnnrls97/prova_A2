using FrancyneLeocadio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

app.MapPost("/api/funcionario/cadastrar", ([FromBody] Funcionario funcionarioRequest, [FromServices] AppDbContext dbContext) =>
{
    dbContext.Funcionarios.Add(funcionarioRequest);
    dbContext.SaveChanges();

    return Results.Created();
});

app.MapGet("api/funcionario/listar", ([FromServices] AppDbContext dbContext) =>
{
    if (!dbContext.Funcionarios.Any())
    {
        return Results.NotFound("Nenhuma folha cadastrada");
    }

    return Results.Ok(dbContext.Funcionarios.ToList());
});

app.MapPost("/api/folha/cadastrar", ([FromBody] Folha folhaRequest, [FromServices] AppDbContext dbContext) =>
{
    Funcionario? f = dbContext.Funcionarios.Find(folhaRequest.FuncionarioId);

    if (f is null)
    {
        return Results.NotFound("Funcionário não encontrado");
    }

    folhaRequest.SalarioBruto = folhaRequest.Valor * folhaRequest.Quantidade;

    double aliquotaIrrf = 0;
    double parcelaIrrf = 0;
    double inss = 0.08;
    double inssValor = 0;

    if (folhaRequest.SalarioBruto > 4664.68)
    {
        aliquotaIrrf = 0.275;
        parcelaIrrf = 869.36;

    }
    else if (folhaRequest.SalarioBruto >= 3751.06)
    {
        aliquotaIrrf = 0.225;
        parcelaIrrf = 636.13;

    }
    else if (folhaRequest.SalarioBruto >= 2826.66)
    {
        aliquotaIrrf = 0.15;
        parcelaIrrf = 354.8;

    }
    else if (folhaRequest.SalarioBruto >= 1903.99)
    {
        aliquotaIrrf = 0.075;
        parcelaIrrf = 142.8;
    }

    if (folhaRequest.SalarioBruto < 5645.81)
    {
        if (folhaRequest.SalarioBruto >= 2822.91)
        {
            inss = 0.11;

        }
        else if (folhaRequest.SalarioBruto >= 1693.73)
        {
            inss = 0.09;
        }

        inssValor = folhaRequest.SalarioBruto * inss;

    }
    else
    {
        inssValor = 621.03;
    }

    folhaRequest.ImpostoInns = inssValor;
    folhaRequest.ImpostFgts = folhaRequest.SalarioBruto * 0.08;
    folhaRequest.ImpostoIrrf = folhaRequest.SalarioBruto * aliquotaIrrf - parcelaIrrf;
    folhaRequest.SalarioLiquido = folhaRequest.SalarioBruto - folhaRequest.ImpostoIrrf - folhaRequest.ImpostoInns;

    dbContext.Folhas.Add(folhaRequest);
    dbContext.SaveChanges();

    return Results.Created();
});

app.MapGet("api/folha/listar", ([FromServices] AppDbContext dbContext) =>
{
    if (!dbContext.Folhas.Any())
    {
        return Results.NotFound();
    }

    return Results.Ok(dbContext.Folhas.Include(p => p.Funcionario).ToList());
});

app.MapGet("api/folha/buscar/{cpf}/{mes}/{ano}", (string cpf, int mes, int ano, [FromServices] AppDbContext dbContext) =>
{
    Folha? folha = dbContext
                        .Folhas
                        .Include(p => p.Funcionario)
                        .FirstOrDefault(f => f.Funcionario != null && f.Funcionario.Cpf == cpf && f.Mes == mes && f.Ano == ano);

    if (folha is null)
    {
        return Results.NotFound("folha não encontrada");
    }

    return Results.Ok(folha);
});


app.Run();
