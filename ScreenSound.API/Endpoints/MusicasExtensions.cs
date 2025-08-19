using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ScreenSound.API.Requests;
using ScreenSound.API.Responses;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using ScreenSound.Shared.Modelos.Modelos;

namespace ScreenSound.API.Endpoints
{
    public static class MusicasExtensions
    {
        public static void AddEndPointsMusicas(this WebApplication app)
        {

            app.MapGet("/Musicas", ([FromServices] DAL<Musica> dal) =>
            {
                return Results.Ok(EntityListToResponseList(dal.Listar()));
            });

            app.MapGet("/Musicas/{nome}", ([FromServices] DAL<Musica> dal,
                                           string nome) =>
            {
                var musicas = dal.RecuperarMuitosPor(
                    m => m.Nome.ToUpper().Equals(nome.ToUpper()));
                if (musicas.IsNullOrEmpty())
                {
                    return Results.NotFound("Música não encontrada!");
                }
                return Results.Ok(EntityListToResponseList(musicas));
            });


            app.MapPost("/Musicas", ([FromServices] DAL<Musica> dal,
                                     [FromServices] DAL<Genero> dalGenero,
                                     [FromBody] MusicaRequest musicaRequest) =>
            {
                var musica = new Musica(musicaRequest.nome,
                                        musicaRequest.anoLancamento)
                {
                    ArtistaId = musicaRequest.artistaId,
                    //CONVERSÃO ICollection<GeneroRequest> PARA
                    //ICollection<Genero>
                    //COM USO DE OPERADOR TERNÁRIO PARA CASO generos LISTADOS
                    //EM musicaRequest SEJA VAZIO
                    Generos =
                    musicaRequest.generos is not null ?
                        GeneroRequestConverter(musicaRequest.generos, dalGenero) :
                        new List<Genero>()
                };
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
                    return Results.NotFound("Música não encontrada!");
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
                    return Results.NotFound("Música não encontrada!");
                }
                musicaAtualizada.Nome = musicaRequestEdit.nome;
                musicaAtualizada.AnoLancamento = musicaRequestEdit.anoLancamento;
                dal.Atualizar(musicaAtualizada);

                return Results.Ok();
            });
        }


        //MÉTODOS AUXILIARES
        private static ICollection<Genero> GeneroRequestConverter(
                                ICollection<GeneroRequest> listaGenerosRequest, DAL<Genero> dalGenero)
        {
            //return generos.Select(a => RequestToEntity(a)).ToList();
            var listaDeGeneros = new List<Genero>();
            foreach (var item in listaGenerosRequest)
            {
                //CADA GeneroRequest É CONVERTIDO NUM OBJETO Genero
                var entity = RequestToEntity(item);
                
                //O ITEM ATUAL DO LOOP É SALVO EM genero 
                //CASO SEU NOME JÁ ESTEJA NA BASE DE DADOS
                var genero = dalGenero.RecuperarPrimeiroPor(
                    g=> g.Nome.ToUpper().Equals(item.nome.ToUpper()));

                if(genero is not null)
                {
                    listaDeGeneros.Add(genero);//?????
                }
                else
                {
                    listaDeGeneros.Add(entity);
                }

                //OU SEJA, PARA A MÚSICA QUE ESTÁ SENDO CADASTRADA
                //SE NO CAMPO GENERO EU TENTAR INSERIR UM NOME QUE JÁ TENHA NO BANCO
                //ADICIONO EM listaDeGeneros
                //SE NÃO HOUVER NO BANCO
                //ADICIONO DA MESMA FORMA ????
            }
            return listaDeGeneros;
        }

        private static Genero RequestToEntity(GeneroRequest generoRequest)
        {
            return new Genero(generoRequest.nome)
            {
                Descricao = generoRequest.descricao
            };
        }

        private static MusicaResponse EntityToResponse(Musica musica)
        {
            return new MusicaResponse(musica.Id, musica.Nome, musica.ArtistaId, musica.AnoLancamento);
        }

        private static ICollection<MusicaResponse> 
                              EntityListToResponseList(IEnumerable<Musica> musicaList)
        {
            return musicaList.Select(a => EntityToResponse(a)).ToList();
        }

        

    }
}
