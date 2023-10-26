using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace виселица
{
    internal class Gameplay
    {
        private static void SaveGame(string playerName, string word, char[] hiddenword, int lives, string difficulty)
        {
            string hiddenwordString = new string(hiddenword);
            DataBase.SaveGame(playerName, word, hiddenwordString, lives, difficulty);
        }


        public static void ContinueSavedGame(string playerName)
        {
            int wins = 0;
            string savedWord = DataBase.GetSavedWord(playerName);
            string savedHiddenWord = DataBase.GetSavedHiddenWord(playerName);
            int savedLives = DataBase.GetSavedLives(playerName);
            int savedDifficulty = DataBase.GetSavedDifficulty(playerName);


            Console.Clear();

            char[] hiddenword = savedHiddenWord.ToCharArray();
            string word = savedWord;
            int lives = savedLives;
            string difficulty = "";
            if (savedDifficulty == 1)
            {
                difficulty = "легкий";
            }
            else if (savedDifficulty == 2)
            {
                difficulty = "сложный";
            }


            Output.Response(lives, hiddenword);

            while (lives > 0 && new string(hiddenword) != word)
            {
                char letter = Console.ReadKey().KeyChar;

                Console.Clear();

                bool letterFound = false;

                if (letter == '0')
                {
                    Console.Write("Введите ваше имя для сохранения: ");
                    playerName = Console.ReadLine();
                    SaveGame(playerName, word, hiddenword, lives, difficulty);
                    Console.Clear();
                    Console.WriteLine("Игра сохранена!");
                    Menu.MainMenu();
                }

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

            if (lives == 0)
            {
                Console.WriteLine("Вы проиграли");
                Console.Write("Введите ваше имя: ");
                playerName = Console.ReadLine();
                DataBase.AddPlayerToLeaderboard(playerName, wins);
                Menu.MainMenu();
            }
            else
            {
                Console.WriteLine("Вы выиграли");
                Console.Write("Нажмите любую клавишу, чтобы начать новую игру, или 'выход', чтобы выйти: ");
                string restartChoice = Console.ReadLine();
                if (restartChoice.ToLower() != "да")
                {
                    Console.Write("Введите ваше имя для списка лидеров: ");
                    playerName = Console.ReadLine();
                    DataBase.AddPlayerToLeaderboard(playerName, wins);
                    Menu.MainMenu();
                }
                else
                {
                    Level();
                }
            }

            Console.Clear();
        }



        private static bool _isGameLoaded = false;

        public static void Level()
        {
            int wins = 0;

            while (true)
            {
                Console.Clear();
                int SetDifficulty = Menu.SetDifficulty();
                string difficulty = "";
                if (SetDifficulty == 1)
                {
                    difficulty = "легкий";
                }
                else if (SetDifficulty == 2)
                {
                    difficulty = "сложный";
                }

                //Непрерывная игра
                while (true)
                {
                    string word;
                    char[] hiddenword;
                    int lives;

                    /*if (!_isGameLoaded)
                    {*/
                    word = DataBase.GetRandomWord(SetDifficulty);
                    hiddenword = new char[word.Length];
                    lives = 6;

                    for (int i = 0; i < word.Length; i++)
                    {
                        hiddenword[i] = '_';
                    }
                   
                    Output.Response(lives, hiddenword);

                    while (lives > 0 && new string(hiddenword) != word)
                    {
                        char letter = Console.ReadKey().KeyChar;

                        Console.Clear();

                        bool letterFound = false;

                        if (letter == '0')
                        {
                            Console.Write("Введите ваше имя для сохранения: ");
                            string playerName = Console.ReadLine();
                            SaveGame(playerName, word, hiddenword, lives, difficulty);
                            //_isGameSaved = true;
                            Console.Clear();
                            Console.WriteLine("Игра сохранена!");
                            Menu.MainMenu();
                        }

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

                    if (lives == 0)
                    {
                        Console.WriteLine("Вы проиграли");
                        //gameOver = true;
                        Console.Write("Введите ваше имя: ");
                        string playerName = Console.ReadLine();
                        DataBase.AddPlayerToLeaderboard(playerName, wins);
                        Menu.MainMenu();
                    }

                    else
                    {
                        Console.WriteLine("Вы выиграли");
                        wins++;
                        Console.Write("Нажмите любую клавишу, чтобы начать новую игру, или 'выход', чтобы выйти: ");
                        string restartChoice = Console.ReadLine();
                        if (restartChoice.ToLower() != "да")
                        {
                            Console.Write("Введите ваше имя для списка лидеров: ");
                            string playerName = Console.ReadLine();
                            DataBase.AddPlayerToLeaderboard(playerName, wins);
                            Menu.MainMenu();
                        }
                    }
                    Console.Clear();
                    //isGameSaved = false;

                }
            }
        }
            }
}
