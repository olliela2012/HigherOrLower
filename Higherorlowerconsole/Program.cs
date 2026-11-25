using HigherOrLowerLogic;

const int width = 62;
var quit = false;

try
{
    Console.Title = "HIGHER / LOWER - The Card Terminal";
    Console.CursorVisible = false;
    Console.ForegroundColor = ConsoleColor.Green;

    while (!quit)
    {
        var score = 0;
        var game = new Game();
        var currentCard = game.nextcard();
        var message = "Press H for higher or L for lower.";
        var playing = true;
        var gameOver = false;
        var finalMessage = "";

        while (playing)
        {
            DrawScreen(currentCard, score, message, width);
            var key = Console.ReadKey(intercept: true).Key;

            if (key == ConsoleKey.Q || key == ConsoleKey.Escape)
            {
                playing = false;
                quit = true;
                finalMessage = "SESSION ENDED BY PLAYER";
                continue;
            }

            if (key != ConsoleKey.H && key != ConsoleKey.L)
            {
                message = "UNKNOWN INPUT. USE H, L, OR Q.";
                continue;
            }

            Card nextCard;
            try
            {
                nextCard = game.nextcard();
            }
            catch (InvalidOperationException)
            {
                playing = false;
                gameOver = true;
                finalMessage = "DECK EMPTY - YOU PLAYED THE WHOLE PACK!";
                continue;
            }

            var comparison = nextCard.CompareTo(currentCard);
            var guessedHigher = key == ConsoleKey.H;
            var correct = comparison == 0 || (guessedHigher && comparison > 0) || (!guessedHigher && comparison < 0);

            if (!correct)
            {
                DrawScreen(currentCard, score, guessedHigher ? "GUESS: HIGHER" : "GUESS: LOWER", width);
                Console.WriteLine();
                Console.WriteLine($"  NEXT CARD: {CardName(nextCard)}");
                playing = false;
                gameOver = true;
                finalMessage = "INCORRECT GUESS - GAME OVER";
                continue;
            }

            score++;
            currentCard = nextCard;
            message = comparison == 0
                ? "SAME VALUE - SAFE! SCORE INCREASED."
                : "CORRECT! NEXT CARD, PLEASE...";
        }

        DrawScreen(currentCard, score, finalMessage, width);
        if (!gameOver)
        {
            continue;
        }

        Console.WriteLine();
        Console.Write("  PLAY AGAIN? [Y/N]: ");
        ConsoleKey replayKey;
        do
        {
            replayKey = Console.ReadKey(intercept: true).Key;
        } while (replayKey != ConsoleKey.Y && replayKey != ConsoleKey.N);

        Console.WriteLine(replayKey == ConsoleKey.Y ? "Y" : "N");
        quit = replayKey == ConsoleKey.N;
    }
}
finally
{
    Console.ResetColor();
    Console.CursorVisible = true;
}

static void DrawScreen(Card card, int score, string message, int width)
{
    Console.Clear();
    var line = new string('=', width);
    Console.WriteLine(line);
    Console.WriteLine("  H I G H E R   /   L O W E R       [ CARD TERMINAL v1.0 ]");
    Console.WriteLine(line);
    Console.WriteLine();
    Console.WriteLine("  CURRENT CARD                         SCORE");
    Console.WriteLine("  +-------------+                      +------+");
    Console.WriteLine($"  | {CardRank(card),-2} {SuitSymbol(card.Suit)}        |                      | {score,4} |");
    Console.WriteLine("  |             |                      +------+");
    Console.WriteLine("  |             |");
    Console.WriteLine($"  |        {SuitSymbol(card.Suit)} {CardRank(card),2} |");
    Console.WriteLine("  |             |");
    Console.WriteLine("  +-------------+");
    Console.WriteLine();
    Console.WriteLine($"  {message}");
    Console.WriteLine();
    Console.WriteLine("  [H] HIGHER    [L] LOWER    [Q] QUIT");
    Console.WriteLine("  " + line);
}

static string CardName(Card card) => $"{CardRank(card)} OF {card.Suit.ToString().ToUpperInvariant()}";

static string CardRank(Card card) => card.FaceValue switch
{
    1 => "A",
    11 => "J",
    12 => "Q",
    13 => "K",
    _ => card.FaceValue.ToString()
};

static char SuitSymbol(Suit suit) => suit switch
{
    Suit.Hearts => 'H',
    Suit.Diamonds => 'D',
    Suit.Clubs => 'C',
    Suit.Spades => 'S',
    _ => '?'
};
