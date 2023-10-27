using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace виселица
{
    internal class Leaderboard
    {
        public static void Show()
        {
            Console.Clear();
            DataBase.DisplayLeaderboard();
            bool validOption = false;
            Console.Write("\n   0 - вернуться в главное меню\n\n   > ");
            while (!validOption)
            {
                string? leaderboardChoice = Console.ReadLine();
                if (leaderboardChoice == "0")
                {
                    validOption = true;
                    Menu.MainMenu();
                }
                else Leaderboard.Show();
            }
        }
    }                       
}
