using GradeTracker.Core;

namespace GradeTracker.Tests;

public class GradeCalculatorTests
{
    [Theory]
    [InlineData(100, 'A')]
    [InlineData(80, 'A')]
    [InlineData(79, 'B')]
    [InlineData(70, 'B')]
    [InlineData(69, 'C')]
    [InlineData(60, 'C')]
    [InlineData(59, 'D')]
    [InlineData(50, 'D')]
    [InlineData(49, 'F')]
    [InlineData(0, 'F')]
    public void GetGrade_ReturnsLetterForBoundaries(int score, char expected)
    {
        Assert.Equal(expected, GradeCalculator.GetGrade(score));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void GetGrade_RejectsScoreOutOfRange(int score)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => GradeCalculator.GetGrade(score));
    }
}
