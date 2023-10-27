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
                        Gameplay.ContinueSavedGame(playerNameInput);
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
            Console.WriteLine("Выберите уровень сложности\n1 - нормальный\n2 - сложный\n\n0 - вернуться в главное меню");
            int difficulty;
            bool isValidInput = int.TryParse(Console.ReadLine(), out difficulty);
            while (!isValidInput)
            {
                Console.WriteLine("Неверный выбор. Пожалуйста, попробуйте снова.");
                isValidInput = int.TryParse(Console.ReadLine(), out difficulty);
            }
            switch (difficulty)
            {
                case 1:
                    break;

                case 2:
                    break;

                case 0:
                    MainMenu();
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Неверный выбор. Пожалуйста, попробуйте снова.");
                    return SetDifficulty();
            }

            return difficulty;
        }
    }
}
