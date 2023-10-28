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
                    Gameplay.Level();
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
                        Gameplay.ContinueSavedGame(playerNameInput);
                    }
                    else Menu.MainMenu();

                    break;

                default:
                    Console.Clear();
                    MainMenu();
                    break;
            }
        }

        public static string SetDifficulty()
        {
            Console.Write("\n\tВЫБЕРИТЕ УРОВЕНЬ СЛОЖНОСТИ\n\n   1 - нормальный\n   2 - сложный\n\n   0 - вернуться в главное меню\n\n   > ");
            string? difficulty = Console.ReadLine();

            switch (difficulty)
            {
                case "1":
                    break;

                case "2":
                    break;

                case "0":
                    MainMenu();
                    break;

                default:
                    Console.Clear();
                    return SetDifficulty();
            }

            return difficulty;
        }
    }
}