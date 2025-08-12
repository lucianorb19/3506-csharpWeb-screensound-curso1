using ScreenSound.Modelos;

namespace ScreenSound.Banco
{
    internal class MusicaDAL
    {
        //CAMPOS
        private readonly ScreenSoundContext context;

        //CONSTRUTOR
        public MusicaDAL(ScreenSoundContext context)
        {
            this.context = context;
        }

        //DEMAIS MÉTODOS
        public IEnumerable<Musica> ListarMusicas()
        {
            return context.Musicas.ToList();
        }

        public void AdicionarMusica(Musica musica)
        {
            context.Musicas.Add(musica);
            context.SaveChanges();
        }

        public void AtualizarMusica(Musica musica)
        {
            context.SaveChanges();
            context.Musicas.Update(musica);
        }

        public void DeletarMusica(Musica musica)
        {
            context.Musicas.Remove(musica);
            context.SaveChanges();
        }





    }
}
