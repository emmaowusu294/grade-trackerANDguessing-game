using GuessingGame.App;
using GuessingGame.Core;

// Wire the console UI to the real console and a random secret number that the user is supposed to guess.
var console = new GameConsole(Console.In, Console.Out, () => Random.Shared.Next(NumberGame.MinNumber, NumberGame.MaxNumber + 1));
console.Run();
