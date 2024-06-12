namespace LigaResults
{
  public class Program
  {
    /// <summary>
    /// Validates if the execution folder is "soccer-results".
    /// </summary>
    /// <param name="path">The path to validate.</param>
    /// <returns>True if the folder name is "soccer-results"; otherwise, false.</returns>
    public static bool IsSoccerResultsFolder(string path)
    {
      return path.Replace('/', '\\').Contains("\\soccer-results");
    }

    /// <summary>
    /// Retrieves available ligas from the specified path.
    /// </summary>
    /// <param name="path">The path to search for ligas.</param>
    /// <returns>A list of available liga names.</returns>
    public static List<string> GetAvailableLigas(string path)
    {
      return Directory.GetDirectories(path).Select(Path.GetFileName).ToList();
    }

    /// <summary>
    /// Selects a liga based on user input.
    /// </summary>
    /// <param name="ligas">The list of available ligas.</param>
    /// <param name="input">The user input.</param>
    /// <returns>The selected liga name, or null if invalid input.</returns>
    public static string SelectLiga(List<string> ligas, string input)
    {
      if (int.TryParse(input, out int selection) && selection >= 1 && selection <= ligas.Count)
      {
        return ligas[selection - 1];
      }
      return ligas.Contains(input) ? input : null;
    }

    /// <summary>
    /// Retrieves match data from .txt files up to a specified playday.
    /// </summary>
    /// <param name="ligaPath">The path to the liga directory.</param>
    /// <param name="maxPlayday">The maximum playday to retrieve data for.</param>
    /// <returns>A list of match results.</returns>
    public static List<MatchResult> GetData(string ligaPath, int? maxPlayday = null)
    {
      var files = Directory.GetFiles(ligaPath, "*.txt").OrderBy(f => f).Take(maxPlayday ?? int.MaxValue);
      return files.SelectMany(file => File.ReadAllLines(file).Select(ParseMatchResult)).ToList();
    }

    /// <summary>
    /// Parses a match result from a string.
    /// </summary>
    /// <param name="match">The match result string.</param>
    /// <returns>A MatchResult object representing the parsed match result.</returns>
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

    /// <summary>
    /// Groups match results by team and calculates aggregate statistics.
    /// </summary>
    /// <param name="results">The list of match results.</param>
    /// <returns>A list of aggregated team results.</returns>
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

    /// <summary>
    /// Sorts teams based on Points (Descending), Goal Difference (Descending), Wins (Descending), Name (Ascending).
    /// </summary>
    /// <param name="teams">The list of TeamResult objects.</param>
    /// <returns>A sorted list of team results.</returns>
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

    /// <summary>
    /// Prints the liga table to the console.
    /// </summary>
    /// <param name="ligaResults">The list of team results to print.</param>
    /// <param name="liga">The name of the liga.</param>
    /// <param name="bisSpieltag">The maximum playday to include in the table.</param>
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

    /// <summary>
    /// Queries the user for the maximum playday to calculate the table for.
    /// </summary>
    /// <param name="totalSpieltage">The total number of playdays available.</param>
    /// <returns>The playday selected by the user, or null if all playdays should be included.</returns>
    public static int? GetMaxPlaydaySelection(int totalSpieltage)
    {
      while (true)
      {
        Console.WriteLine($"Bis zu welchem Spieltag möchten Sie die Tabelle berechnen? (1 bis {totalSpieltage} eingeben oder leer lassen für alle Spieltage): ");
        var input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input)) return null;
        if (int.TryParse(input, out var spieltag) && spieltag >= 1 && spieltag <= totalSpieltage) return spieltag;
        Console.WriteLine($"\nUngültige Eingabe. Bitte geben Sie eine Zahl zwischen 1 und {totalSpieltage} ein.");
      }
    }

    public static void Main(string[] args)
    {
      while (true)
      {
        int? bisSpieltag = null;

        var execFolderPath = Directory.GetCurrentDirectory();

        // Validate the execution folder
        if (!IsSoccerResultsFolder(execFolderPath))
        {
          Console.WriteLine("Ungültiger Ordner, stelle sicher dass die Anwendung im Ordner \"soccer-results\" gestartet wurde");
          Environment.Exit(0);
        }

        // Retrieve and display available ligas
        var ligas = GetAvailableLigas(execFolderPath);
        Console.WriteLine("Verfügbare Ligen:");
        for (var i = 0; i < ligas.Count; i++) Console.WriteLine($"[{i + 1}] {ligas[i]}");

        string ligaSelection;
        do
        {
          // Prompt user to select a liga or exit
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


        bisSpieltag = GetMaxPlaydaySelection(totalSpieltage);     // Query user for maximum playday
        var matchResultsList = GetData(pathToLiga, bisSpieltag);  // Retrieve match data from .txt files
        var groupedTeams = GroupByTeam(matchResultsList);         // Group match results by team
        var ligaResultsList = SortTeams(groupedTeams);            // Sort teams

        // Print the results table
        PrintLigaTable(ligaResultsList, ligaSelection, bisSpieltag);

        // Reset the current directory and clear the console for the next input
        Directory.SetCurrentDirectory(execFolderPath);
        Console.Write("\n\nDrücke eine Beliebige Taste um fortzufahren");
        Console.ReadKey();
        Console.Clear();
      }
    }
  }
}
