using Xunit;
using LigaResults;

namespace LigaResultsTest
{
  public class UnitTest1
  {
    [Fact]
    public void TestGetLigaResults()
    {
      // Arrange
      string ligaSelection = "Bundesliga";
      string outputAsArray = "false";

      // Act
      var results = Program.GetLigaResults(ligaSelection, outputAsArray);

      // Assert
      Assert.NotNull(results);
      Assert.True(results.Count > 0);
    }
  }
}
