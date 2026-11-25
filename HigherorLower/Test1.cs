using HigherOrLowerLogic;

namespace HigherorLower
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var x = new Card(7, Suit.Hearts);
            var x2 = new Card(7, Suit.Hearts);
            var x3 = new Card(8, Suit.Hearts);
            Assert.AreEqual(x,x2);
            Assert.IsTrue(x3>x);
        }


        [TestMethod(DisplayName ="Check all cards are different in a new shuffled deck")]
        public void TestMethod2()
        {
            var deck = new Deck();
            var cards = deck.GetAllCards();
            var uniqueCards = new HashSet<Card>(cards);
            Assert.AreEqual(cards.Length, uniqueCards.Count);
        }
    }
}
