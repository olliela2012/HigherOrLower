const suits = [
  { name: "Hearts", symbol: "♥", red: true },
  { name: "Diamonds", symbol: "♦", red: true },
  { name: "Clubs", symbol: "♣", red: false },
  { name: "Spades", symbol: "♠", red: false }
];
const ranks = ["", "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"];
const currentCardEl = document.querySelector("#current-card");
const nextCardEl = document.querySelector("#next-card");
const higherButton = document.querySelector("#higher-button");
const lowerButton = document.querySelector("#lower-button");
const restartButton = document.querySelector("#restart-button");
let deck;
let currentCard;
let score;
let active;

function newDeck() {
  const cards = suits.flatMap((suit, suitIndex) =>
    Array.from({ length: 13 }, (_, index) => ({ value: index + 1, rank: ranks[index + 1], suit, suitIndex }))
  );
  for (let i = cards.length - 1; i > 0; i--) {
    const j = Math.floor(Math.random() * (i + 1));
    [cards[i], cards[j]] = [cards[j], cards[i]];
  }
  return cards;
}

function showCard(element, card) {
  const colorClass = card.suit.red ? "red-suit" : "black-suit";
  element.className = "playing-card deal";
  element.setAttribute("aria-label", `${card.rank} of ${card.suit.name}`);
  element.innerHTML = `
    <span class="card-corner corner-top ${colorClass}"><span class="rank">${card.rank}</span><span class="suit">${card.suit.symbol}</span></span>
    <span class="card-center ${colorClass}">${card.suit.symbol}</span>
    <span class="card-corner corner-bottom ${colorClass}"><span class="rank">${card.rank}</span><span class="suit">${card.suit.symbol}</span></span>`;
  window.setTimeout(() => element.classList.remove("deal"), 400);
}

function setMessage(text, tone = "") {
  document.querySelector("#message").textContent = text;
  document.querySelector("#message-indicator").textContent = tone === "good" ? "✓" : tone === "bad" ? "×" : "›";
  document.querySelector("#message-indicator").style.color = tone === "bad" ? "var(--red)" : "var(--green)";
}

function updateScore() {
  document.querySelector("#score").textContent = String(score).padStart(2, "0");
  const best = Math.max(score, Number(localStorage.getItem("higherLowerBest") || 0));
  localStorage.setItem("higherLowerBest", String(best));
  document.querySelector("#best").textContent = String(best).padStart(2, "0");
}

function renderDeckCount() {
  const remaining = deck.length;
  document.querySelector("#deck-count").textContent = `${remaining} ${remaining === 1 ? "CARD" : "CARDS"} LEFT`;
}

function hideNextCard() {
  nextCardEl.className = "playing-card card-back";
  nextCardEl.setAttribute("aria-label", "Next card hidden");
  nextCardEl.innerHTML = '<div class="back-pattern"><span>H / L</span></div>';
}

function startGame() {
  deck = newDeck();
  currentCard = deck.pop();
  score = 0;
  active = true;
  showCard(currentCardEl, currentCard);
  hideNextCard();
  document.querySelector("#current-caption").textContent = "FIRST CARD";
  document.querySelector("#next-caption").textContent = "YOUR GUESS";
  higherButton.disabled = false;
  lowerButton.disabled = false;
  updateScore();
  renderDeckCount();
  setMessage("Pick a side. Trust your instinct.");
}

function guess(direction) {
  if (!active) return;
  const nextCard = deck.pop();
  if (!nextCard) {
    active = false;
    higherButton.disabled = true;
    lowerButton.disabled = true;
    setMessage(`Deck's empty! Final streak: ${score}.`, "good");
    document.querySelector("#next-caption").textContent = "DECK EMPTY";
    return;
  }

  showCard(nextCardEl, nextCard);
  document.querySelector("#next-caption").textContent = `${nextCard.rank} OF ${nextCard.suit.name.toUpperCase()}`;
  renderDeckCount();
  const difference = nextCard.value - currentCard.value;
  const correct = difference === 0 || (direction === "higher" && difference > 0) || (direction === "lower" && difference < 0);

  if (correct) {
    score++;
    currentCard = nextCard;
    showCard(currentCardEl, currentCard);
    hideNextCard();
    document.querySelector("#current-caption").textContent = "NOW PLAYING";
    document.querySelector("#next-caption").textContent = "YOUR GUESS";
    updateScore();
    setMessage(difference === 0 ? "It's a tie! You're safe. Keep it going." : "Nice call. The run continues.", "good");
    if (deck.length === 0) {
      active = false;
      higherButton.disabled = true;
      lowerButton.disabled = true;
      setMessage(`Perfect run! Deck's empty. Final streak: ${score}.`, "good");
      document.querySelector("#next-caption").textContent = "DECK EMPTY";
    }
  } else {
    active = false;
    higherButton.disabled = true;
    lowerButton.disabled = true;
    setMessage(`${direction === "higher" ? "Higher" : "Lower"} was the call. Final streak: ${score}. Try again?`, "bad");
  }
}

higherButton.addEventListener("click", () => guess("higher"));
lowerButton.addEventListener("click", () => guess("lower"));
restartButton.addEventListener("click", startGame);
document.addEventListener("keydown", event => {
  if (event.repeat || event.altKey || event.ctrlKey || event.metaKey) return;
  if (event.key.toLowerCase() === "h") guess("higher");
  if (event.key.toLowerCase() === "l") guess("lower");
});

document.querySelector("#best").textContent = String(Number(localStorage.getItem("higherLowerBest") || 0)).padStart(2, "0");
startGame();

