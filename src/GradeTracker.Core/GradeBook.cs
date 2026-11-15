namespace GradeTracker.Core;

// Stores student names and scores in fixed-size arrays and computes statistics.
public class GradeBook
{
    private readonly string[] _names;
    private readonly int[] _scores;

    public GradeBook(int capacity)
    {
        if (capacity < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be at least 1.");
        }

        _names = new string[capacity];
        _scores = new int[capacity];
    }

    public int Capacity => _scores.Length;
    public int Count { get; private set; }
    public bool IsFull => Count == Capacity;

    public void Add(string name, int score)
    {
        if (IsFull)
        {
            throw new InvalidOperationException("The grade book is full.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty.", nameof(name));
        }

        if (!GradeCalculator.IsValidScore(score))
        {
            throw new ArgumentOutOfRangeException(nameof(score), $"Score must be between {GradeCalculator.MinScore} and {GradeCalculator.MaxScore}.");
        }

        _names[Count] = name.Trim();
        _scores[Count] = score;
        Count++;
    }

    public string GetName(int index) => _names[CheckIndex(index)];
    public int GetScore(int index) => _scores[CheckIndex(index)];

    public double Average()
    {
        EnsureNotEmpty();
        int total = 0;
        for (int i = 0; i < Count; i++)
        {
            total += _scores[i];
        }

        return (double)total / Count;
    }

    public int Highest()
    {
        EnsureNotEmpty();
        int highest = _scores[0];
        for (int i = 1; i < Count; i++)
        {
            highest = Math.Max(highest, _scores[i]);
        }

        return highest;
    }

    public int Lowest()
    {
        EnsureNotEmpty();
        int lowest = _scores[0];
        for (int i = 1; i < Count; i++)
        {
            lowest = Math.Min(lowest, _scores[i]);
        }

        return lowest;
    }

    private int CheckIndex(int index)
    {
        if (index < 0 || index >= Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return index;
    }

    private void EnsureNotEmpty()
    {
        if (Count == 0)
        {
            throw new InvalidOperationException("No scores have been added.");
        }
    }
}
