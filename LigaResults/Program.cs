namespace LigaResults
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text.Json;

  namespace LigaResults
  {
    public class Program
    {

      // Teste ob der Ausführungsort den Ordnernamen "soccer-results" hat
      public static bool TestExecPath(string pathToMainFolder)
      {
        if (pathToMainFolder.Contains("\\soccer-results"))
        {
          return true;
        }
        else
        {
          return false;
        }
      }


      // fordert nutzer zur auswahl einer Liga auf unt gib den namen des ausgewählten Ordners (Liga) zurück
      public static string GetLigaSelection(string pathToMainFolder, string? input = null)
      {
        List<string> ligas = Directory.GetDirectories(pathToMainFolder).Select(Path.GetFileName).ToList();

        if (input != null && !string.IsNullOrWhiteSpace(input))
        {
          if (ligas.Contains(input))
          {
            return input;
          }
          else
          {
            return "ERROR: Ungültiger Liganame. Bitte geben Sie einen gültigen Liganamen ein.";
          }
        }
        else
        {
          Console.WriteLine("Verfügbare Ligen:");
          Console.WriteLine("");

          for (int i = 0; i < ligas.Count; i++)
          {
            Console.WriteLine("[{0}] {1}", i + 1, ligas[i]);
          }

          do
          {
            Console.WriteLine("");
            Console.Write("Nummer oder Name der Liga eingeben oder mittels 'exit' die Anwendung verlassen: ");
            input = Console.ReadLine();

            if (int.TryParse(input, out int selection))
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
            else if (input.ToLower() == "exit")
            {
              // Das Programm beenden
              Environment.Exit(0);
              return null;
            }
            else
            {
              Console.WriteLine("Ungültiger Liganame. Bitte geben Sie einen gültigen Liganamen ein.");
            }
          } while (true);
        }
      }


      // Iteriert über "DayX" Ordner, Lädt die Daten der Matches aus den .txt Dateien und gibt die ergebnisse jedes teams für jeden Tag in einer Liste zurück
      public static List<MatchResult> GetData(string pathToLiga)
      {

        Directory.SetCurrentDirectory(pathToLiga);

        List<MatchResult> resultsList = new List<MatchResult>();

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


            MatchResult team1Result = new MatchResult
            {
              TeamName = team1Name,
              GoalsShot = team1Goals,
              GoalsTaken = team2Goals,
              Wins = (team1Goals > team2Goals) ? 1 : 0,
              Losses = (team1Goals < team2Goals) ? 1 : 0,
              Draws = (team1Goals == team2Goals) ? 1 : 0
            };

            MatchResult team2Result = new MatchResult
            {
              TeamName = team2Name,
              GoalsShot = team2Goals,
              GoalsTaken = team1Goals,
              Wins = (team2Goals > team1Goals) ? 1 : 0,
              Losses = (team2Goals < team1Goals) ? 1 : 0,
              Draws = (team2Goals == team1Goals) ? 1 : 0
            };
            resultsList.Add(team1Result);
            resultsList.Add(team2Result);
          }
        }
        return resultsList;
      }

      // Gruppieren der zusammengestellten Ergebnisliste aus den TXT-Dateien nach dem TeamName-Feld und sortieret basierend auf dem Punktesystem
      public static List<TeamResult> GroupByTeamAndPoints(List<MatchResult> results)
      {

        List<TeamResult> TeamResult = results.GroupBy(t => t.TeamName)
            .Select(g => new TeamResult
            {
              Team = g.Key,
              Wins = g.Sum(t => t.Wins),
              Losses = g.Sum(t => t.Losses),
              Draws = g.Sum(t => t.Draws),
              GoalsShot = g.Sum(t => t.GoalsShot),
              GoalsTaken = g.Sum(t => t.GoalsTaken),
              Difference = g.Sum(t => t.GoalsShot) - g.Sum(t => t.GoalsTaken),
              Points = (g.Sum(t => t.Wins) * 3) + g.Sum(t => t.Draws)
            })
            .OrderByDescending(f => f.Points)
            .ThenByDescending(f => f.Difference)
            .ThenByDescending(f => f.GoalsShot)
            .ToList();

        for (int i = 0; i < TeamResult.Count; i++)
        {
          TeamResult[i].Place = i + 1;
        }

        return TeamResult;
      }

      // Umwandlung der Ergebnisse im JSON Format
      public static string ConvertToJson(List<TeamResult> ligaResults)
      {
        return JsonSerializer.Serialize(ligaResults);
      }


      // Ausgabe der Ergebnisse als eine Ligatabelle
      public static void PrintLigaTable(List<TeamResult> ligaResults)
      {
        Console.WriteLine($"{"\nLigatabelle:",-20}{"",35}");
        Console.WriteLine("-----------------------------------------");
        Console.WriteLine($"{"Platz",-10}{"Team",-30}{"Siege",-10}{"Unentschieden",-15}{"Niederlagen",-15}{"Tore",-10}{"Differenz",-15}{"Punkte",-10}");
        foreach (var result in ligaResults)
        {
          Console.WriteLine($"{result.Place,-10}{result.Team,-30}{result.Wins,-10}{result.Draws,-15}{result.Losses,-15}{result.GoalsShot,-10}{result.Difference,-15}{result.Points,-10}");
        }
      }

      // Hauptmethode
      public static void Main(string[] args)
      {
        while (true)
        {
          string? ligaSelectionParam = args.Length > 0 ? args[0] : null;
          string? outputAsJSON = args.Length > 1 ? args[1] : null;
          string execFolderPath = Directory.GetCurrentDirectory();

          if (TestExecPath(execFolderPath))
          {
            string ligaSelection = GetLigaSelection(execFolderPath, ligaSelectionParam);

            if (ligaSelection.StartsWith("ERROR: "))
            {
              Console.WriteLine("ERROR: Ungültiger Liganame. Bitte gib einen gültigen Liganamen als Parameter an.");
              Environment.Exit(0);
            }

            string pathToLiga = Path.Combine(execFolderPath, ligaSelection); ;

            List<MatchResult> matchResultsList = GetData(pathToLiga);
            List<TeamResult> ligaResultsList = GroupByTeamAndPoints(matchResultsList);

            Console.Clear();
            if (outputAsJSON == "true" || outputAsJSON == "True")
            {
              // output ligaResults as Json
              Console.WriteLine(ConvertToJson(ligaResultsList));
            }
            else
            {
              PrintLigaTable(ligaResultsList);
            }
            Directory.SetCurrentDirectory(execFolderPath);


            Console.Write("\n\nDrücke eine Beliebige Taste um fortzufahren");
            Console.ReadKey();
            if (ligaSelectionParam != null && !string.IsNullOrWhiteSpace(ligaSelectionParam)) { Environment.Exit(0); }
            Console.Clear();
          }
        }
      }
    }
  }
}