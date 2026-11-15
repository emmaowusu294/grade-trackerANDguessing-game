using System.Globalization;
using GuessingGame.Core;

namespace GuessingGame.App;

// Runs the menu and the game rounds over any reader and writer.
public class GameConsole
{
    private readonly TextReader _input;
    private readonly TextWriter _output;
    private readonly Func<int> _nextSecret;
    private bool _inputClosed;

    public GameConsole(TextReader input, TextWriter output, Func<int> nextSecret)
    {
        _input = input;
        _output = output;
        _nextSecret = nextSecret;
    }

    public void Run()
    {
        _output.WriteLine("=== Number Guessing Game ===");
        bool keepGoing = true;
        while (keepGoing && !_inputClosed)
        {
            keepGoing = HandleMenuChoice(ReadMenuChoice());
        }

        _output.WriteLine(_inputClosed ? "Input closed. Goodbye!" : "Goodbye!");
    }

    private string? ReadMenuChoice()
    {
        _output.WriteLine();
        _output.WriteLine("1. Play");
        _output.WriteLine("2. Quit");
        _output.Write("Choose an option: ");
        return ReadLine();
    }

    // Returns false when the user wants to quit.
    private bool HandleMenuChoice(string? choice)
    {
        switch (choice)
        {
            case null:
                return false;
            case "1":
                PlayRound();
                return true;
            case "2":
                return false;
            default:
                _output.WriteLine("Invalid choice. Please enter 1 or 2.");
                return true;
        }
    }

    private void PlayRound()
    {
        var game = new NumberGame(_nextSecret());
        _output.WriteLine($"I'm thinking of a number between {NumberGame.MinNumber} and {NumberGame.MaxNumber}. You have {NumberGame.MaxTries} tries.");

        while (!game.IsOver)
        {
            int? guess = ReadGuess();
            if (guess is null)
            {
                return;
            }

            ShowHint(game.Guess(guess.Value), game.TriesLeft);
        }

        ShowResult(game);
    }

    // Keeps asking until a valid guess is entered; null means input closed.
    private int? ReadGuess()
    {
        while (true)
        {
            _output.Write("Enter your guess: ");
            string? line = ReadLine();
            if (line is null)
            {
                return null;
            }

            if (!int.TryParse(line, NumberStyles.Integer, CultureInfo.InvariantCulture, out int guess))
            {
                _output.WriteLine("That is not a whole number. Try again.");
            }
            else if (!NumberGame.IsInRange(guess))
            {
                _output.WriteLine($"Your guess must be between {NumberGame.MinNumber} and {NumberGame.MaxNumber}.");
            }
            else
            {
                return guess;
            }
        }
    }

    private void ShowHint(GuessResult result, int triesLeft)
    {
        if (result == GuessResult.Correct)
        {
            return;
        }

        _output.WriteLine(result == GuessResult.TooHigh ? "Too high!" : "Too low!");
        _output.WriteLine($"Tries left: {triesLeft}");
    }

    private void ShowResult(NumberGame game)
    {
        if (game.IsWon)
        {
            _output.WriteLine($"Correct! You won in {game.TriesUsed} {(game.TriesUsed == 1 ? "try" : "tries")}.");
        }
        else
        {
            _output.WriteLine($"Out of tries. You lose! The number was {game.SecretNumber}.");
        }
    }

    private string? ReadLine()
    {
        string? line = _input.ReadLine();
        _inputClosed = line is null;
        return line?.Trim();
    }
}
