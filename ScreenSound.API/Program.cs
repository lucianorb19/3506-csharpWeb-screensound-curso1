using Microsoft.AspNetCore.Mvc;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
//IGNORAR CICLOS NA SERIALIZA��O
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>
    (options => options.SerializerOptions.ReferenceHandler = 
                             ReferenceHandler.IgnoreCycles);

var app = builder.Build();



app.MapGet("/Artistas", () =>
{
    var dal = new DAL<Artista>(new ScreenSoundContext());
    return Results.Ok(dal.Listar());
});

//LISTA TODOS ARTISTAS COM NOME ESPEC�FICO
app.MapGet("/Artistas/{nome}", (string nome) =>
{
    var dal = new DAL<Artista>(new ScreenSoundContext());
    var artista =  dal.RecuperarMuitosPor(
        a => a.Nome.ToUpper().Equals(nome.ToUpper()));
    if(artista is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(artista);
});

//ADICIONA UM ARTISTA RECEBIDO NO CORPO DA REQUISI��O
app.MapPost("/Artistas",([FromBody]Artista artista) =>
{
    var dal = new DAL<Artista>(new ScreenSoundContext());
    dal.Adicionar(artista);
    return Results.Ok();
});

app.Run();
