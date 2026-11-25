namespace HigherOrLowerLogic
{
    public class Card : IEquatable<Card>, IComparable<Card>
    {
        public int FaceValue { get; set; }
        public Suit Suit { get; set; }
        public Card(int faceValue, Suit suit)
        {
            FaceValue = faceValue;
            Suit = suit;
        }

        public bool Equals(Card? other)
        {
            return FaceValue == other?.FaceValue && Suit == other?.Suit;
        }

        public int CompareTo(Card? other)
        {
            if (other == null) return 1;
            if (FaceValue != other.FaceValue) return FaceValue.CompareTo(other.FaceValue);
            return 0;
        }
        public static bool operator >(Card left, Card right)
        {
            return left.CompareTo(right) > 0;
        }
        public static bool operator <(Card left, Card right)
        {
            return left.CompareTo(right) < 0;
        }
        public override string ToString()
        {
            return $"{facevaluename(FaceValue)} of {Suit}";

        }
        private static string facevaluename(int facevalue)
        {
            return facevalue switch
            {
                1 => "Ace",
                11 => "Jack",
                12 => "Queen",
                13 => "King",
                _ => facevalue.ToString()
            };
        }
    }

}

