using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace виселица
{
    public class Runtime
    {
        public static int DifficultyChoice()
        {
            Console.Clear();
            Console.WriteLine("Выберите уровень сложности\n1 - нормальный\n2 - сложный");
            int i = int.Parse(Console.ReadLine());
            switch (i)
            {
                case 1:
                    break;

                case 2:
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("\nНеверный выбор. Пожалуйста, попробуйте снова.");
                    DifficultyChoice();
                    break;
            }
            return i;
        }
        
        public static int MainMenu(out bool gamestart, out bool leaderboard)
        {
            Console.Clear();
            Console.WriteLine("ВИСЕЛИЦА\n\n1 - начать игру\n2 - список лидеров\n3 - выход");

            int menuchoice = int.Parse(Console.ReadLine());

            gamestart = false;
            leaderboard = false;
            int difficulty = 0;
            switch (menuchoice)
            {
                case 1:
                    gamestart = true;
                    difficulty = DifficultyChoice();
                    break;
                
                case 2:
                    leaderboard = true;
                    break;
                
                case 3:
                    Environment.Exit(0);
                    break;
                
                default:
                    Console.Clear();
                    Console.WriteLine("Неверный выбор. Пожалуйста, попробуйте снова.");
                    MainMenu(out gamestart, out leaderboard);
                    break;
            }
            return difficulty;
        }

        public static bool gameOver = false;

        public static void Gameplay()
        {
            bool gamestart = false, leaderboard = false;
            int wins = 0;
            while (true)
            {
                if (gameOver)
                {
                    Console.Write("Введите 'да', чтобы начать новую игру, или любое другое слово для выхода: ");
                    string? restartChoice = Console.ReadLine();
                    if (restartChoice.ToLower() != "да")
                    {
                        break;
                    }

                    gameOver = false;
                    gamestart = true;
                    continue;
                }

                int difficulty = MainMenu(out gamestart, out leaderboard);
                Console.Clear();

                //Непрерывная игра
                while (true)
                {
                    if (gamestart)
                    {
                        string word_random = DBCooperation.GetRandomWordFromDatabase(difficulty);
                        string word = word_random;
                        char[] hiddenword = new char[word.Length];
                        int lives = 6;

                        for (int i = 0; i < word.Length; i++)
                        {
                            hiddenword[i] = '_';
                        }

                        Output.Response(lives, hiddenword);

                        while (lives > /*minlives*/ 0 && new string(hiddenword) != word)
                        {
                            char letter = Console.ReadKey().KeyChar;

                            Console.Clear();

                            bool letterFound = false;

                            for (int i = 0; i < word.Length; i++)
                            {
                                if (letter == hiddenword[i])
                                {
                                    Console.WriteLine("Вы уже вводили {0}\n", letter);
                                    letterFound = true;
                                    break;
                                }

                                if (letter == word[i])
                                {
                                    hiddenword[i] = letter;
                                    letterFound = true;
                                }
                            }

                            if (!letterFound)
                            {
                                lives--;
                            }

                            Output.Response(lives, letter, hiddenword);
                        }

                        if (lives == /*minlives*/ 0)
                        {
                            Console.WriteLine("Вы проиграли");
                            gameOver = true;
                            Console.Write("Введите ваше имя: ");
                            string? playerName = Console.ReadLine();
                            DBCooperation.AddPlayerToLeaderboard(playerName, wins);
                            gamestart = false;

                            MainMenu(out gamestart, out leaderboard);
                        }
                        
                        else
                        {
                            Console.WriteLine("Вы выиграли");
                            wins++;
                            Console.Write("Введите 'да', чтобы начать новую игру, или 'выход', чтобы выйти: ");
                            string? restartChoice = Console.ReadLine();
                            if (restartChoice.ToLower() != "да")
                            {
                                Console.Write("Введите ваше имя для списка лидеров: ");
                                string? playerName = Console.ReadLine();
                                DBCooperation.AddPlayerToLeaderboard(playerName, wins);
                                MainMenu(out gamestart, out leaderboard);

                            }
                        }
                        Console.Clear();
                    }

                    if (leaderboard)
                    {
                        Console.WriteLine("1 - вернуться в главное меню");
                        Program.DisplayLeaderboard();
                        bool validOption = false;
                        while (!validOption)
                        {
                            string? leaderboardChoice = Console.ReadLine();
                            if (leaderboardChoice == "1")
                            {
                                validOption = true;
                                Console.Clear();
                                leaderboard = false;
                                MainMenu(out gamestart, out leaderboard);
                            }
                        }
                    }
                }
            }
        }
    }
}
