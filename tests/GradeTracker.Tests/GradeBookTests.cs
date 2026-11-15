using GradeTracker.Core;

namespace GradeTracker.Tests;

public class GradeBookTests
{
    private static GradeBook BookWith(params int[] scores)
    {
        var book = new GradeBook(scores.Length);
        for (int i = 0; i < scores.Length; i++)
        {
            book.Add($"Student{i + 1}", scores[i]);
        }

        return book;
    }

    [Theory]
    [InlineData(new[] { 90, 70, 50 }, 70.0, 90, 50)]
    [InlineData(new[] { 75 }, 75.0, 75, 75)]
    [InlineData(new[] { 0, 100 }, 50.0, 100, 0)]
    [InlineData(new[] { 81, 82 }, 81.5, 82, 81)]
    public void Statistics_AreCalculatedFromStoredScores(int[] scores, double average, int highest, int lowest)
    {
        GradeBook book = BookWith(scores);

        Assert.Equal(average, book.Average(), 2);
        Assert.Equal(highest, book.Highest());
        Assert.Equal(lowest, book.Lowest());
    }

    [Theory]
    [InlineData("", 50)]
    [InlineData("   ", 50)]
    [InlineData("Ama", -1)]
    [InlineData("Ama", 101)]
    public void Add_RejectsEmptyNameOrInvalidScore(string name, int score)
    {
        var book = new GradeBook(1);

        Assert.ThrowsAny<ArgumentException>(() => book.Add(name, score));
        Assert.Equal(0, book.Count);
    }

    [Fact]
    public void Add_StoresTrimmedNameAndRejectsWhenFull()
    {
        var book = new GradeBook(1);
        book.Add("  Kofi  ", 65);

        Assert.Equal("Kofi", book.GetName(0));
        Assert.Equal(65, book.GetScore(0));
        Assert.Throws<InvalidOperationException>(() => book.Add("Esi", 70));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public void Constructor_RejectsCapacityBelowOne(int capacity)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new GradeBook(capacity));
    }

    [Fact]
    public void Statistics_ThrowWhenBookIsEmpty()
    {
        var book = new GradeBook(2);

        Assert.Throws<InvalidOperationException>(() => book.Average());
        Assert.Throws<InvalidOperationException>(() => book.Highest());
        Assert.Throws<InvalidOperationException>(() => book.Lowest());
    }
}
