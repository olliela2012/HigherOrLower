using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HigherOrLowerLogic
{
    public class Deck
    {
        private readonly static Card[] OrderedDeck;

        static Deck()
        {
            OrderedDeck = new Card[52];
            foreach (var suit in new Suit[] { Suit.Hearts, Suit.Diamonds, Suit.Clubs, Suit.Spades })
            {
                for (var faceValue = 1; faceValue <= 13; faceValue++)
                {
                    var index = ((int)suit * 13) + (faceValue - 1);
                    OrderedDeck[index] = new Card(faceValue, suit);
                }
            }
        }

        private readonly Card[] array = (Card[])OrderedDeck.Clone();
        /// <summary>
        /// Creates a new shuffled deck
        /// </summary>
        public Deck()
        {
            Random random = new Random();
            int n = array.Count();
            while (n > 1)
            {
                n--;
                int i = random.Next(n + 1);
                var temp = array[i];
                array[i] = array[n];
                array[n] = temp;
            }
        }
        public Card[] GetAllCards()
        {
            return (Card[])array.Clone();
        }


    }
    }


  
