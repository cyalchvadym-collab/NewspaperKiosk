namespace ConsoleApp1;

public class Newspaper : Publication
{
    public string Frequency { get; set; } = "щоденно";
    public override string TypeName => "Газета";
}
