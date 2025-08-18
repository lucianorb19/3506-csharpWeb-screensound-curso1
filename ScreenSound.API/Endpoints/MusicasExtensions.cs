using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ScreenSound.API.Requests;
using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.API.Endpoints
{
    public static class MusicasExtensions
    {
        public static void AddEndPointsMusicas(this WebApplication app)
        {

            app.MapGet("/Musicas", ([FromServices] DAL<Musica> dal) =>
            {
                return Results.Ok(dal.Listar());
            });

            app.MapGet("/Musicas/{nome}", ([FromServices] DAL<Musica> dal,
                                           string nome) =>
            {
                var musicas = dal.RecuperarMuitosPor(
                    m => m.Nome.ToUpper().Equals(nome.ToUpper()));
                if (musicas.IsNullOrEmpty())
                {
                    return Results.NotFound();
                }
                return Results.Ok(musicas);
            });

            app.MapPost("/Musicas", ([FromServices] DAL<Musica> dal,
                                     [FromBody] MusicaRequest musicaRequest) =>
            {
                var musica = new Musica(musicaRequest.nome,
                                        musicaRequest.anoLancamento);
                dal.Adicionar(musica);
                return Results.Ok();
            });


            app.MapDelete("/Musicas/{id}", ([FromServices] DAL<Musica> dal,
                                            int id) =>
            {
                var musicaDeletada = dal.RecuperarPrimeiroPor(
                    m => m.Id.Equals(id));

                if (musicaDeletada is null)
                {
                    return Results.NotFound();
                }

                dal.Deletar(musicaDeletada);
                return Results.NoContent();
            });


            app.MapPut("/Musicas", ([FromServices] DAL<Musica> dal,
                                    [FromBody] MusicaRequestEdit musicaRequestEdit) =>
            {
                var musicaAtualizada = dal.RecuperarPrimeiroPor(
                    m => m.Id.Equals(musicaRequestEdit.id));

                if (musicaAtualizada is null)
                {
                    return Results.NotFound();
                }
                musicaAtualizada.Nome = musicaRequestEdit.nome;
                musicaAtualizada.AnoLancamento = musicaRequestEdit.anoLancamento;
                dal.Atualizar(musicaAtualizada);

                return Results.Ok();
            });

        }
    }
}
