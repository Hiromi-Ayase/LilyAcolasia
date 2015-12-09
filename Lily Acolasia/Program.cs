using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lily_Acolasia
{
    class Program
    {
        static void Main(string[] args)
        {
            IGameObserver observer = new ConsoleGameObserver();
            GameOperator game = new GameOperator(observer, "PlayerA", "PlayerB");
            game.Start();
        }
    }

    /// <summary>
    /// Console game observer implementation.
    /// </summary>
    class ConsoleGameObserver : IGameObserver
    {
        private static readonly Dictionary<string, string> ErrorMessages = new Dictionary<string, string>() 
        {
            {GameExceptionType.FIELD_INDEX_EXCEPTION, "The field index is out of range."},
            {GameExceptionType.CARD_STRING_EXCEPTION, "The card representation string is invalid."},
            {GameExceptionType.ALREADY_DISCARDED_EXCEPTION, "You have already discarded a card."},
            {GameExceptionType.NOT_DISCARDED_EXCEPTION, "You have not discarded any card yet."},
            {GameExceptionType.ALREADY_TRASHED_EXCEPTION, "You have already trashed a card."},
            {GameExceptionType.CARD_ALREADY_FIXED_EXCEPTION, "The card list in the field is already fixed."},
            {GameExceptionType.CARD_NOT_FOUND_EXCEPTION, "The card is not found."},
            {GameExceptionType.CARD_TOO_MANY_EXCEPTION, "The number of cards is too many."},
            {GameExceptionType.NO_CARD_EXCEPTION, "No such card exists."},
        };

        public void RoundStart(GameRound round)
        {
            Console.WriteLine("Round" + round.Round + " start!");
            Console.WriteLine();
        }

        public void GameStart(GameRound round)
        {
            Console.WriteLine("Game start!");
        }

        public void GameEnd(GameRound round)
        {
            Console.WriteLine("Result: " + round.Point1 + ":" + round.Point2);
            Console.WriteLine("Press any key.");
            Console.ReadLine();

        }

        public void RoundEnd(GameRound round)
        {
            Console.WriteLine(round.Current.ToString());
            int winner = round.Current.Winner;
            if (winner == 2)
            {
                Console.WriteLine("Round" + round.Round + " Even.");
            }
            else
            {
                Console.WriteLine("Round" + round.Round + " " + round.Current.Players[winner].Name + " won!");
            }
            Console.WriteLine("Press any key.");
            Console.ReadLine();
        }

        public void TurnStart(GameRound round)
        {
            Console.WriteLine("============ Turn Start ============");
            Console.WriteLine();
        }

        public void TurnEnd(GameRound round)
        {
            Console.WriteLine("============ Turn End ============");
            Console.WriteLine();
        }


        public Command CmdStart(GameRound round, object[] opt)
        {
            CardGame game = round.Current;
            Console.WriteLine(game.ToString());
            Console.WriteLine("Status:" + game.Status.ToString());
            if (game.Status == GameStatus.Status.WaitSpecialInput) {
                int effectType = (game.LastTrashed.Power - 1) / 2;
                string[] messages = { "Trush my arbitrary card.", "Refresh my hand.", "Change the field.", "Trush enemy's arbitrary card.", "Get an extra turn." };
                Console.WriteLine("Card effect: " + messages[effectType]);
            }
            Console.WriteLine();

            switch (game.Status)
            {
                case GameStatus.Status.First:
                    Console.WriteLine("1: Discard a card. (ex. \"> 1 R3 3[enter]\" - discard the card R3 to the field 3)");
                    Console.WriteLine("2: Trash a card.   (ex. \"> 2 G9 3[enter]\" - trash the card G9 in the field 3)");
                    break;
                case GameStatus.Status.Trashed:
                case GameStatus.Status.Discarded:
                    Console.WriteLine("1: Discard a card. (ex. \"> 1 R3 3[enter]\" - discard the card R3 to the field 3)");
                    break;
                case GameStatus.Status.WaitSpecialInput:
                    if (game.LastTrashed.HasSpecialInput)
                    {
                        Console.WriteLine("3: Input the card effect param. (ex. \"> 3 P5 1[enter]\" - trash enemy's P5 in the field 1)");
                        break;
                    }
                    else
                    {
                        return Command.Special;
                    }
            }
            if (game.Status == GameStatus.Status.End || game.IsFilled)
            {
                Console.WriteLine("4: Turn end.       (ex. \"> 4[enter]\" - turn end)");
            }
            Console.WriteLine();

            Console.Write(game.Player.Name + "> ");
            string line = Console.ReadLine();

            if (line.Length == 0)
            {
                line = AI(game);
            }

            string[] str = line.Split(' ');
            int mode;
            if (str.Length == 0 || !int.TryParse(str[0], out mode))
            {
                return Command.Null;
            }

            if (mode < 4)
            {
                int field;
                if (str.Length < 3 || !int.TryParse(str[2], out field))
                {
                    Console.WriteLine("Error: Invalid command");
                    Console.WriteLine();
                    return Command.Null;
                }
                opt[0] = str[1];
                opt[1] = field;
            }
            Console.WriteLine();
            switch (mode)
            {
                case 1:
                    return Command.Discard;
                case 2:
                    return Command.Trash;
                case 3:
                    return Command.Special;
                case 4:
                    return Command.Next;
                default:
                    return Command.Null;
            }
        }

        public void CmdEnd(GameRound round)
        {
        }

        private string AI(CardGame game)
        {
            string line = "";
            if (game.Status == GameStatus.Status.End || game.IsFilled)
            {
                line = "4";
            }
            else
            {
                if (game.Status == GameStatus.Status.WaitSpecialInput)
                {
                    int turn = (game.Turn + (game.LastTrashed.Power == 1 ? 0 : 1)) % 2;
                    List<Field> fieldList = game.Fields.Where(f => f.CardList[turn].Count() > 0).ToList();
                    if (fieldList.Count() > 0)
                    {
                        Field field = fieldList[GameRandom.Next() % fieldList.Count()];
                        Card card = field.CardList[turn][GameRandom.Next() % field.CardList[turn].Count()];
                        line = "3 " + card.Name + " " + field.Number;
                    }
                }
                else if (game.Status == GameStatus.Status.First && GameRandom.Next() % 2 == 0)
                {
                    List<Field> fieldList = game.Fields.Where(f => f.CardList[game.Turn].Count() > 0).ToList();
                    if (fieldList.Count() > 0)
                    {
                        Field field = fieldList[GameRandom.Next() % fieldList.Count()];
                        Card card = field.CardList[game.Turn][GameRandom.Next() % field.CardList[game.Turn].Count()];
                        line = "2 " + card.Name + " " + field.Number;
                    }
                }
                if (line.Length == 0)
                {
                    List<int> fieldList = game.Fields.Where(f => f.CardList[game.Turn].Count() < 3).Select(f => f.Number).ToList();
                    CardDeck deck = game.Player.Hand;
                    int index = GameRandom.Next() % deck.Count();
                    int field = GameRandom.Next() % fieldList.Count();
                    line = "1 " + deck[index].Name + " " + fieldList[field];
                }
            }
            Console.WriteLine("Auto: " + line);
            return line;
        }


        public void CmdError(GameRound round, GameException ex)
        {

            Console.WriteLine("Error: " + ErrorMessages[ex.Type]);
            Console.WriteLine();
        }
    }
}
