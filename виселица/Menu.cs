﻿using System.Web;

namespace виселица
{
    internal class Menu
    {
        public static void MainMenu()
        {
            Console.Clear();
            Console.Write("\n\tВИСЕЛИЦА\n\n   1 - начать новую игру\n   2 - список лидеров\n   3 - выход\n   4 - продолжить\n\n   > ");

            string? menuchoice = Console.ReadLine();

            switch (menuchoice)
            {
                case "1":
                    SetDifficulty();
                    break;

                case "2":
                    Leaderboard.Show();
                    break;

                case "3":
                    Console.Clear();
                    Environment.Exit(0);
                    break;

                case "4":
                    string playerNameInput = DataBase.LoadGame();
                    if (playerNameInput != "0")
                    {
                        Gameplay.ContinueLevel(playerNameInput);
                    }
                    else MainMenu();

                    break;

                default:
                    Console.Clear();
                    MainMenu();
                    break;
            }
        }

        public static void SetDifficulty()
        {
            Console.Clear();
            Console.Write("\n\tВЫБЕРИТЕ УРОВЕНЬ СЛОЖНОСТИ\n\n   1 - нормальный\n   2 - сложный\n\n   0 - вернуться в главное меню\n\n   > ");
                        
            string? difficultyString = Console.ReadLine();

            switch (difficultyString)
            {
                case "1":
                    int difficulty = int.Parse(difficultyString);
                    Gameplay.FirstLevel(difficulty);
                    break;

                case "2":
                    difficulty = int.Parse(difficultyString);
                    Gameplay.FirstLevel(difficulty);
                    break;

                case "0":
                    MainMenu();
                    break;

                default:
                    SetDifficulty();
                    break;
            }
        }
    }
}