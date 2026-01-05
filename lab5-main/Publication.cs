namespace NewspaperKioskLab5
{
    class Publication
    {
        public string Name { get; set; }
        public double Price { get; set; }

        public Publication() { }

        public Publication(string name, double price)
        {
            Name = name;
            Price = price;
        }
    }
}

