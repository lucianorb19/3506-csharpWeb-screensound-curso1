using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.Menus
{
    internal class MenuMostrarMusicasPorAno : Menu<Musica>
    {
        public override void Executar(DAL<Musica> musicaDAL)
        {
            base.Executar(musicaDAL);
            base.ExibirTituloDaOpcao("MÚSICAS POR ANO DE LANÇAMENTO");

            Console.Write("Procurar músicas de qual ano?   -> ");
            int anoLancamento = int.Parse(Console.ReadLine()!);
            var musicas = musicaDAL.RecuperarMuitosPor(
                m => m.AnoLancamento == anoLancamento);
            if (musicas.Any())
            {
                foreach (var musica in musicas)
                {
                    musica.ExibirFichaTecnica();
                }

                Console.WriteLine("Digite uma tecla para voltar ao menu principal");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.WriteLine($"Músicas do ano {anoLancamento} não encontradas!");
                Console.WriteLine("Digite uma tecla para voltar ao menu principal");
                Console.ReadKey();
                Console.Clear();
            }
           

            

        }
    }
}
