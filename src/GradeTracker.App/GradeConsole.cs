using System.Globalization;
using GradeTracker.Core;

namespace GradeTracker.App;

// Runs the menu, collects student data and prints the report over any reader and writer.
public class GradeConsole
{
    public const int MaxStudents = 100;

    private readonly TextReader _input;
    private readonly TextWriter _output;
    private GradeBook? _gradeBook;
    private bool _inputClosed;

    public GradeConsole(TextReader input, TextWriter output)
    {
        _input = input;
        _output = output;
    }

    public void Run()
    {
        _output.WriteLine("=== Grade Tracker ===");
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
        _output.WriteLine("1. Enter students");
        _output.WriteLine("2. Show report");
        _output.WriteLine("3. Quit");
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
                EnterStudents();
                return true;
            case "2":
                ShowReport();
                return true;
            case "3":
                return false;
            default:
                _output.WriteLine("Invalid choice. Please enter 1, 2 or 3.");
                return true;
        }
    }

    // Replaces any previous data with a new set of students.
    private void EnterStudents()
    {
        int? count = ReadNumber("How many students? ", 1, MaxStudents);
        if (count is null)
        {
            return;
        }

        var book = new GradeBook(count.Value);
        while (!book.IsFull)
        {
            int number = book.Count + 1;
            string? name = ReadName($"Name of student {number}: ");
            int? score = name is null ? null : ReadNumber($"Score for {name} (0-100): ", GradeCalculator.MinScore, GradeCalculator.MaxScore);
            if (score is null)
            {
                return;
            }

            book.Add(name!, score.Value);
        }

        _gradeBook = book;
        ShowReport();
    }

    private void ShowReport()
    {
        if (_gradeBook is null)
        {
            _output.WriteLine("No students yet. Choose option 1 first.");
            return;
        }

        _output.WriteLine();
        _output.WriteLine("--- Report ---");
        for (int i = 0; i < _gradeBook.Count; i++)
        {
            int score = _gradeBook.GetScore(i);
            _output.WriteLine($"{_gradeBook.GetName(i)}: {score} ({GradeCalculator.GetGrade(score)})");
        }

        _output.WriteLine(string.Create(CultureInfo.InvariantCulture, $"Average: {_gradeBook.Average():F2}"));
        _output.WriteLine($"Highest: {_gradeBook.Highest()}");
        _output.WriteLine($"Lowest: {_gradeBook.Lowest()}");
    }

    // Keeps asking until a whole number in range is entered; null means input closed.
    private int? ReadNumber(string prompt, int min, int max)
    {
        while (true)
        {
            _output.Write(prompt);
            string? line = ReadLine();
            if (line is null)
            {
                return null;
            }

            if (!int.TryParse(line, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
            {
                _output.WriteLine("That is not a whole number. Try again.");
            }
            else if (value < min || value > max)
            {
                _output.WriteLine($"Please enter a number between {min} and {max}.");
            }
            else
            {
                return value;
            }
        }
    }

    // Keeps asking until a non-empty name is entered; null means input closed.
    private string? ReadName(string prompt)
    {
        while (true)
        {
            _output.Write(prompt);
            string? line = ReadLine();
            if (line is null || line.Length > 0)
            {
                return line;
            }

            _output.WriteLine("Name cannot be empty. Try again.");
        }
    }

    private string? ReadLine()
    {
        string? line = _input.ReadLine();
        _inputClosed = line is null;
        return line?.Trim();
    }
}
