using System.Collections.Generic;
using System.IO;
using System.Linq;

static class FileService
{
    private static string filePath = "publications.txt";

    public static List<Publication> Load()
    {
        var list = new List<Publication>();

        if (!File.Exists(filePath))
            return list;

        foreach (var line in File.ReadAllLines(filePath))
        {
            var parts = line.Split(';');
            list.Add(new Publication(
                parts[0],
                parts[1],
                double.Parse(parts[2])
            ));
        }

        return list;
    }

    public static void Save(List<Publication> publications)
    {
        File.WriteAllLines(
            filePath,
            publications.Select(p =>
                $"{p.Title};{p.Type};{p.Price}")
        );
    }
}
