using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.Menus;

internal class MenuMostrarArtistas : Menu<Artista>
{
    public override void Executar(DAL<Artista> artistaDAL)
    {
        base.Executar(artistaDAL);
        ExibirTituloDaOpcao("Exibindo todos os artistas registradas na nossa aplicação");

        foreach (var artista in artistaDAL.Listar())
        {
            Console.WriteLine();
            Console.WriteLine($"{artista}");
        }

        Console.WriteLine("\nDigite uma tecla para voltar ao menu principal");
        Console.ReadKey();
        Console.Clear();
    }
}
