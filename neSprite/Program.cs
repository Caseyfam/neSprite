using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace neSprite
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter location of rom: ");
            string fileDirectory = Console.ReadLine();
            ReadFile(fileDirectory);
            Console.ReadLine();
        }

        public static void ReadFile(string fileDirectory)
        {
            int hexIn;
            String hex = "";
            int counter = 0;
            string[] left8 = new string[8];
            string[] right8 = new string[8];

            try
            {
                FileStream fs = new FileStream(fileDirectory, FileMode.Open);
                for (int i = 0; (hexIn = fs.ReadByte()) != -1; i++)
                {
                    hex = string.Format("{0:X2}", hexIn);
                    string binaryString = String.Join(String.Empty, hex.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
                    if (counter < 8)
                    {
                        left8[counter] = binaryString;
                        counter++;
                    }
                    else
                    {
                        right8[counter - 8] = binaryString;
                        counter++;
                        if (counter == 16)
                        {
                            CompileSprite(left8, right8);
                            counter = 0;
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Error: File not found. Press any key to exit.");
            }
        }

        public static void CompileSprite(string[] leftBytes, string[] rightBytes)
        {
            string[] compiledSprite = new string[8];
            for (int i = 0; i < 8; i++) // 8 lines of 8
            {
                for (int j = 0; j < 8; j++)
                {
                    if (leftBytes[i][j].Equals('1') && rightBytes[i][j].Equals('1'))
                    {
                        compiledSprite[i] += "3";
                    }
                    else if (leftBytes[i][j].Equals('0') && rightBytes[i][j].Equals('0'))
                    {
                        compiledSprite[i] += "0";
                    }
                    else if (leftBytes[i][j].Equals('1') && rightBytes[i][j].Equals('0'))
                    {
                        compiledSprite[i] += "1";
                    }
                    else if (leftBytes[i][j].Equals('0') && rightBytes[i][j].Equals('1'))
                    {
                        compiledSprite[i] += "2";
                    }
                }
                
            }

            for (int i = 0; i < 8; i++)
            {
                //Console.WriteLine(compiledSprite[i]);
                for (int j = 0; j < 8; j++)
                {
                    switch (compiledSprite[i][j])
                    {
                        case '0':
                            Console.ForegroundColor = ConsoleColor.Black;
                            break;
                        case '1':
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case '2':
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case '3':
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Black;
                            break;
                    }
                    Console.Write("█");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
