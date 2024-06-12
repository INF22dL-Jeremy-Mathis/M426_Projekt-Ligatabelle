namespace LigaResults
{
  public class Program
  {
    // Validates if the execution folder is "soccer-results"
    public static bool IsSoccerResultsFolder(string path)
    {
      return path.Replace('/', '\\').Contains("\\soccer-results");
    }

    // Retrieves available ligas
    public static List<string> GetAvailableLigas(string path)
    {
      return Directory.GetDirectories(path).Select(Path.GetFileName).ToList();
    }

    // Selects a liga based on input
    public static string SelectLiga(List<string> ligas, string input)
    {
      if (int.TryParse(input, out int selection) && selection >= 1 && selection <= ligas.Count)
      {
        return ligas[selection - 1];
      }
      return ligas.Contains(input) ? input : null;
    }

    // Retrieves match data (.txt files) up to a specified playday
    public static List<MatchResult> GetData(string ligaPath, int? maxPlayday = null)
    {
      var files = Directory.GetFiles(ligaPath, "*.txt").OrderBy(f => f).Take(maxPlayday ?? int.MaxValue);
      return files.SelectMany(file => File.ReadAllLines(file).Select(ParseMatchResult)).ToList();
    }

    // Parses a match result from a string
    public static MatchResult ParseMatchResult(string match)
    {
      var teams = match.Split(':').Select(t => t.Trim()).ToArray();

      var team1Parts = teams[0].Split(' ');
      var team1Name = string.Join(" ", team1Parts.Take(team1Parts.Length - 1));
      var team1Goals = int.Parse(team1Parts.Last());

      var team2Parts = teams[1].Split(' ');
      var team2Goals = int.Parse(team2Parts.First());
      var team2Name = string.Join(" ", team2Parts.Skip(1));

      return new MatchResult
      {
        Team1 = new TeamMatchResult
        {
          TeamName = team1Name,
          GoalsShot = team1Goals,
          GoalsTaken = team2Goals,
          Wins = team1Goals > team2Goals ? 1 : 0,
          Losses = team1Goals < team2Goals ? 1 : 0,
          Draws = team1Goals == team2Goals ? 1 : 0
        },
        Team2 = new TeamMatchResult
        {
          TeamName = team2Name,
          GoalsShot = team2Goals,
          GoalsTaken = team1Goals,
          Wins = team2Goals > team1Goals ? 1 : 0,
          Losses = team2Goals < team1Goals ? 1 : 0,
          Draws = team2Goals == team1Goals ? 1 : 0
        }
      };
    }

    // Groups match results by team and calculates aggregate statistics
    public static List<TeamResult> GroupByTeam(List<MatchResult> results)
    {
      return results
          .SelectMany(r => new[] { r.Team1, r.Team2 })
          .GroupBy(t => t.TeamName)
          .Select(g => new TeamResult
          {
            Name = g.Key,
            Siege = g.Sum(t => t.Wins),
            Niederlagen = g.Sum(t => t.Losses),
            Unentschieden = g.Sum(t => t.Draws),
            Tore = g.Sum(t => t.GoalsShot),
            Gegentore = g.Sum(t => t.GoalsTaken),
            Tordifferenz = g.Sum(t => t.GoalsShot) - g.Sum(t => t.GoalsTaken),
            Punkte = g.Sum(t => t.Wins) * 3 + g.Sum(t => t.Draws)
          })
          .ToList();
    }

    // Sorts teams based on multiple criteria
    public static List<TeamResult> SortTeams(List<TeamResult> teams)
    {
      return teams
          .OrderByDescending(t => t.Punkte)
          .ThenByDescending(t => t.Tordifferenz)
          .ThenByDescending(t => t.Siege)
          .ThenBy(t => t.Name)
          .Select((team, index) =>
          {
            team.Rang = index + 1;
            return team;
          })
          .ToList();
    }

    // Prints the liga table
    public static void PrintLigaTable(List<TeamResult> ligaResults, string liga, int? bisSpieltag)
    {
      var header = bisSpieltag.HasValue
          ? $"\nLigatabelle bis Spieltag {bisSpieltag}: {liga}"
          : $"\nLigatabelle: {liga} - Endergebnisse";

      Console.WriteLine($"{header,-20}{"",35}");
      Console.WriteLine(new string('-', 120));
      Console.WriteLine($"{"Rang",5}{"",2}{"Name",-41}{"Punkte",6}{"",2}{"Siege",5}{"",2}{"Niederlagen",11}{"",2}{"Unentschieden",13}{"",2}{"Tore",4}{"",2}{"Gegentore",9}{"",2}{"Tordifferenz",12}");
      Console.WriteLine(new string('-', 120));

      foreach (var result in ligaResults)
      {
        Console.WriteLine($"{result.Rang,5}{"",2}{result.Name,-41}{result.Punkte,6}{"",2}{result.Siege,5}{"",2}{result.Niederlagen,11}{"",2}{result.Unentschieden,13}{"",2}{result.Tore,4}{"",2}{result.Gegentore,9}{"",2}{result.Tordifferenz,12}");
      }
    }

    // Queries the user for the maximum playday
    public static int? GetMaxPlaydaySelection(int totalSpieltage)
    {
      while (true)
      {
        Console.WriteLine($"Bis zu welchem Spieltag möchten Sie die Tabelle berechnen? (1 bis {totalSpieltage} eingeben oder leer lassen für alle Spieltage): ");
        var input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input)) return null;
        if (int.TryParse(input, out var spieltag) && spieltag >= 1 && spieltag <= totalSpieltage) return spieltag;
        Console.WriteLine($"Ungültige Eingabe. Bitte geben Sie eine Zahl zwischen 1 und {totalSpieltage} ein.");
      }
    }

    // Main method
    public static void Main(string[] args)
    {
      while (true)
      {
        int? bisSpieltag = null;

        var execFolderPath = Directory.GetCurrentDirectory();

        if (!IsSoccerResultsFolder(execFolderPath))
        {
          Console.WriteLine("Ungültiger Ordner, stelle sicher dass die Anwendung im Ordner \"soccer-results\" gestartet wurde");
          Environment.Exit(0);
        }

        var ligas = GetAvailableLigas(execFolderPath);
        Console.WriteLine("Verfügbare Ligen:");
        for (var i = 0; i < ligas.Count; i++) Console.WriteLine($"[{i + 1}] {ligas[i]}");

        string ligaSelection;
        do
        {
          Console.Write("\nNummer oder Name der Liga eingeben oder mittels 'exit' die Anwendung verlassen: ");
          var input = Console.ReadLine();

          if (input.ToLower() == "exit") Environment.Exit(0);

          ligaSelection = SelectLiga(ligas, input);

          if (ligaSelection == null)
          {
            Console.WriteLine($"ERROR: Ungültige Ligaauswahl. Bitte geben Sie eine Zahl zwischen 1 und {ligas.Count} ein oder einen gültigen Liganamen.");
          }
        } while (ligaSelection == null);

        var pathToLiga = Path.Combine(execFolderPath, ligaSelection);
        var totalSpieltage = Directory.GetFiles(pathToLiga, "*.txt").Length;

        bisSpieltag = GetMaxPlaydaySelection(totalSpieltage);
        var matchResultsList = GetData(pathToLiga, bisSpieltag);
        var groupedTeams = GroupByTeam(matchResultsList);
        var ligaResultsList = SortTeams(groupedTeams);

        PrintLigaTable(ligaResultsList, ligaSelection, bisSpieltag);

        Directory.SetCurrentDirectory(execFolderPath);
        Console.Write("\n\nDrücke eine Beliebige Taste um fortzufahren");
        Console.ReadKey();
        Console.Clear();
      }
    }
  }
}
