using ScreenSound.Modelos;

namespace ScreenSound.Banco
{
    internal class MusicaDAL : DAL<Musica>
    {
        //CAMPOS
        private readonly ScreenSoundContext context;

        //CONSTRUTOR
        public MusicaDAL(ScreenSoundContext context)
        {
            this.context = context;
        }

        //DEMAIS MÉTODOS
        public override IEnumerable<Musica> Listar()
        {
            return context.Musicas.ToList();
        }

        public override void Adicionar(Musica musica)
        {
            context.Musicas.Add(musica);
            context.SaveChanges();
        }

        public override void Atualizar(Musica musica)
        {
            context.SaveChanges();
            context.Musicas.Update(musica);
        }

        public override void Deletar(Musica musica)
        {
            context.Musicas.Remove(musica);
            context.SaveChanges();
        }

        public Musica? RecuperarPeloNome(string nome)
        {
            return context.Musicas
                .FirstOrDefault(musica => musica.Nome.Equals(nome));
        }




    }
}
