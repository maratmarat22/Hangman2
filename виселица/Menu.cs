using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace виселица
{
    internal class Menu
    {
        public static void MainMenu()
        {
            Console.Clear();
            Console.WriteLine("ВИСЕЛИЦА\n\n1 - начать игру\n2 - список лидеров\n3 - выход\n4 - продолжить");

            int menuchoice = int.Parse(Console.ReadLine());

            switch (menuchoice)
            {
                case 1:
                    Gameplay.Level();
                    break;

                case 2:
                    Leaderboard.Show();
                    break;

                case 3:
                    Environment.Exit(0);
                    break;

                case 4:
                    string playerNameInput = DataBase.LoadGame();
                    if (playerNameInput != "0")
                    {
                        Gameplay.Level(playerNameInput);
                    }

                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Неверный выбор. Пожалуйста, попробуйте снова.");
                    MainMenu();
                    break;
            }
        }
        
        public static int SetDifficulty()
        {
            Console.Clear();
            Console.WriteLine("Выберите уровень сложности\n1 - нормальный\n2 - сложный");
            int difficulty = int.Parse(Console.ReadLine());
            switch (difficulty)
            {
                case 1:
                    break;

                case 2:
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("\nНеверный выбор. Пожалуйста, попробуйте снова.");
                    SetDifficulty();
                    break;
            }
            return difficulty;
        }
    }
}
