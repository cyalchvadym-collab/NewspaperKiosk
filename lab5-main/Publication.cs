class Publication
{
    public string Title { get; set; }
    public string Type { get; set; } // Газета / Журнал
    public double Price { get; set; }

    public Publication(string title, string type, double price)
    {
        Title = title;
        Type = type;
        Price = price;
    }

    public override string ToString()
    {
        return $"{Title} | {Type} | {Price} грн";
    }
}
