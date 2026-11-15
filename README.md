# Guessing Game & Grade Tracker

Two small C# console apps (.NET 10) in one solution, `GuessingGameGradeTracker.sln`.

- **Guessing Game**: guess a random number from 1 to 100 in at most 7 tries. After each guess you are told "Too high!" or "Too low!" and how many tries are left.
- **Grade Tracker**: enter a number of students with their names and scores (0 to 100). The app shows each student's grade plus the class average, highest and lowest score.

Both apps show a menu that repeats until you choose to quit.

## Requirements

- .NET 10 SDK, or a newer SDK with the .NET 10 runtime installed. The projects target `net10.0`.

## How to run

From the repository root:

```bash
dotnet run --project src/GuessingGame.App
dotnet run --project src/GradeTracker.App
```

## How to test

```bash
dotnet build
dotnet test
```

There are 15 xUnit test methods (6 in `GuessingGame.Tests`, 9 in `GradeTracker.Tests`). Most are `[Theory]` tests with several `[InlineData]` cases. The console UI classes are tested by feeding scripted input through `StringReader` and checking what was written to a `StringWriter`.

## Project structure

```
GuessingGameGradeTracker.sln
src/
  GuessingGame.Core/    NumberGame (secret number, guess checking, tries left, game over), GuessResult
  GuessingGame.App/     Program.cs (wiring only) and GameConsole (menu and console I/O)
  GradeTracker.Core/    GradeBook (names and scores in arrays, average, highest, lowest), GradeCalculator (score to letter)
  GradeTracker.App/     Program.cs (wiring only) and GradeConsole (menu and console I/O)
tests/
  GuessingGame.Tests/   NumberGameTests, GameConsoleTests
  GradeTracker.Tests/   GradeCalculatorTests, GradeBookTests, GradeConsoleTests
```

### Why it's split this way

- **Core projects hold the rules and make no Console calls.** The logic can be tested directly with plain values and reused by another front end (GUI, web) without changes.
- **App projects only handle input and output.** `GameConsole` and `GradeConsole` take a `TextReader` and a `TextWriter` rather than using `Console` directly. That means tests can drive the full menu with a `StringReader` and read the output from a `StringWriter`.
- **`Program.cs` is a few lines of wiring**: it connects the UI class to the real console (and, for the game, to a random number source).
- **The secret number is passed in.** `NumberGame` receives its secret through its constructor, and `GameConsole` receives a `Func<int>` that produces one, so tests are repeatable while the real app uses `Random.Shared`.
- **Each class has one job.** `GradeCalculator` only maps scores to letters, `GradeBook` only stores scores and calculates statistics, and the console classes only talk to the user.

## Grade scale

| Grade | Score    |
|-------|----------|
| A     | 80 - 100 |
| B     | 70 - 79  |
| C     | 60 - 69  |
| D     | 50 - 59  |
| F     | below 50 |

## Assumptions

- **Guesses and scores must be whole numbers.** Input such as `79.5` or `4.5` is rejected and you are asked again. This avoids unclear grade boundaries such as 79.5.
- **Invalid guesses do not use up a try.** A non-number or a number outside 1 to 100 shows an error and asks again, and the try counter does not change.
- **Bad input re-prompts instead of returning to the menu.** If you enter a bad student count, name or score, only that one question is asked again. Students you have already entered are kept.
- **Numbers are parsed with the invariant culture.** Leading and trailing spaces are ignored and a leading `+` or `-` is accepted. Thousands separators such as `1,000` are not.
- **The class size is 1 to 100 students.** The upper limit stops huge or accidental numbers.
- **Entering students again replaces the previous class.** Choosing "Enter students" a second time starts a new grade book rather than adding to the old one.
- **The report is shown automatically** after all students are entered. It can be shown again with "Show report".
- **Names are trimmed and must not be empty.** Duplicate names are allowed.
- **The average is shown to 2 decimal places** with a `.` decimal separator, whatever your system's regional settings.
- **Highest and lowest show the score only**, not which student got it.
- **Closed input exits cleanly.** If input ends (for example Ctrl+Z / Ctrl+D, or a piped file runs out), the app prints `Input closed. Goodbye!` and exits. A round or a student entry that was in progress is discarded.
- **The secret number is revealed when you lose.**

## Known limitations

- Data is kept in memory only. Nothing is saved when the app exits.
- The number range (1 to 100), try limit (7) and grade boundaries are constants in code, not settings.
- Each round is independent. The game does not keep scores or a win/loss history.
- The UI is English only.
