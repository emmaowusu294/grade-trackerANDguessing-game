namespace GradeTracker.Core;

// Converts a score into a letter grade.
public static class GradeCalculator
{
    public const int MinScore = 0;
    public const int MaxScore = 100;

    public static bool IsValidScore(int score) => score >= MinScore && score <= MaxScore;

    public static char GetGrade(int score)
    {
        if (!IsValidScore(score))
        {
            throw new ArgumentOutOfRangeException(nameof(score), $"Score must be between {MinScore} and {MaxScore}.");
        }

        if (score >= 80) return 'A';
        if (score >= 70) return 'B';
        if (score >= 60) return 'C';
        if (score >= 50) return 'D';
        return 'F';
    }
}
