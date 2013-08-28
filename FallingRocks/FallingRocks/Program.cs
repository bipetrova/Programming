using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FallingRocks
{
    class FallingRocks
    {
        static int playerPadSize = 5;
        static int playfieldWidth = 40;
        static int livesCount = 10;
        static int pointsCount = 0;
        static string[] rocksSymbols = { "^", "@", "*", "++", "&", ";", "%", "$", "#", "!", ".." };
        static int indexRocksSymbols;

        struct Object
        {
            public int x;
            public int y;
            public string c;
            public ConsoleColor color;
        }

        static void RemoveScrollBars()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BufferHeight = Console.WindowHeight = 20;
            Console.BufferWidth = Console.WindowWidth = 60;
        }

        
        static void PrintOnPosition(int x, int y, string symbol, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(symbol);
        }


        static void Main()
        {
            RemoveScrollBars();

            Object dwarf = new Object();
            dwarf.x = playfieldWidth / 2 - playerPadSize / 2;
            dwarf.y = Console.WindowHeight - 1;
            dwarf.c = "(O)";
            dwarf.color = ConsoleColor.White;

            Random randomGenerator = new Random();
            List<Object> rocks = new List<Object>();

            string[] colorNames = ConsoleColor.GetNames(typeof(ConsoleColor));

            while (true)
            {

                bool hitted = false;
                indexRocksSymbols = randomGenerator.Next(0, rocksSymbols.Length);
                ConsoleColor color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorNames[randomGenerator.Next(0, colorNames.Length)]);

                Object newRock = new Object();
                newRock.color = color;
                newRock.c = rocksSymbols[indexRocksSymbols];
                newRock.x = randomGenerator.Next(0, playfieldWidth);
                newRock.y = 0;
                rocks.Add(newRock);

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                    while (Console.KeyAvailable) Console.ReadKey(true);

                    if (pressedKey.Key == ConsoleKey.LeftArrow)
                    {
                        if (dwarf.x - 1 >= 0)
                        {
                            dwarf.x = dwarf.x - 1;
                        }
                    }
                    else if (pressedKey.Key == ConsoleKey.RightArrow)
                    {
                        if (dwarf.x + 1 < playfieldWidth)
                        {
                            dwarf.x = dwarf.x + 1;
                        }
                    }
                }

                for (int i = 0; i < rocks.Count; i++)
                {
                    Object oldObject = rocks[i];
                    Object newObject = new Object();
                    newObject.x = oldObject.x;
                    newObject.y = oldObject.y + 1;
                    newObject.c = oldObject.c;
                    newObject.color = oldObject.color;
                    rocks.Remove(oldObject);

                    if (newObject.y == Console.WindowHeight)
                    {
                        pointsCount++;
                    }

                    if ((newObject.y == dwarf.y && newObject.x == dwarf.x)
                            || (newObject.y == dwarf.y && newObject.x == dwarf.x + 1)
                            || (newObject.y == dwarf.y && newObject.x == dwarf.x + 2))
                    {
                        livesCount--;
                        hitted = true;

                        if (livesCount <= 0)
                        {
                            PrintOnPosition(47, 5, "GAME OVER", ConsoleColor.Red);
                            Console.ReadLine();
                            Environment.Exit(0);
                        }
                    }

                    if (newObject.y < Console.WindowHeight)
                    {
                        rocks.Add(newObject);
                    }
                }

                Console.Clear();

                if (hitted)
                {
                    rocks.Clear();
                    PrintOnPosition(dwarf.x, dwarf.y, "XXX", ConsoleColor.Red);
                }
                 else
                {
                    PrintOnPosition(dwarf.x, dwarf.y, dwarf.c, dwarf.color);
                }


                foreach (Object rock in rocks)
                {
                    PrintOnPosition(rock.x, rock.y, rock.c, rock.color);
                }

                for (int i = 0; i < Console.WindowHeight; i++)
                {
                    PrintOnPosition(42, i, "|");
                }
                PrintOnPosition(44, 1, "*FALLING ROCKS*" , ConsoleColor.Magenta);
                PrintOnPosition(47, 3, "Score: " + pointsCount);
                PrintOnPosition(47, 5, "Lives: " + livesCount);
                Thread.Sleep(150);
            }
        }
    }
}