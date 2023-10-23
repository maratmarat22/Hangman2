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
            Console.WriteLine("1 - вернуться в главное меню");
            DataBase.DisplayLeaderboard();
            bool validOption = false;
            while (!validOption)
            {
                string? leaderboardChoice = Console.ReadLine();
                if (leaderboardChoice == "1")
                {
                    validOption = true;
                    Menu.MainMenu();
                }
            }
        }
    }                       
}
