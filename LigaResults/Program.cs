namespace LigaResults
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;

  namespace LigaResults
  {
    public class Program
    {
      public class TeamResult
      {
        public string TeamName { get; set; }
        public int GoalsShot { get; set; }
        public int GoalsTaken { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
      }

      public class FormattedStats
      {
        public int Place { get; set; }
        public string Team { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public string Goals { get; set; }
        public int Difference { get; set; }
        public int Points { get; set; }
      }

      // Methode zur Auswahl der Liga durch den Benutzer
      public static string GetLigaSelection(List<string> ligas)
      {
        Console.WriteLine("Verfügbare Ligen:");
        Console.WriteLine("");

        for (int i = 0; i < ligas.Count; i++)
        {
          Console.WriteLine("[{0}] {1}", i + 1, ligas[i]);
        }

        string input;
        do
        {
          Console.WriteLine("");
          Console.Write("Nummer oder Name der Liga eingeben: ");
          input = Console.ReadLine();

          int selection;
          if (int.TryParse(input, out selection))
          {
            if (selection >= 1 && selection <= ligas.Count)
            {
              return ligas[selection - 1];
            }
            else
            {
              Console.WriteLine("Ungültige Ligaauswahl. Bitte geben Sie eine Zahl zwischen 1 und {0} ein.", ligas.Count);
            }
          }
          else if (ligas.Contains(input))
          {
            return input;
          }
          else
          {
            Console.WriteLine("Ungültiger Liganame. Bitte geben Sie einen gültigen Liganamen ein.");
          }
        } while (true);
      }

      // Methode zum Abrufen der Ligaergebnisse
      public static List<FormattedStats> GetLigaResults(string ligaSelection, string outputAsArray)
      {
        string mainFolderPath = Directory.GetCurrentDirectory();
        if (mainFolderPath.Contains("\\soccer-results"))
        {
          if (string.IsNullOrEmpty(ligaSelection))
          {
            List<string> ligas = Directory.GetDirectories(mainFolderPath).Select(Path.GetFileName).ToList();
            ligaSelection = GetLigaSelection(ligas);
          }

          string ligaPath = Path.Combine(mainFolderPath, ligaSelection);
          if (!Directory.Exists(ligaPath))
          {
            Console.WriteLine("Die Ziel-Liga konnte nicht gefunden werden. Bitte versuchen Sie es erneut mit einem gültigen Ligennamen.");
            Console.Read();
            return null;
          }

          Directory.SetCurrentDirectory(ligaPath);

          List<TeamResult> stats = GetData();

          List<FormattedStats> formattedStats = stats.GroupBy(t => t.TeamName)
              .Select(g => new FormattedStats
              {
                Team = g.Key,
                Wins = g.Sum(t => t.Wins),
                Losses = g.Sum(t => t.Losses),
                Draws = g.Sum(t => t.Draws),
                Goals = $"{g.Sum(t => t.GoalsShot)}:{g.Sum(t => t.GoalsTaken)}",
                Difference = g.Sum(t => t.GoalsShot) - g.Sum(t => t.GoalsTaken),
                Points = (g.Sum(t => t.Wins) * 3) + g.Sum(t => t.Draws)
              })
              .OrderByDescending(f => f.Points)
              .ThenByDescending(f => f.Difference)
              .ThenByDescending(f => int.Parse(f.Goals.Split(':')[0]))
              .ToList();

          for (int i = 0; i < formattedStats.Count; i++)
          {
            formattedStats[i].Place = i + 1;
          }

          Directory.SetCurrentDirectory(mainFolderPath);

          return formattedStats;
        }
        else
        {
          Console.WriteLine("Der Ordner \"soccer-results\" konnte nicht gefunden werden. Überprüfen Sie Ihren Ausführungspfad und versuchen sie es erneut.");
          Console.Read();
          return null;
        }
      }

      // Methode zum Abrufen der Daten aus den Dateien
      public static List<TeamResult> GetData()
      {
        List<TeamResult> stats = new List<TeamResult>();

        foreach (string file in Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt"))
        {

          string[] matches = File.ReadAllLines(file);

          foreach (string match in matches)
          {
            string[] teams = match.Split(':').Select(t => t.Trim()).ToArray();

            string[] team1Parts = teams[0].Split(' ');
            string team1Name = string.Join(" ", team1Parts.Take(team1Parts.Length - 1));
            int team1Goals = int.Parse(team1Parts.Last());

            string[] team2Parts = teams[1].Split(' ');
            string team2Name = string.Join(" ", team2Parts.Skip(1));
            int team2Goals = int.Parse(team2Parts.First());


            TeamResult team1Result = new TeamResult
            {
              TeamName = team1Name,
              GoalsShot = team1Goals,
              GoalsTaken = team2Goals,
              Wins = (team1Goals > team2Goals) ? 1 : 0,
              Losses = (team1Goals < team2Goals) ? 1 : 0,
              Draws = (team1Goals == team2Goals) ? 1 : 0
            };

            TeamResult team2Result = new TeamResult
            {
              TeamName = team2Name,
              GoalsShot = team2Goals,
              GoalsTaken = team1Goals,
              Wins = (team2Goals > team1Goals) ? 1 : 0,
              Losses = (team2Goals < team1Goals) ? 1 : 0,
              Draws = (team2Goals == team1Goals) ? 1 : 0
            };
            stats.Add(team1Result);
            stats.Add(team2Result);
          }
        }
        return stats;
      }

      // Hauptmethode
      public static void Main(string[] args)
      {
        while (true)
        {
          string ligaSelection = args.Length > 0 ? args[0] : null;
          string outputAsArray = args.Length > 1 ? args[1] : null;

          List<FormattedStats> results = GetLigaResults(ligaSelection, outputAsArray);

          if (results != null)
          {
            Console.Clear();
            if (outputAsArray == "true" || outputAsArray == "True")
            {
              foreach (var result in results)
              {
                Console.WriteLine($"{result.Place}. {result.Team}: {result.Points} Punkte");
              }
            }
            else
            {
              Console.WriteLine($"{"\nLigatabelle:",-20}{"",35}");
              Console.WriteLine("-----------------------------------------");
              Console.WriteLine($"{"Platz",-10}{"Team",-30}{"Siege",-10}{"Unentschieden",-15}{"Niederlagen",-15}{"Tore",-10}{"Differenz",-15}{"Punkte",-10}");
              foreach (var result in results)
              {
                Console.WriteLine($"{result.Place,-10}{result.Team,-30}{result.Wins,-10}{result.Draws,-15}{result.Losses,-15}{result.Goals,-10}{result.Difference,-15}{result.Points,-10}");
              }
            }
            // Benutzer zur Auswahl auffordern, ob er das Programm beenden möchte
            Console.Write("\n\nMöchten Sie das Programm beenden? (ja/nein): ");
            string choice = Console.ReadLine();

            // Überprüfen, ob der Benutzer das Programm beenden möchte
            if (string.Equals(choice, "ja", StringComparison.OrdinalIgnoreCase) || string.Equals(choice, "j", StringComparison.OrdinalIgnoreCase))
            {
              // Das Programm beenden
              Environment.Exit(0);
            }

            Console.Clear();
          }
          else
          {          // Das Programm beenden
            Environment.Exit(0);
          }
        }
      }
    }
  }

}