using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//INJEÇÃO DE DEPENDÊNCIA - DAL<Artista>, DAL<Musica> E ScreenSoundContext
builder.Services.AddDbContext<ScreenSoundContext>();
builder.Services.AddTransient<DAL<Artista>>();
builder.Services.AddTransient<DAL<Musica>>();

//IGNORAR CICLOS NA SERIALIZAÇÃO
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>
    (options => options.SerializerOptions.ReferenceHandler = 
                             ReferenceHandler.IgnoreCycles);

var app = builder.Build();


//CRUD ARTISTAS
app.MapGet("/Artistas", ([FromServices] DAL<Artista> dal) =>
{
    return Results.Ok(dal.Listar());
});


app.MapGet("/Artistas/{nome}", ([FromServices] DAL < Artista > dal, 
                                string nome) =>
{
    var artistas =  dal.RecuperarMuitosPor(
        a => a.Nome.ToUpper().Equals(nome.ToUpper()));
    if(artistas.IsNullOrEmpty())
    {
        return Results.NotFound();
    }
    return Results.Ok(artistas);
});


app.MapPost("/Artistas",([FromServices] DAL<Artista> dal,
                         [FromBody]Artista artista) =>
{
    dal.Adicionar(artista);
    return Results.Ok();
});


app.MapDelete("/Artistas/{id}", ([FromServices] DAL<Artista> dal,
                                 int id) =>
{
    var artistaDeletado = dal.RecuperarPrimeiroPor(a => a.Id.Equals(id));
    if (artistaDeletado is null)
    {
        return Results.NotFound();
    }
    dal.Deletar(artistaDeletado);
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


//CRUD MUSICAS
app.MapGet("/Musicas", ([FromServices] DAL<Musica> dal) =>
{
    return Results.Ok(dal.Listar());
});

app.MapGet("/Musicas/{nome}", ([FromServices] DAL<Musica> dal,
                               string nome) =>
{
    var musicas = dal.RecuperarMuitosPor(
        m => m.Nome.ToUpper().Equals(nome.ToUpper()));
    if(musicas.IsNullOrEmpty())
    {
        return Results.NotFound();
    }
    return Results.Ok(musicas);
});

app.MapPost("/Musicas", ([FromServices] DAL<Musica> dal,
                         [FromBody] Musica musica) =>
{
    dal.Adicionar(musica);
    return Results.Ok();
});


app.MapDelete("/Musicas/{id}", ([FromServices] DAL<Musica> dal,
                                int id) =>
{
    var musicaDeletada = dal.RecuperarPrimeiroPor(
        m => m.Id.Equals(id));

    if(musicaDeletada is null)
    {
        return Results.NotFound();
    }

    dal.Deletar(musicaDeletada);
    return Results.NoContent();
});


app.MapPut("/Musicas", ([FromServices] DAL<Musica> dal,
                        [FromBody] Musica musica) =>
{
    var musicaAtualizada = dal.RecuperarPrimeiroPor(
        m => m.Id.Equals(musica.Id));

    if (musicaAtualizada is null)
    {
        return Results.NotFound();
    }
    musicaAtualizada.Nome = musica.Nome;
    musicaAtualizada.AnoLancamento = musica.AnoLancamento;
    dal.Atualizar(musicaAtualizada);

    return Results.Ok();
});


app.Run();
