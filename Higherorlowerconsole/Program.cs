using HigherOrLowerLogic;

var game = new Game();
var currentCard = game.nextcard();
var score = 0;

Console.WriteLine("Higher or Lower!");
Console.WriteLine($"The first card is the {currentCard}.");

while (true)
{
    Console.Write("Will the next card be higher or lower? (h/l): ");
    var guess = Console.ReadLine()?.Trim().ToLowerInvariant();

    if (guess != "h" && guess != "higher" && guess != "l" && guess != "lower")
    {
        Console.WriteLine("Please enter h for higher or l for lower.");
        continue;
    }

    Card nextCard;
    try
    {
        nextCard = game.nextcard();
    }
    catch (InvalidOperationException)
    {
        Console.WriteLine("There are no cards left in the deck.");
        break;
    }

    Console.WriteLine($"The next card is the {nextCard}.");
    var comparison = nextCard.CompareTo(currentCard);
    var guessedHigher = guess == "h" || guess == "higher";
    var correct = comparison == 0 || (guessedHigher && comparison > 0) || (!guessedHigher && comparison < 0);

    if (!correct)
    {
        Console.WriteLine($"Not quite! Your final score is {score}.");
        break;
    }

    score++;
    Console.WriteLine($"Correct! Score: {score}");
    currentCard = nextCard;
}
