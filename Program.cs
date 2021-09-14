﻿using System;
using System.Threading;

namespace SnakeGame
{

    public class Program
    {
        private Snake snake;         /* the snake body */
        private int grow;            /* will the snake grow this round? (>0)  */
        private ConsoleColor orig;   /* save off the original to restore */
        private int Speed = 140;     /* how fast? (lower = faster) */
        private int SpFactor = 0;    /* adjustment for vertical movement */
        private Apples apples;       /* the apple for the snake */

        private void InitConsole()
        {
            Console.Clear();
            Console.CursorVisible = false;
            orig = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write("Stay in bounds, eat apples (a), don't cross yourself.");
            while (Console.CursorLeft < (Console.WindowWidth - 1))
            {
                Console.Write('~');
            }
        }

        private void ResetConsole()
        {
            while(Console.KeyAvailable) { Console.ReadKey(true); } // flush the input buffer
            Console.CursorVisible = true;
            Console.ForegroundColor = orig;
        }

        public Program()
        {
            snake = new Snake(new Location() { X = 10, Y = 10 });
            apples = new Apples();
            apples.GrowNew(Console.WindowWidth,
                Console.WindowHeight - 1,
                snake.Collision);
            grow = 1;  /* grow the first round */
        }

        public void CheckUserInput()
        {
            if(Console.KeyAvailable)
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.UpArrow:
                    snake.ChangeDirection(0, -1);
                    SpFactor = Speed / 2;
                    break;
                case ConsoleKey.DownArrow:
                    snake.ChangeDirection(0, 1);
                    SpFactor = Speed / 2;
                    break;
                case ConsoleKey.LeftArrow:
                    snake.ChangeDirection(-1, 0);
                    SpFactor = 0;
                    break;
                case ConsoleKey.RightArrow:
                    snake.ChangeDirection(1, 0);
                    SpFactor = 0;
                    break;
                case ConsoleKey.Q:
                    ResetConsole();
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }

        private void DrawApple()
        {
            Location aloc = apples.Current;
            Console.SetCursorPosition(aloc.X, aloc.Y);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write('a');
        }

        private void DrawUpdates(MovementResult mr)
        {
            if (mr.MovedTail.Y != 255)
            {
                // 255 signals that we grew... it's hacky but deliciously so
                Console.SetCursorPosition(mr.MovedTail.X, mr.MovedTail.Y);
                Console.Write(' ');
            }
            Console.SetCursorPosition(mr.NewHead.X, mr.NewHead.Y);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write('@');
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(mr.OldHead.X, mr.OldHead.Y);
            Console.Write('#');
            Console.SetCursorPosition(Console.WindowLeft,
                Console.WindowTop + Console.WindowHeight - 1);
        }

        public void RunGame()
        {
            // write the initial location
            Console.SetCursorPosition(10, 10);
            Console.Write('@');
            DrawApple();

            var mr = new MovementResult();

            while (true)
            {
                System.Threading.Thread.Sleep(Speed + SpFactor);
                CheckUserInput();

                snake.Move(ref mr, grow > 0);

                if (OutOfBounds() || snake.SelfCollision()) break;

                DrawUpdates(mr);

                if (apples.Eaten(mr.NewHead))
                {
                    grow = 3;
                    apples.GrowNew(Console.WindowWidth, Console.WindowHeight - 1, snake.Collision);
                    DrawApple();
                    if (Speed > 10) { Speed -= 10; }
                }
                else
                {
                    if (grow > 0) --grow;
                }
            }

            bool OutOfBounds() =>
                (mr.NewHead.X >= Console.WindowWidth) ||
                (mr.NewHead.Y >= Console.WindowHeight - 1);
        }

        public static void Main(string[] args)
        {
            var game = new Program();
            game.InitConsole();
            try
            {
                game.RunGame();
            }
            finally
            {
                game.ResetConsole();
                Console.WriteLine("\n\nYou died.  You had eaten {0} apples.", game.apples.Count);
            }
        }
    }

} // end namespace
