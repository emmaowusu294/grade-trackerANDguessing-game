using GuessingGame.Core;

namespace GuessingGame.Tests;

public class NumberGameTests
{
    [Theory]
    [InlineData(50, 10, GuessResult.TooLow, 6)]
    [InlineData(50, 90, GuessResult.TooHigh, 6)]
    [InlineData(50, 50, GuessResult.Correct, 6)]
    [InlineData(1, 100, GuessResult.TooHigh, 6)]
    [InlineData(100, 1, GuessResult.TooLow, 6)]
    public void Guess_ReturnsHintAndUsesOneTry(int secret, int guess, GuessResult expected, int expectedTriesLeft)
    {
        var game = new NumberGame(secret);

        Assert.Equal(expected, game.Guess(guess));
        Assert.Equal(expectedTriesLeft, game.TriesLeft);
    }

    [Theory]
    [InlineData(0, true, true)]
    [InlineData(6, true, true)]
    [InlineData(6, false, false)]
    [InlineData(7, false, false)]
    public void Game_EndsOnWinOrAfterSevenMisses_AndRejectsMoreGuesses(int misses, bool guessCorrectly, bool expectedWon)
    {
        var game = new NumberGame(42);
        for (int i = 0; i < misses; i++)
        {
            game.Guess(1);
        }

        if (guessCorrectly)
        {
            game.Guess(42);
        }

        bool expectedOver = guessCorrectly || misses == NumberGame.MaxTries;
        Assert.Equal(expectedOver, game.IsOver);
        Assert.Equal(expectedWon, game.IsWon);
        if (expectedOver)
        {
            Assert.Throws<InvalidOperationException>(() => game.Guess(42));
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    [InlineData(-5)]
    public void Constructor_RejectsSecretOutOfRange(int secret)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new NumberGame(secret));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void Guess_RejectsOutOfRangeWithoutUsingATry(int guess)
    {
        var game = new NumberGame(50);

        Assert.Throws<ArgumentOutOfRangeException>(() => game.Guess(guess));
        Assert.Equal(NumberGame.MaxTries, game.TriesLeft);
    }
}
