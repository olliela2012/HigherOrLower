using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace HigherOrLowerLogic
{
    public class Game

    {
        private readonly Card[] _Deck = new Deck().GetAllCards();

        private int _deckindex;
      public Card nextcard()
        {
            if (_deckindex >= _Deck.Length)
            {
                throw new InvalidOperationException("No more cards in the deck.");
            }
            return _Deck[_deckindex++];
        }

    }
}
