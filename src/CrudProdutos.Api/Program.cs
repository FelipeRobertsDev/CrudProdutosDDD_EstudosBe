using CrudProdutos.Application.UseCases;
using CrudProdutos.Domain.Repositories;
using CrudProdutos.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ETAPA 0: registramos as dependências para o .NET construir os objetos por nós.
// Singleton = uma instância compartilhada durante toda a execução da API.
// Como os dados ficam em memória, o repositório PRECISA ser singleton neste exemplo.
builder.Services.AddSingleton<IProdutoRepository, ProdutoRepositoryEmMemoria>();

// Cada Use Case faz UMA ação do sistema. Scoped = instância por requisição HTTP.
builder.Services.AddScoped<CriarProdutoUseCase>();
builder.Services.AddScoped<ListarProdutosUseCase>();
builder.Services.AddScoped<ObterProdutoUseCase>();
builder.Services.AddScoped<AtualizarProdutoUseCase>();
builder.Services.AddScoped<ExcluirProdutoUseCase>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers(); // Aqui o ASP.NET conecta as rotas aos controllers.

app.Run();
