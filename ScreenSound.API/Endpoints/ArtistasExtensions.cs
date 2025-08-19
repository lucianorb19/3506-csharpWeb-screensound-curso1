using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ScreenSound.API.Requests;
using ScreenSound.API.Responses;
using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.API.Endpoints
{
    public static class ArtistasExtensions
    {
        public static void AddEndPointsArtistas(this WebApplication app)
        {

            app.MapGet("/Artistas", ([FromServices] DAL<Artista> dal) =>
            {
                return Results.Ok(EntityListToResponseList(dal.Listar()));
            });


            app.MapGet("/Artistas/{nome}", ([FromServices] DAL<Artista> dal,
                                            string nome) =>
            {
                var artistas = dal.RecuperarMuitosPor(
                    a => a.Nome.ToUpper().Equals(nome.ToUpper()));
                if (artistas.IsNullOrEmpty())
                {
                    return Results.NotFound("Artista/Banda não encontrado!");
                }
                return Results.Ok(EntityListToResponseList(artistas));
            });


            app.MapPost("/Artistas", ([FromServices] DAL<Artista> dal,
                                     [FromBody] ArtistaRequest artistaRequest) =>
            {
                var artista = new Artista(artistaRequest.nome, artistaRequest.bio);
                dal.Adicionar(artista);
                return Results.Ok();
            });


            app.MapDelete("/Artistas/{id}", ([FromServices] DAL<Artista> dal,
                                             int id) =>
            {
                var artistaDeletado = dal.RecuperarPrimeiroPor(a => a.Id.Equals(id));
                if (artistaDeletado is null)
                {
                    return Results.NotFound("Artista/Banda não encontrado!");
                }
                dal.Deletar(artistaDeletado);
                return Results.NoContent();
            });


            app.MapPut("/Artistas", ([FromServices] DAL<Artista> dal,
                                     [FromBody] ArtistaRequestEdit artistaRequestEdit) =>
            {
                var artistaAtualizado = dal.RecuperarPrimeiroPor(
                    a => a.Id.Equals(artistaRequestEdit.id));

                if (artistaAtualizado is null)
                {
                    return Results.NotFound("Artista/Banda não encontrado!");
                }
                artistaAtualizado.Nome = artistaRequestEdit.nome;
                artistaAtualizado.Bio = artistaRequestEdit.bio;
                dal.Atualizar(artistaAtualizado);

                return Results.Ok();

                /*
                 artistaRequestEdit [FromBody] -> artistaAtualizado <-> artista no BD
                 */
            });
        }

        //MÉTODOS PARA O RESPONSE - ENDPOINTS DE CONSULTA
        private static ArtistaResponse EntityToResponse(Artista artista)
        {
            return new ArtistaResponse(artista.Id, artista.Nome, 
                                       artista.Bio, artista.FotoPerfil);
        }

        private static ICollection<ArtistaResponse> 
                    EntityListToResponseList(IEnumerable<Artista> listaDeArtistas)
        {
            return listaDeArtistas.Select(a => EntityToResponse(a)).ToList();
        }

    }
}
