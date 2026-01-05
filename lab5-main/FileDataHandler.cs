using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace NewspaperKioskLab5
{
    static class FileDataHandler
    {
        private static string PubFile = "publications.json";
        private static string CustFile = "customers.json";
        private static string SellFile = "sellers.json";

        public static void SaveAll(
            List<Publication> pubs,
            List<Account> customers,
            List<Account> sellers)
        {
            File.WriteAllText(PubFile, JsonSerializer.Serialize(pubs));
            File.WriteAllText(CustFile, JsonSerializer.Serialize(customers));
            File.WriteAllText(SellFile, JsonSerializer.Serialize(sellers));
        }

        public static void LoadAll(
            ref List<Publication> pubs,
            ref List<Account> customers,
            ref List<Account> sellers)
        {
            if (File.Exists(PubFile))
                pubs = JsonSerializer.Deserialize<List<Publication>>(File.ReadAllText(PubFile)) ?? new();

            if (File.Exists(CustFile))
                customers = JsonSerializer.Deserialize<List<Account>>(File.ReadAllText(CustFile)) ?? new();

            if (File.Exists(SellFile))
                sellers = JsonSerializer.Deserialize<List<Account>>(File.ReadAllText(SellFile)) ?? new();
        }
    }
}

