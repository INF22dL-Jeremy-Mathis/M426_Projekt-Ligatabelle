namespace LigaResults.Tests
{
    public class ProgramTests
    {
        /// <summary>
        /// Gets the path to the TestData directory containing the soccer-results folder.
        /// </summary>
        /// <returns>The full path to the soccer-results directory.</returns>
        private string GetTestDataPath()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\..\.."));
            return Path.Combine(projectDirectory, "TestData/soccer-results");
        }

        /// <summary>
        /// Validates if the IsSoccerResultsFolder method returns true when the path contains "soccer-results".
        /// </summary>
        [Fact]
        public void IsSoccerResultsFolder_ShouldReturnTrue_WhenPathContainsSoccerResults()
        {
            // Arrange
            string path = GetTestDataPath();

            // Act
            bool result = Program.IsSoccerResultsFolder(path);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Validates if the IsSoccerResultsFolder method returns false when the path does not contain "soccer-results".
        /// </summary>
        [Fact]
        public void IsSoccerResultsFolder_ShouldReturnFalse_WhenPathDoesNotContainSoccerResults()
        {
            // Arrange
            string path = Path.GetTempPath();

            // Act
            bool result = Program.IsSoccerResultsFolder(path);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Validates if the GetAvailableLigas method returns the expected list of ligas.
        /// </summary>
        [Fact]
        public void GetAvailableLigas_ShouldReturnListOfLigas()
        {
            // Arrange
            string path = GetTestDataPath();
            var expectedLigas = new List<string>
            {
                "bundesliga",
                "la-liga",
                "ligue-1",
                "premier-league",
                "serie-a",
                "sort-difference",
                "sort-name",
                "sort-points",
                "sort-wins"
            };

            // Act
            var result = Program.GetAvailableLigas(path);

            // Assert
            Assert.Equal(expectedLigas, result);
        }

        /// <summary>
        /// Validates if the SelectLiga method returns the correct liga name based on the input.
        /// </summary>
        /// <param name="input">User input.</param>
        /// <param name="expected">Expected liga name.</param>
        [Theory]
        [InlineData("1", "Liga1")]
        [InlineData("2", "Liga2")]
        [InlineData("Liga1", "Liga1")]
        [InlineData("Liga2", "Liga2")]
        public void SelectLiga_ShouldReturnLigaName_WhenValidInput(string input, string expected)
        {
            // Arrange
            var ligas = new List<string> { "Liga1", "Liga2" };

            // Act
            var result = Program.SelectLiga(ligas, input);

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Validates if the SelectLiga method returns null when the input is invalid.
        /// </summary>
        [Fact]
        public void SelectLiga_ShouldReturnNull_WhenInvalidInput()
        {
            // Arrange
            var ligas = new List<string> { "Liga1", "Liga2" };
            string input = "InvalidLiga";

            // Act
            var result = Program.SelectLiga(ligas, input);

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Validates if the ParseMatchResult method returns the correct match result object.
        /// </summary>
        [Fact]
        public void ParseMatchResult_ShouldReturnCorrectMatchResult()
        {
            // Arrange
            string match = "TeamA 2 : 1 TeamB";

            // Act
            var result = Program.ParseMatchResult(match);

            // Assert
            Assert.Equal("TeamA", result.Team1.TeamName);
            Assert.Equal(2, result.Team1.GoalsShot);
            Assert.Equal(1, result.Team1.GoalsTaken);
            Assert.Equal(1, result.Team1.Wins);
            Assert.Equal(0, result.Team1.Losses);
            Assert.Equal(0, result.Team1.Draws);

            Assert.Equal("TeamB", result.Team2.TeamName);
            Assert.Equal(1, result.Team2.GoalsShot);
            Assert.Equal(2, result.Team2.GoalsTaken);
            Assert.Equal(0, result.Team2.Wins);
            Assert.Equal(1, result.Team2.Losses);
            Assert.Equal(0, result.Team2.Draws);
        }

        /// <summary>
        /// Validates if the GroupByTeam method returns the correct team results.
        /// </summary>
        [Fact]
        public void GroupByTeam_ShouldReturnCorrectTeamResults()
        {
            // Arrange
            var matchResults = new List<MatchResult>
            {
                new MatchResult
                {
                    Team1 = new TeamMatchResult { TeamName = "TeamA", GoalsShot = 2, GoalsTaken = 1, Wins = 1, Losses = 0, Draws = 0 },
                    Team2 = new TeamMatchResult { TeamName = "TeamB", GoalsShot = 1, GoalsTaken = 2, Wins = 0, Losses = 1, Draws = 0 }
                },
                new MatchResult
                {
                    Team1 = new TeamMatchResult { TeamName = "TeamA", GoalsShot = 1, GoalsTaken = 1, Wins = 0, Losses = 0, Draws = 1 },
                    Team2 = new TeamMatchResult { TeamName = "TeamC", GoalsShot = 1, GoalsTaken = 1, Wins = 0, Losses = 0, Draws = 1 }
                }
            };

            // Act
            var result = Program.GroupByTeam(matchResults);
            var teamA = result.First(t => t.Name == "TeamA");

            // Assert
            Assert.Equal(1, teamA.Siege);
            Assert.Equal(0, teamA.Niederlagen);
            Assert.Equal(1, teamA.Unentschieden);
            Assert.Equal(3, teamA.Tore);
            Assert.Equal(2, teamA.Gegentore);
            Assert.Equal(1, teamA.Tordifferenz);
            Assert.Equal(4, teamA.Punkte);
        }

        /// <summary>
        /// Validates if the SortTeams method correctly sorts the teams.
        /// </summary>
        [Fact]
        public void SortTeams_ShouldReturnSortedTeams()
        {
            // Arrange
            var teamResults = new List<TeamResult>
            {
                new TeamResult { Name = "TeamB", Punkte = 4, Tordifferenz = 1, Siege = 1 },
                new TeamResult { Name = "TeamA", Punkte = 4, Tordifferenz = 2, Siege = 1 },
                new TeamResult { Name = "TeamC", Punkte = 3, Tordifferenz = 1, Siege = 1 }
            };

            // Act
            var result = Program.SortTeams(teamResults);

            // Assert
            Assert.Equal("TeamA", result[0].Name);
            Assert.Equal("TeamB", result[1].Name);
            Assert.Equal("TeamC", result[2].Name);
        }

        /// <summary>
        /// Validates if the GetMaxPlaydaySelection method returns null when the input is empty.
        /// </summary>
        [Fact]
        public void GetMaxPlaydaySelection_ShouldReturnNull_WhenInputIsEmpty()
        {
            // Arrange
            var totalSpieltage = 5;

            // Simulate user input by redirecting console input
            using var input = new StringReader("\n");
            Console.SetIn(input);

            // Act
            var result = Program.GetMaxPlaydaySelection(totalSpieltage);

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Validates if the GetMaxPlaydaySelection method returns the correct playday when the input is valid.
        /// </summary>
        [Fact]
        public void GetMaxPlaydaySelection_ShouldReturnPlayday_WhenValidInput()
        {
            // Arrange
            var totalSpieltage = 5;

            // Simulate user input by redirecting console input
            using var input = new StringReader("3\n");
            Console.SetIn(input);

            // Act
            var result = Program.GetMaxPlaydaySelection(totalSpieltage);

            // Assert
            Assert.Equal(3, result);
        }

        /// <summary>
        /// Validates if the GetMaxPlaydaySelection method prompts again when the input is invalid.
        /// </summary>
        [Fact]
        public void GetMaxPlaydaySelection_ShouldPromptAgain_WhenInvalidInput()
        {
            // Arrange
            var totalSpieltage = 5;

            // Simulate user input by redirecting console input
            using var input = new StringReader("invalid\n3\n");
            Console.SetIn(input);

            // Act
            var result = Program.GetMaxPlaydaySelection(totalSpieltage);

            // Assert
            Assert.Equal(3, result);
        }
    }
}
