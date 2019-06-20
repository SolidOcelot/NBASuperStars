using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBASuperStars
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter the path to the json file: ");
                string path = Console.ReadLine();

                Console.WriteLine("Enter the number of maximum years played: ");
                int maxYears = Int32.Parse(Console.ReadLine());

                Console.WriteLine("Enter the number of minimum rating: ");
                int minRating = Int32.Parse(Console.ReadLine());

                Console.WriteLine("Enter the path to the csv file, where you want to save the data: ");
                string csvPath = Console.ReadLine();

                var jsonPath = File.ReadAllText(path);
                List<Player> results = JsonConvert.DeserializeObject<List<Player>>(jsonPath);

                var superStars = SortPlayers(results, maxYears, minRating);
                WriteToCSV(SortDescending(superStars), csvPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong. Please try again.");
                Console.WriteLine("-----------------------------------------");
                Program.Main(args);
            } 
        }

        static List<Player> SortPlayers(List<Player> players, int maxYearsPlayed, int minRating)
        {
            List<Player> superStars = new List<Player>();

            int currentYear = Int32.Parse(DateTime.Now.Year.ToString());

            foreach (var player in players)
            {
                if (player.Rating >= minRating && player.PlayingSince + maxYearsPlayed >= currentYear)
                {
                    superStars.Add(player);
                }
            }

            return superStars;
        }

        static List<Player> SortDescending(List<Player> players)
        {
            return players.OrderByDescending(p => p.Rating).ToList();
        }

        static void WriteToCSV(List<Player> players, string path)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@path, false))
                {
                    file.WriteLine("Name,Rating");
                    foreach (var player in players)
                    {
                        file.WriteLine(player.Name + ", " + player.Rating);
                    }

                    Console.WriteLine("Writing completed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong with the file. Please try again.");
                Program.Main(null);
            }
        }
    }
}
