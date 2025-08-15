using Microsoft.AspNetCore.Mvc;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//INJE��O DE DEPEND�NCIA - DAL<Artista> E ScreenSoundContext
builder.Services.AddDbContext<ScreenSoundContext>();
builder.Services.AddTransient<DAL<Artista>>();

//IGNORAR CICLOS NA SERIALIZA��O
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>
    (options => options.SerializerOptions.ReferenceHandler = 
                             ReferenceHandler.IgnoreCycles);

var app = builder.Build();



app.MapGet("/Artistas", ([FromServices] DAL<Artista> dal) =>
{
    return Results.Ok(dal.Listar());
});

//LISTA TODOS ARTISTAS COM NOME ESPEC�FICO
app.MapGet("/Artistas/{nome}", ([FromServices] DAL < Artista > dal, 
                                string nome) =>
{
    var artista =  dal.RecuperarMuitosPor(
        a => a.Nome.ToUpper().Equals(nome.ToUpper()));
    if(artista is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(artista);
});

//ADICIONA UM ARTISTA RECEBIDO NO CORPO DA REQUISI��O
app.MapPost("/Artistas",([FromServices] DAL < Artista > dal,
                         [FromBody]Artista artista) =>
{
    dal.Adicionar(artista);
    return Results.Ok();
});


app.MapDelete("/Artistas/{id}", ([FromServices] DAL<Artista> dal,
                                 int id) =>
{
    var artista = dal.RecuperarPrimeiroPor(a => a.Id.Equals(id));
    if (artista is null)
    {
        return Results.NotFound();
    }
    dal.Deletar(artista);
    return Results.NoContent();
});


app.MapPut("/Artistas", ([FromServices] DAL<Artista> dal,
                         [FromBody] Artista artista) =>
{
    var artistaAtualizado = dal.RecuperarPrimeiroPor(
        a => a.Id.Equals(artista.Id));

    if(artistaAtualizado is null)
    {
        return Results.NotFound();
    }
    artistaAtualizado.Nome = artista.Nome;
    artistaAtualizado.Bio = artista.Bio;
    artistaAtualizado.FotoPerfil = artista.FotoPerfil;
    dal.Atualizar(artistaAtualizado);

    return Results.Ok();

    /*
     artista [FromBody] -> artistaAtualizado <-> artista no BD
     */


});

app.Run();
