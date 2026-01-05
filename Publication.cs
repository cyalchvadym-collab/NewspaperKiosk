namespace ConsoleApp1;

public abstract class Publication
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public abstract string TypeName { get; }

    public override string ToString()
        => $"{Id,-3} | {TypeName,-8} | {Title,-28} | {Price,6} | {Stock,5}";
}
