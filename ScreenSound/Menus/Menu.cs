using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.Menus;

internal class Menu<T> where T : class
{
    public void ExibirTituloDaOpcao(string titulo)
    {
        int quantidadeDeLetras = titulo.Length;
        string asteriscos = string.Empty.PadLeft(quantidadeDeLetras, '*');
        Console.WriteLine(asteriscos);
        Console.WriteLine(titulo);
        Console.WriteLine(asteriscos + "\n");
    }
    public virtual void Executar(DAL<T> objeto)
    {
        Console.Clear();
    }
}
