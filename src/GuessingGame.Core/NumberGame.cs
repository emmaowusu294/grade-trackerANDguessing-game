namespace GuessingGame.Core;

// Holds the state of one round: secret number, tries used and whether it is over.
public class NumberGame
{
    public const int MinNumber = 1;
    public const int MaxNumber = 100;
    public const int MaxTries = 7;

    private readonly int _secretNumber;
    private int _triesUsed;

    // The secret is passed in so tests can control it.
    public NumberGame(int secretNumber)
    {
        if (!IsInRange(secretNumber))
        {
            throw new ArgumentOutOfRangeException(nameof(secretNumber), $"Secret must be between {MinNumber} and {MaxNumber}.");
        }

        _secretNumber = secretNumber;
    }

    public int SecretNumber => _secretNumber;
    public int TriesUsed => _triesUsed;
    public int TriesLeft => MaxTries - _triesUsed;
    public bool IsWon { get; private set; }
    public bool IsOver => IsWon || TriesLeft == 0;

    public static bool IsInRange(int number) => number >= MinNumber && number <= MaxNumber;

    // Checks a guess and uses up one try.
    public GuessResult Guess(int guess)
    {
        if (IsOver)
        {
            throw new InvalidOperationException("The game is already over.");
        }

        if (!IsInRange(guess))
        {
            throw new ArgumentOutOfRangeException(nameof(guess), $"Guess must be between {MinNumber} and {MaxNumber}.");
        }

        _triesUsed++;
        GuessResult result = Compare(guess);
        IsWon = result == GuessResult.Correct;
        return result;
    }

    private GuessResult Compare(int guess)
    {
        if (guess < _secretNumber)
        {
            return GuessResult.TooLow;
        }

        return guess > _secretNumber ? GuessResult.TooHigh : GuessResult.Correct;
    }
}
