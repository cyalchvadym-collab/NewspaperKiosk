namespace ConsoleApp1;

public class Magazine : Publication
{
    public int IssueNumber { get; set; }
    public string Category { get; set; } = "";
    public override string TypeName => "Журнал";
}
