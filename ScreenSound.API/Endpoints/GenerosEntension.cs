using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ScreenSound.API.Requests;
using ScreenSound.API.Responses;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using ScreenSound.Shared.Modelos.Modelos;

namespace ScreenSound.API.Endpoints
{
    public static class GenerosEntension
    {
        public static void AddEndPointsGeneros(this WebApplication app)
        {
            
            app.MapGet("/Generos", ([FromServices] DAL<Genero> dal) =>
            {
                return Results.Ok(EntityListToResponseList(dal.Listar()));
            });


            app.MapGet("/Generos/{nome}", ([FromServices] DAL<Genero> dal,
                                           string nome) =>
            {
                var generos = dal.RecuperarMuitosPor(
                    g => g.Nome.ToUpper().Equals(nome.ToUpper()));
                
                if(generos.IsNullOrEmpty())
                {
                    return Results.NotFound("Gênero não encontrado!");
                }
                
                return Results.Ok(EntityListToResponseList(generos));
            });


            app.MapPost("/Generos", ([FromServices] DAL<Genero> dal,
                                     [FromBody] GeneroRequest generoRequest) =>
            {
                dal.Adicionar(RequestToEntity(generoRequest));
                return Results.Ok();
;            });

 
            app.MapPut("/Generos", ([FromServices] DAL<Genero> dal,
                                    [FromBody] GeneroRequestEdit generoRequestEdit) =>
            {
                var generoAtualizado = dal.RecuperarPrimeiroPor(
                    g => g.Id.Equals(generoRequestEdit.id));
                
                if(generoAtualizado is null)
                {
                    return Results.NotFound("Gênero não encontrado!");
                }
                
                generoAtualizado.Nome = generoRequestEdit.nome;
                generoAtualizado.Descricao = generoRequestEdit.descricao;
                
                dal.Atualizar(generoAtualizado);
                return Results.Ok();

            });

            
            app.MapDelete("/Generos/{id}", ([FromServices]DAL<Genero> dal,
                                            int id) =>
            {
                var generoDeletado = dal.RecuperarPrimeiroPor(
                    g => g.Id.Equals(id));
                
                if(generoDeletado is null)
                {
                    return Results.NotFound("Gênero não encontrado!");
                }
                
                dal.Deletar(generoDeletado);
                return Results.NoContent();

            });

        }

        

        //MÉTODOS AUXILIARES
        private static GeneroResponse EntityToResponse(Genero genero)
        {
            return new GeneroResponse(genero.Id, genero.Nome,
                                       genero.Descricao);
        }

        private static ICollection<GeneroResponse>
                    EntityListToResponseList(IEnumerable<Genero> listaDeGeneros)
        {
            return listaDeGeneros.Select(a => EntityToResponse(a)).ToList();
        }

        private static Genero RequestToEntity(GeneroRequest generoRequest)
        {
            return new Genero(generoRequest.nome)
            {
                Descricao = generoRequest.descricao
            };
        }

    }
}
