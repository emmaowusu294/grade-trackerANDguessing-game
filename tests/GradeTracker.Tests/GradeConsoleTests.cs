using GradeTracker.App;

namespace GradeTracker.Tests;

public class GradeConsoleTests
{
    private static string Run(string input)
    {
        var output = new StringWriter();
        new GradeConsole(new StringReader(input), output).Run();
        return output.ToString();
    }

    private static string Lines(string input) => string.Join(Environment.NewLine, input.Split('|')) + Environment.NewLine;

    [Theory]
    [InlineData("1|2|Ama|85|Kofi|45|3", new[] { "Ama: 85 (A)", "Kofi: 45 (F)", "Average: 65.00", "Highest: 85", "Lowest: 45", "Goodbye!" })]
    [InlineData("1|1|Esi|72|2|3", new[] { "Esi: 72 (B)", "Average: 72.00" })]
    [InlineData("1|1|Esi|72|1|1|Yaw|55|2|3", new[] { "Yaw: 55 (D)", "Average: 55.00" })]
    public void Run_EntersStudentsAndShowsReport(string input, string[] expectedTexts)
    {
        string output = Run(Lines(input));

        foreach (string expected in expectedTexts)
        {
            Assert.Contains(expected, output);
        }
    }

    [Theory]
    [InlineData("7|3", "Invalid choice. Please enter 1, 2 or 3.")]
    [InlineData("2|3", "No students yet. Choose option 1 first.")]
    [InlineData("1|two|1|Ama|50|3", "That is not a whole number. Try again.")]
    [InlineData("1|0|1|Ama|50|3", "Please enter a number between 1 and 100.")]
    [InlineData("1|1||Ama|50|3", "Name cannot be empty. Try again.")]
    [InlineData("1|1|Ama|abc|50|3", "That is not a whole number. Try again.")]
    [InlineData("1|1|Ama|101|50|3", "Please enter a number between 0 and 100.")]
    [InlineData("1|1|Ama", "Input closed. Goodbye!")]
    public void Run_HandlesBadInputWithoutCrashing(string input, string expectedMessage)
    {
        string output = Run(Lines(input));

        Assert.Contains(expectedMessage, output);
    }
}
