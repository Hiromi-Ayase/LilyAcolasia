using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Lily_Acolasia
{
    /// <summary>
    /// Random class for the Game.
    /// </summary>
    class GameRandom
    {
        private static Random rand = new Random();

        /// <summary>
        /// Get the next color.
        /// </summary>
        /// <returns></returns>
        public static int Next()
        {
            return rand.Next();
        }
    }

    /// <summary>
    /// Color class.
    /// </summary>
    class Color
    {
        /// <summary>
        /// Red.
        /// </summary>
        public static readonly Color Red = new Color("R");
        /// <summary>
        /// Green.
        /// </summary>
        public static readonly Color Green = new Color("G");
        /// <summary>
        /// Blue.
        /// </summary>
        public static readonly Color Blue = new Color("B");
        /// <summary>
        /// Yellow.
        /// </summary>
        public static readonly Color Yellow = new Color("Y");
        /// <summary>
        /// Purple.
        /// </summary>
        public static readonly Color Purple = new Color("P");
        /// <summary>
        /// All colors list.
        /// </summary>
        public static readonly Color[] List = { Red, Green, Blue, Yellow, Purple };

        private readonly string name;
        private Color(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Get the string representation.
        /// </summary>
        /// <returns>Color string representation.</returns>
        public override string ToString()
        {
            return this.name;
        }

        /// <summary>
        /// Get the color instance from the color character.
        /// </summary>
        /// <param name="c">Character of color.</param>
        /// <returns>Color instance.</returns>
        public static Color Get(char c)
        {
            switch (c)
            {
                case 'R':
                    return Red;
                case 'G':
                    return Green;
                case 'B':
                    return Blue;
                case 'Y':
                    return Yellow;
                case 'P':
                    return Purple;
                default:
                    return null;
            }
        }
    }

    /// <summary>
    /// Card class.
    /// </summary>
    class Card : IComparable<Card>
    {
        private static readonly Regex regex;
        private readonly int power;
        private readonly Color color;
        private readonly string name;

        /// <summary>
        /// Card power.
        /// </summary>
        public int Power { get { return this.power; } }
        /// <summary>
        /// Card color.
        /// </summary>
        public Color Color { get { return this.color; } }
        /// <summary>
        /// Card name.
        /// </summary>
        public string Name { get { return this.name; } }

        static Card()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Color color in Color.List)
            {
                sb.Append(color.ToString());
            }
            regex = new Regex("[" + sb.ToString() + "][1-9]");
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="power">Power.</param>
        /// <param name="color">Color.</param>
        public Card(int power, Color color)
        {
            this.power = power;
            this.color = color;
            this.name = String.Format("{0}{1}", color.ToString(), power);
        }

        /// <summary>
        /// The special skill of this card.
        /// </summary>
        /// <param name="game">Game instance.</param>
        /// <returns>Result.</returns>
        public string Special(CardGame game)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// String representation for the card.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString()
        {
            return String.Format("[{0}]", this.name);
        }

        /// <summary>
        /// Judge if the card string equals to this card.
        /// </summary>
        /// <param name="cardStr">The card name.</param>
        /// <returns>True: same  False: different.</returns>
        public bool IsMatch(string cardStr)
        {
            if (!regex.IsMatch(cardStr))
            {
                throw GameException.getCardStrException(cardStr);
            }
            return this.name == cardStr;
        }

        /// <summary>
        /// Comparator.
        /// </summary>
        /// <param name="other">Other card instance.</param>
        /// <returns>Comparison result.</returns>
        public int CompareTo(Card other)
        {
            char c1 = this.color.ToString()[0];
            char c2 = other.color.ToString()[0];
            if (c1 == c2)
            {
                return this.power - other.power;
            }
            else
            {
                return c1 - c2;
            }
        }
    }

    /// <summary>
    /// Player class.
    /// </summary>
    class Player
    {
        private readonly CardDeck hand;
        private readonly String name;

        /// <summary>
        /// Player's hand.
        /// </summary>
        public CardDeck Hand { get { return this.hand; } }
        /// <summary>
        /// Player's name.
        /// </summary>
        public string Name { get { return this.name; } }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Player's name.</param>
        public Player(string name)
        {
            this.name = name;
            this.hand = new CardDeck();
        }

        /// <summary>
        /// Initialize the hand.
        /// </summary>
        public void Init()
        {
            this.hand.Init();
        }

        /// <summary>
        /// Add the card to the hand.
        /// </summary>
        /// <param name="card">The card to be added.</param>
        public void Add(Card card)
        {
            this.hand.Add(card);
        }

        /// <summary>
        /// String represantation for this player.
        /// </summary>
        /// <returns>String represantation.</returns>
        public override string ToString()
        {
            return String.Format("{0}: {1}", name, this.hand.ToString());
        }
    }

    /// <summary>
    /// Card deck class.
    /// </summary>
    class CardDeck : IEnumerable<Card>
    {
        private readonly int size;
        private List<Card> list = new List<Card>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="size">Size.</param>
        public CardDeck(int size)
        {
            this.size = size;
        }

        /// <summary>
        /// Constructor for infinite size deck.
        /// </summary>
        public CardDeck()
        {
            this.size = -1;
        }

        /// <summary>
        /// Initialize the deck.
        /// </summary>
        public void Init()
        {
            this.list.Clear();
        }

        /// <summary>
        /// Get the card by index.
        /// </summary>
        /// <param name="i">Index.</param>
        /// <returns>Card instance.</returns>
        public Card this[int i]
        {
            get { return this.list[i]; }
        }

        /// <summary>
        /// Draw the first card.
        /// </summary>
        /// <returns></returns>
        public Card Draw()
        {
            if (this.list.Count() == 0)
            {
                throw GameException.getNoCardException();
            }
            Card card = this.list.First();
            if (!this.list.Remove(card))
            {
                throw new Exception();
            }
            return card;
        }

        /// <summary>
        /// Get the card by string representation.
        /// </summary>
        /// <param name="cardStr">String representation.</param>
        /// <returns>Card instance.</returns>
        public Card Get(string cardStr)
        {
            foreach (Card card in this.list)
            {
                if (card.IsMatch(cardStr))
                {
                    return card;
                }
            }
            return null;
        }

        /// <summary>
        /// Remove the card by string representation.
        /// </summary>
        /// <param name="cardStr">String representation.</param>
        /// <returns>Removed card instance.</returns>
        public Card Remove(string cardStr)
        {
            Card card = this.Get(cardStr);
            if (card != null)
            {
                this.list.Remove(card);
            }
            else
            {
                throw GameException.getCardNotFoundException(cardStr);
            }
            return card;
        }

        /// <summary>
        /// Add the card to the deck.
        /// </summary>
        /// <param name="card">Card instance.</param>
        public void Add(Card card)
        {
            if (size >= 0 && list.Count() == size)
            {
                throw GameException.getCardTooManyException();
            }
            this.list.Add(card);
        }

        /// <summary>
        /// Add another deck's cards.
        /// </summary>
        /// <param name="deck">Other deck.</param>
        public void AddRange(CardDeck deck)
        {
            this.list.AddRange(deck.list);
        }

        /// <summary>
        /// Shuffle the card.
        /// </summary>
        public void Shuffle()
        {
            this.list = this.list.OrderBy(i => GameRandom.Next()).ToList();
        }

        /// <summary>
        /// String represantation for this deck.
        /// </summary>
        /// <returns>String represantation.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Math.Max(this.size, this.list.Count()); i++)
            {
                if (i < this.list.Count())
                {
                    sb.Append(this.list[i].ToString());
                }
                else
                {
                    sb.Append("    ");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// IEnumerable implementation.
        /// </summary>
        /// <returns>IEnumerable instance.</returns>
        public IEnumerator<Card> GetEnumerator()
        {
            foreach (Card card in this.list)
            {
                yield return card;
            }
        }

        /// <summary>
        /// IEnumerable implementation.
        /// </summary>
        /// <returns>IEnumerable instance.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// Talon class.
    /// </summary>
    class Talon
    {
        private CardDeck deck = new CardDeck();
        private CardDeck trash = new CardDeck();

        /// <summary>
        /// Initialize the talon.
        /// </summary>
        public void Init()
        {
            deck.Init();
            foreach (Color color in Color.List)
            {
                for (int i = 1; i <= 9; i++)
                {
                    deck.Add(new Card(i, color));
                }
            }
            deck.Shuffle();
        }

        /// <summary>
        /// Refresh the talon.
        /// </summary>
        public void Refresh()
        {
            deck.AddRange(trash);
            trash.Init();
            deck.Shuffle();
        }

        /// <summary>
        /// Draw the card from this talon.
        /// </summary>
        /// <returns>Card instance.</returns>
        public Card Draw()
        {
            return deck.Draw();
        }

        /// <summary>
        /// Trash the card.
        /// </summary>
        /// <param name="card">Trashed card.</param>
        public void Trash(Card card)
        {
            trash.Add(card);
        }

        /// <summary>
        /// String represantation for this deck.
        /// </summary>
        /// <returns>String represantation.</returns>
        public override string ToString()
        {
            return String.Format("Talon: {0}", deck.ToString());
        }

        /// <summary>
        /// True: empty deck  False: other.
        /// </summary>
        public bool IsEmpty
        {
            get { return this.deck.Count() == 0; }
        }
    }

    /// <summary>
    /// Field class.
    /// </summary>
    class Field
    {
        private const int MAX_DISCARD = 3;
        private const int PLAYER_NUM = 2;

        private const int SCORE_FIELD_MATCH = 2;
        private const int SCORE_SAME_COLOR = 3;
        private const int SCORE_SAME_NUMBER = 3;

        private readonly Color color;
        private readonly int number;
        private readonly CardDeck[] cardList = new CardDeck[PLAYER_NUM];
        private int win = -1;

        /// <summary>
        /// Card list.
        /// </summary>
        public CardDeck[] CardList { get { return this.cardList; } }
        /// <summary>
        /// Winner(-1:Not fixed  0:Player1 won  1:Player2 won  2:Even).
        /// </summary>
        public int Winner { get { return this.win; } }
        /// <summary>
        /// Field number.
        /// </summary>
        public int Number { get { return this.number; } }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="number">Field number.</param>
        public Field(int number)
        {
            this.number = number;
            this.color = Color.List[GameRandom.Next() % Color.List.Length];
            cardList[0] = new CardDeck(MAX_DISCARD);
            cardList[1] = new CardDeck(MAX_DISCARD);
        }

        /// <summary>
        /// Get the card list.
        /// </summary>
        /// <param name="turn">Turn number.</param>
        /// <returns>CardDeck instance.</returns>
        public CardDeck GetCardList(int turn)
        {
            return this.CardList[turn];
        }

        /// <summary>
        /// Remove the card from the field.
        /// </summary>
        /// <param name="turn">Turn number.</param>
        /// <param name="cardStr">String representation of the card.</param>
        /// <returns>Card instance.</returns>
        public Card Remove(int turn, string cardStr)
        {
            if (this.win >= 0)
            {
                throw GameException.getCardAlreadyFixedException();
            }
            return cardList[turn].Remove(cardStr);
        }

        /// <summary>
        /// Add the card to the field.
        /// </summary>
        /// <param name="turn">Turn number.</param>
        /// <param name="cardStr">String representation of the card.</param>
        public void Add(int turn, Card card)
        {
            if (this.win >= 0)
            {
                throw GameException.getCardAlreadyFixedException();
            }
            else
            {
                cardList[turn].Add(card);
            }
        }

        /// <summary>
        /// Get the score.
        /// </summary>
        /// <param name="turn">Turn number.</param>
        /// <returns>Score.</returns>
        public int getScore(int turn)
        {
            int score = 0;
            CardDeck list = this.CardList[turn];

            foreach (Card card in this.CardList[turn])
            {
                score += card.Power;
                if (card.Color == this.color)
                {
                    score += SCORE_FIELD_MATCH;
                }
            }

            if (list.Count() == MAX_DISCARD)
            {
                Color color = list[0].Color;
                int power = list[0].Power;
                for (int i = 0; i < MAX_DISCARD; i++)
                {
                    if (color != list[i].Color)
                    {
                        color = null;
                    }
                    if (power != list[i].Power)
                    {
                        power = -1;
                    }
                }
                if (color != null)
                {
                    score += SCORE_SAME_COLOR;
                }
                if (power >= 0)
                {
                    score += SCORE_SAME_NUMBER;
                }
            }
            return score;
        }

        /// <summary>
        /// Evaluate this field.
        /// If there are 3 cards in the both side, the field will be fixed.
        /// </summary>
        /// <returns>-1:Not fixed, 0:Player1 won, 1:Player2 won, 2:Even</returns>
        public int Eval()
        {
            if (Math.Min(this.cardList[0].Count(), this.cardList[1].Count()) < MAX_DISCARD)
            {
                return this.win;
            }

            if (this.win < 0)
            {
                int score1 = getScore(1);
                int score0 = getScore(0);

                if (score1 == score0)
                {
                    this.win = 2;
                }
                else if (score1 > score0)
                {
                    this.win = 1;
                }
                else
                {
                    this.win = 0;
                }
            }
            return this.win;
        }

        /// <summary>
        /// Get the string representation of the Field.
        /// </summary>
        /// <param name="before">The before field for cascade.</param>
        /// <returns>String representation.</returns>
        public string GetString(string before)
        {
            if (before == null)
            {
                before = "\n\n\n\n\n";
            }
            string[] lines = before.Split('\n');

            StringBuilder sb1 = new StringBuilder();
            sb1.Append(" ");
            for (int i = 0; i < MAX_DISCARD; i++)
            {
                if (i < cardList[0].Count())
                {
                    sb1.Append(cardList[0][i].ToString());
                }
                else
                {
                    sb1.Append("    ");
                }
            }
            sb1.Append("  ");
            lines[0] += sb1.ToString();

            string winner;
            switch (win)
            {
                case 0:
                    winner = "0";
                    break;
                case 1:
                    winner = "1";
                    break;
                case PLAYER_NUM:
                    winner = "E";
                    break;
                default:
                    winner = "-";
                    break;
            }

            lines[1] += "-------------- ";
            // R: 02 vs 03 (-) 
            lines[2] += String.Format("[{0}] {1}:{2, 2}v{3,-2}({4}) ", number, color.ToString(), getScore(0), getScore(1), winner);
            lines[3] += "-------------- ";

            StringBuilder sb3 = new StringBuilder();
            sb3.Append(" ");
            for (int i = 0; i < MAX_DISCARD; i++)
            {
                if (i < cardList[1].Count())
                {
                    sb3.Append(cardList[1][i].ToString());
                }
                else
                {
                    sb3.Append("    ");
                }
            }
            sb3.Append("  ");
            lines[4] += sb3.ToString();
            return String.Join("\n", lines);
        }
    }

    /// <summary>
    /// Card game class.
    /// </summary>
    class CardGame
    {
        private const int PLAYER_NUM = 2;
        private const int FIELD_NUM = 5;
        private const int INIT_CARD = 5;
        private const int DISCARD_NUM = 2;
        private const int TRASHED_NUM = 1;

        private readonly Field[] fields = new Field[FIELD_NUM];
        private readonly Player[] players = new Player[PLAYER_NUM];
        private readonly Talon talon;

        private int trashed;
        private int discarded;
        private int turn;
        private int firstTurn;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="playerA">Player1 name</param>
        /// <param name="playerB">Player2 name</param>
        public CardGame(string playerA, string playerB)
        {
            this.players[0] = new Player(playerA);
            this.players[1] = new Player(playerB);

            talon = new Talon();
            init();
        }

        /// <summary>
        /// Initialize the game.
        /// </summary>
        public void init()
        {
            this.turn = GameRandom.Next() % PLAYER_NUM;
            this.firstTurn = this.turn;
            for (int i = 0; i < FIELD_NUM; i++)
            {
                this.fields[i] = new Field(i);
            }
            talon.Init();
            this.players[0].Init();
            this.players[1].Init();

            for (int i = 0; i < INIT_CARD; i++)
            {
                Card card1 = this.talon.Draw();
                Card card2 = this.talon.Draw();
                this.players[0].Add(card1);
                this.players[1].Add(card2);
            }
        }

        /// <summary>
        /// Proceed to the next turn.
        /// </summary>
        public void nextTurn()
        {
            if (this.Winner >= 0)
            {
                return;
            }

            if (!this.IsDiscarded)
            {
                throw GameException.getNotTrashedException();
            }
            this.discarded = 0;
            this.trashed = 0;

            turn = (turn + 1) % PLAYER_NUM;

            foreach (Field field in this.fields)
            {
                field.Eval();
            }
            this.Draw();
            this.Draw();
        }

        /// <summary>
        /// Talon.
        /// </summary>
        public Talon Talon { get { return this.talon; } }
        /// <summary>
        /// Players.
        /// </summary>
        public Player[] Players { get { return this.players; } }
        /// <summary>
        /// Fileds.
        /// </summary>
        public Field[] Fields { get { return this.fields; } }
        /// <summary>
        /// Turn.
        /// </summary>
        public int Turn { get { return this.turn; } }
        /// <summary>
        /// Discarded flag.
        /// </summary>
        public bool IsDiscarded { get { return this.discarded == DISCARD_NUM; } }
        /// <summary>
        /// Trashed flag.
        /// </summary>
        public bool IsTrashed { get { return this.trashed == TRASHED_NUM; } }

        /// <summary>
        /// Winner.
        /// </summary>
        public int Winner
        {
            get
            {
                int before = -1;
                int[] evals = new int[2];
                int fixedField = 0;
                for (int i = 0; i < FIELD_NUM; i++)
                {
                    int eval = this.fields[i].Winner;
                    if (eval >= 0 && eval < PLAYER_NUM)
                    {
                        if (eval == before)
                        {
                            return eval;
                        }

                        evals[eval]++;
                        if (evals[eval] > Math.Floor((double)FIELD_NUM / 2 + 1))
                        {
                            return eval;
                        }
                    }
                    before = eval;

                    if (eval >= 0)
                    {
                        fixedField++;
                    }
                }

                if (fixedField == FIELD_NUM || this.talon.IsEmpty && Math.Max(this.players[0].Hand.Count(), this.players[1].Hand.Count()) == 0)
                {
                    if (evals[0] == evals[1])
                    {
                        return PLAYER_NUM;
                    }
                    else
                    {
                        return evals[0] > evals[1] ? 0 : 1;
                    }
                }

                return -1;
            }
        }

        /// <summary>
        /// Draw the card from the talon.
        /// </summary>
        public void Draw()
        {
            if (this.talon.IsEmpty)
            {
                this.talon.Refresh();
            }
            Card card = this.talon.Draw();
            this.players[this.turn].Add(card);
        }

        /// <summary>
        /// Discard the card to the field.
        /// </summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="cardStr">Card string.</param>
        /// <returns>Discarded card instance.</returns>
        public Card Discard(int fieldIndex, string cardStr)
        {
            if (this.IsDiscarded)
            {
                throw GameException.getAlreadyDiscardedException();
            }

            if (fieldIndex >= FIELD_NUM || fieldIndex < 0)
            {
                throw GameException.getFieldIndexException();
            }

            CardDeck hand = this.players[turn].Hand;
            Card card = hand.Get(cardStr);
            if (card == null) {
                throw GameException.getCardNotFoundException(cardStr);
            }
            this.fields[fieldIndex].Add(this.turn, card);
            hand.Remove(cardStr);

            this.discarded ++;
            this.trashed = TRASHED_NUM;
            return card;
        }

        /// <summary>
        /// Trash the card from the field.
        /// </summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="cardStr">Card string.</param>
        /// <returns>Trashed card instance.</returns>
        public Card Trash(int fieldIndex, string cardStr)
        {
            if (this.IsTrashed)
            {
                throw GameException.getAlreadyTrashedException();
            }
            if (fieldIndex >= FIELD_NUM || fieldIndex < 0)
            {
                throw GameException.getFieldIndexException();
            }


            Card card = this.fields[fieldIndex].Remove(this.turn, cardStr);
            this.talon.Trash(card);
            this.trashed ++;
            return card;
        }

        /// <summary>
        /// String representation for this game.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(this.talon.ToString());
            sb.AppendLine();
            sb.AppendLine();

            string fieldStr = null;
            foreach (Field field in this.fields)
            {
                fieldStr = field.GetString(fieldStr);
            }
            sb.Append(fieldStr);
            sb.AppendLine();

            sb.Append(this.turn == 0 ? "* " : "  ");
            sb.Append(this.players[0].ToString());
            sb.AppendLine();

            sb.Append(this.turn == 1 ? "* " : "  ");
            sb.Append(this.players[1].ToString());
            sb.AppendLine();

            return sb.ToString();
        }
    }

    /// <summary>
    /// Game round class.
    /// </summary>
    class GameRound
    {
        private readonly CardGame[] game;
        private int point1 = 0;
        private int point2 = 0;
        private int round = 0;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="n">Round number.</param>
        /// <param name="name1">Player1 name.</param>
        /// <param name="name2">Player2 name.</param>
        public GameRound(int n, string name1, string name2)
        {
            this.game = new CardGame[n];
            for (int i = 0; i < n; i++)
            {
                this.game[i] = new CardGame(name1, name2);
            }
        }

        /// <summary>
        /// Player1's point.
        /// </summary>
        public int Point1 { get { return this.point1; } }
        /// <summary>
        /// Player2's point.
        /// </summary>
        public int Point2 { get { return this.point2; } }
        /// <summary>
        /// Current round.
        /// </summary>
        public int Round { get { return this.round; } }
        /// <summary>
        /// Current game.
        /// </summary>
        public CardGame Current { get { return this.game[round]; } }

        /// <summary>
        /// Return if the round has a next turn or not.
        /// </summary>
        /// <returns>True: yes  False: no</returns>
        public bool HasNext()
        {
            int winner = this.Current.Winner;
            if (this.Current.Winner >= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Return if a next round exists or not.
        /// </summary>
        /// <returns>True: yes  False: all games are finished.</returns>
        public bool NextRound()
        {
            int winner = this.Current.Winner;
            if (winner == 1)
            {
                this.point2++;
            }
            else if (winner == 0)
            {
                this.point1++;
            }
            this.round++;
            return this.round < game.Length;
        }
    }

    /// <summary>
    /// Game observer interface.
    /// </summary>
    interface IGameObserver
    {
        /// <summary>
        /// Called when the game is started.
        /// </summary>
        /// <param name="round">Game round instance.</param>
        void GameStart(GameRound round);
        /// <summary>
        /// Called when the game is finished.
        /// </summary>
        /// <param name="round">Game round instance.</param>
        void GameEnd(GameRound round);
        /// <summary>
        /// Called when the round is started.
        /// </summary>
        /// <param name="round">Game round instance.</param>
        void RoundStart(GameRound round);
        /// <summary>
        /// Called when the round is finished.
        /// </summary>
        /// <param name="round">Game round instance.</param>
        void RoundEnd(GameRound round);
        /// <summary>
        /// Called when the turn is started.
        /// </summary>
        /// <param name="round">Game round instance.</param>
        void TurnStart(GameRound round);
        /// <summary>
        /// Called when the turn is finished.
        /// </summary>
        /// <param name="round">Game round instance.</param>
        void TurnEnd(GameRound round);
        /// <summary>
        /// Called when the command input is started.
        /// </summary>
        /// <param name="round">Game round instance.</param>
        /// <param name="opt">Output options.</param>
        /// <returns>Command.</returns>
        int CmdStart(GameRound round, object[] opt);
        /// <summary>
        /// Called when the command is finished.
        /// </summary>
        /// <param name="round">Game round instance.</param>
        void CmdEnd(GameRound round);
        /// <summary>
        /// Called when some exception happens.
        /// </summary>
        /// <param name="round">Game round instance.</param>
        /// <param name="ex">GameException instance.</param>
        void CmdError(GameRound round, GameException ex);
    }

    /// <summary>
    /// Game operator class.
    /// </summary>
    class GameOperator
    {
        private const int ROUND = 3;
        private GameRound round;
        private IGameObserver observer;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name1">Player1 name.</param>
        /// <param name="name2">Player2 name.</param>
        /// <param name="observer">Observer.</param>
        public GameOperator(IGameObserver observer, string name1, string name2)
        {
            this.round = new GameRound(ROUND, name1, name2);
            this.observer = observer;
        }

        /// <summary>
        /// Start the game.
        /// </summary>
        public void Start()
        {
            observer.GameStart(round);
            while (true)
            {
                observer.RoundStart(round);
                while (round.HasNext())
                {
                    Turn();
                }
                observer.RoundEnd(round);

                if (!round.NextRound())
                {
                    break;
                }
            }
            observer.GameEnd(round);
        }

        private void Turn()
        {
            CardGame game = round.Current;
            observer.TurnStart(round);

            object[] opt = new object[10];
            while (true)
            {
                int mode = observer.CmdStart(round, opt);
                try
                {
                    if (mode < 3)
                    {
                        if (mode == 1)
                        {
                            string cardStr = (string)opt[0];
                            int field = (int)opt[1];
                            game.Discard(field, cardStr);
                        }
                        else if (mode == 2)
                        {
                            string cardStr = (string)opt[0];
                            int field = (int)opt[1];
                            game.Trash(field, cardStr);
                        }
                        observer.CmdEnd(round);
                    }
                    else if (mode == 3)
                    {
                        observer.TurnEnd(round);
                        game.nextTurn();
                        break;
                    }
                }
                catch (GameException ex)
                {
                    observer.CmdError(round, ex);
                }
            }
        }
    }
}
