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


        public int CmdStart(GameRound round, object[] opt)
        {
            CardGame game = round.Current;
            Console.WriteLine(game.ToString());
            Console.WriteLine("1: Discard a card. (ex. \"> 1 R3 3[enter]\" - discard the card R3 to the field 3)");
            Console.WriteLine("2: Trash a card.   (ex. \"> 2 G9 3[enter]\" - trash the card G9 in the field 3)");
            Console.WriteLine("3: Turn end.       (ex. \"> 3[enter]\" - turn end)");
            Console.WriteLine();

            Console.Write(game.Players[game.Turn].Name + "> ");
            string line = Console.ReadLine();

            if (line.Length == 0)
            {
                line = AI(game);
            }

            string[] str = line.Split(' ');
            int mode;
            if (str.Length == 0 || !int.TryParse(str[0], out mode))
            {
                return -1;
            }

            if (mode < 3)
            {
                int field;
                if (str.Length < 3 || !int.TryParse(str[2], out field))
                {
                    Console.WriteLine("Error: Invalid command");
                    return -1;
                }
                opt[0] = str[1];
                opt[1] = field;
            }
            Console.WriteLine();
            return mode;
        }

        public void CmdEnd(GameRound round)
        {
        }

        private string AI(CardGame game)
        {
            string line = "";
            if (game.IsDiscarded)
            {
                line = "3";
            }
            else
            {
                if (!game.IsTrashed && GameRandom.Next() % 2 == 0)
                {
                    List<Field> fieldList = new List<Field>();
                    foreach (Field f in game.Fields)
                    {
                        if (f.CardList[game.Turn].Count() > 0)
                        {
                            fieldList.Add(f);
                        }
                    }
                    if (fieldList.Count() > 0)
                    {
                        Field field = fieldList[GameRandom.Next() % fieldList.Count()];
                        Card card = field.CardList[game.Turn][GameRandom.Next() % field.CardList[game.Turn].Count()];
                        line = "2 " + card.Name + " " + field.Number;
                    }
                }
                if (line.Length == 0)
                {
                    CardDeck deck = game.Players[game.Turn].Hand;
                    int index = GameRandom.Next() % deck.Count();
                    int field = GameRandom.Next() % 5;
                    line = "1 " + deck[index].Name + " " + field;
                }
            }
            Console.WriteLine("Auto: " + line);
            return line;
        }


        public void CmdError(GameRound round, GameException ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            Console.WriteLine();
        }
    }
}
