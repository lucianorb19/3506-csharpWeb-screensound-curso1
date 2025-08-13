namespace ScreenSound.Modelos;

public class Musica
{
    //PROPRIEDADES
    public string Nome { get; set; }
    public int Id { get; set; }
    public int? AnoLancamento { get; set; }
    public virtual Artista? Artista { get; set; }


    //CONSTRUTOR
    public Musica(string nome)
    {
        Nome = nome;
    }


    //DEMAIS MÉTODOS
    public void ExibirFichaTecnica()
    {
        Console.WriteLine($"Nome: {Nome}\n" +
                          $"Ano: {AnoLancamento}\n" +
                          $"Artista/Banda: {Artista.Nome}\n");
      
    }

    public override string ToString()
    {
        return @$"Id: {Id}
        Nome: {Nome}";
    }
}