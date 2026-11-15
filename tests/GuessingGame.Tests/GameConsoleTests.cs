using GuessingGame.App;

namespace GuessingGame.Tests;

public class GameConsoleTests
{
    private static string Run(string input, int secret = 42)
    {
        var output = new StringWriter();
        new GameConsole(new StringReader(input), output, () => secret).Run();
        return output.ToString();
    }

    private static string Lines(params string[] lines) => string.Join(Environment.NewLine, lines) + Environment.NewLine;

    [Theory]
    [InlineData("1|10|80|42|2", new[] { "Too low!", "Too high!", "Tries left: 5", "Correct! You won in 3 tries.", "Goodbye!" })]
    [InlineData("1|1|1|1|1|1|1|1|2", new[] { "Tries left: 1", "Out of tries. You lose! The number was 42.", "Goodbye!" })]
    [InlineData("1|42|1|42|2", new[] { "Correct! You won in 1 try." })]
    public void Run_PlaysRoundsUntilQuit(string input, string[] expectedTexts)
    {
        string output = Run(Lines(input.Split('|')));

        foreach (string expected in expectedTexts)
        {
            Assert.Contains(expected, output);
        }
    }

    [Theory]
    [InlineData("9|2", "Invalid choice. Please enter 1 or 2.")]
    [InlineData("|2", "Invalid choice. Please enter 1 or 2.")]
    [InlineData("1|abc|42|2", "That is not a whole number. Try again.")]
    [InlineData("1|4.5|42|2", "That is not a whole number. Try again.")]
    [InlineData("1|0|42|2", "Your guess must be between 1 and 100.")]
    [InlineData("1|101|42|2", "Your guess must be between 1 and 100.")]
    [InlineData("1|50", "Input closed. Goodbye!")]
    [InlineData("", "Input closed. Goodbye!")]
    public void Run_HandlesBadInputWithoutCrashing(string input, string expectedMessage)
    {
        string text = input.Length == 0 ? "" : Lines(input.Split('|'));

        string output = Run(text);

        Assert.Contains(expectedMessage, output);
    }
}
