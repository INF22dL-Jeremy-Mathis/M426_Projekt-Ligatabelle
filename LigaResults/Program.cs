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
      public static List<MatchResult> GetData(string pathToLiga, int? bisSpieltag = null)
      {

        Directory.SetCurrentDirectory(pathToLiga);

        List<MatchResult> resultsList = new List<MatchResult>();


        var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt")
                             .OrderBy(f => f)
                             .Take(bisSpieltag ?? int.MaxValue);

        foreach (string file in files)
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
      public static List<TeamResult> GroupByTeam(List<MatchResult> results)
      {
        return results.GroupBy(t => t.TeamName)
            .Select(g => new TeamResult
            {
              Name = g.Key,
              Siege = g.Sum(t => t.Wins),
              Niederlagen = g.Sum(t => t.Losses),
              Unentschieden = g.Sum(t => t.Draws),
              Tore = g.Sum(t => t.GoalsShot),
              Gegentore = g.Sum(t => t.GoalsTaken),
              Tordifferenz = g.Sum(t => t.GoalsShot) - g.Sum(t => t.GoalsTaken),
              Punkte = (g.Sum(t => t.Wins) * 3) + g.Sum(t => t.Draws)
            })
            .ToList();
      }

      public static List<TeamResult> SortTeams(List<TeamResult> teamResults)
      {
        List<TeamResult> sortedResults = teamResults
            .OrderByDescending(f => f.Punkte)
            .ThenByDescending(f => f.Tordifferenz)
            .ThenByDescending(f => f.Siege)
            .ThenBy(f => f.Name)
            .ToList();

        for (int i = 0; i < sortedResults.Count; i++)
        {
          sortedResults[i].Rang = i + 1;
        }

        return sortedResults;
      }

      public static List<TeamResult> GroupByTeamAndPoints(List<MatchResult> results)
      {
        List<TeamResult> groupedTeams = GroupByTeam(results);
        return SortTeams(groupedTeams);
      }

      // Umwandlung der Ergebnisse im JSON Format
      public static string ConvertToJson(List<TeamResult> ligaResults)
      {
        return JsonSerializer.Serialize(ligaResults);
      }


      // Ausgabe der Ergebnisse als eine Ligatabelle
      public static void PrintLigaTable(List<TeamResult> ligaResults, string liga, int? bisSpieltag)
      {
        if (bisSpieltag is int)
          Console.WriteLine($"{"\nLigatabelle bis spieltag " + bisSpieltag + ": " + liga,-20}{"",35}");
        else
          Console.WriteLine($"{"\nLigatabelle: " + liga + " - Endergebnisse",-20}{"",35}");
        Console.WriteLine($"{"------------------------------------------------------------------------------------------------------------------------",120}");
        Console.WriteLine($"{"Rang",5}{"",2}{"Name",-41}{"Punkte",6}{"",2}{"Siege",5}{"",2}{"Niederlagen",11}{"",2}{"Unentschieden",13}{"",2}{"Tore",4}{"",2}{"Gegentore",9}{"",2}{"Tordifferenz",12}");
        Console.WriteLine($"{"------------------------------------------------------------------------------------------------------------------------",120}");
        foreach (var result in ligaResults)
        {
          Console.WriteLine($"{result.Rang,5}{"",2}{result.Name,-41}{result.Punkte,6}{"",2}{result.Siege,5}{"",2}{result.Niederlagen,11}{"",2}{result.Unentschieden,13}{"",2}{result.Tore,4}{"",2}{result.Gegentore,9}{"",2}{result.Tordifferenz,12}");
        }
      }


      // Hauptmethode
      public static void Main(string[] args)
      {
        while (true)
        {
          string? ligaSelectionParam = args.Length > 0 ? args[0] : null;
          string? outputAsJSON = args.Length > 1 ? args[1] : null;
          int? bisSpieltag = null; // Korrektur: Initialisierung als nullable int
          string execFolderPath = Directory.GetCurrentDirectory();

          if (!TestExecPath(execFolderPath))
          {
            Console.WriteLine("Ungültiger Ordner, stelle sicher dass die Anwendung im Ordner \"soccer-results\" gestartet wurde");
            Environment.Exit(0);
          }

          // Überprüfung, ob outputAsJSON einen gültigen booleschen Wert hat
          if (outputAsJSON != null && !(outputAsJSON.ToLower() == "true" || outputAsJSON.ToLower() == "false"))
          {
            Console.WriteLine("Ungültiger Wert für Wahl des JSON Outputs. Bitte geben Sie entweder 'true' oder 'false' als 2. Parameter ein.");
            Environment.Exit(0);
          }

          if (args.Length > 2)
          {
            try
            {
              bisSpieltag = int.Parse(args[2]);
            }
            catch (FormatException)
            {
              Console.WriteLine("Ungültiger Spieltag. Bitte geben Sie einen gültigen Spieltag ein.");
              Environment.Exit(0);
            }
          }

          string ligaSelection = GetLigaSelection(execFolderPath, ligaSelectionParam);

          if (ligaSelection.StartsWith("ERROR: "))
          {
            Console.WriteLine("ERROR: Ungültiger Liganame. Bitte gib einen gültigen Liganamen als Parameter an.");
            Environment.Exit(0);
          }

          string pathToLiga = Path.Combine(execFolderPath, ligaSelection);
          int totalSpieltage = Directory.GetFiles(pathToLiga, "*.txt").Length;

          // Falls keine Parameter übergeben wurden, Benutzer zur Eingabe auffordern
          if (args.Length == 0)
          {
            do
            {
              Console.WriteLine($"Bis zu welchem Spieltag möchten Sie die Tabelle berechnen? (1 bis {totalSpieltage} eingeben oder leer lassen für alle Spieltage): ");
              string input = Console.ReadLine();
              if (string.IsNullOrWhiteSpace(input))
              {
                bisSpieltag = null;
                break;
              }
              else if (int.TryParse(input, out int spieltag) && spieltag >= 1 && spieltag <= totalSpieltage)
              {
                bisSpieltag = spieltag;
                break;
              }
              else
              {
                Console.WriteLine($"Ungültige Eingabe. Bitte geben Sie eine Zahl zwischen 1 und {totalSpieltage} ein.");
              }
            } while (true);
          }
          else if (args.Length < 3 && outputAsJSON != null && (outputAsJSON.ToLower() == "true" || outputAsJSON.ToLower() == "false"))
          {
            // Automatisch alle Tage ausgeben
            bisSpieltag = null;
          }
          else if (args.Length == 3)
          {
            // Parameter für den Spieltag wurde übergeben
            try
            {
              int parsedSpieltag = int.Parse(args[2]);
              if (parsedSpieltag < 1 || parsedSpieltag > totalSpieltage)
              {
                Console.WriteLine($"Ungültige Eingabe. Bitte geben Sie eine Zahl zwischen 1 und {totalSpieltage} ein.");
                Environment.Exit(0);
              }
              bisSpieltag = parsedSpieltag;
            }
            catch (FormatException)
            {
              Console.WriteLine("Ungültiger Spieltag. Bitte geben Sie einen gültigen Spieltag ein.");
              Environment.Exit(0);
            }
          }

          List<MatchResult> matchResultsList = GetData(pathToLiga, bisSpieltag);
          List<TeamResult> ligaResultsList = GroupByTeamAndPoints(matchResultsList);

          if (ligaSelectionParam == null) { Console.Clear(); }
          else { Console.WriteLine("\n"); }

          if (outputAsJSON == "true" || outputAsJSON == "True")
          {
            // Ausgabe der Ligaergebnisse als JSON
            Console.WriteLine(ConvertToJson(ligaResultsList));
          }
          else
          {
            PrintLigaTable(ligaResultsList, ligaSelection, bisSpieltag);
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