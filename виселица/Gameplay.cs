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
            playerName = DataBase.LoadGame();

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
                char letter = char.ToLower(Console.ReadKey().KeyChar);

                Console.Clear();

                bool letterFound = false;

                if (letter == '0')
                {
                    //Console.Write("Введите ваше имя для сохранения: ");
                    //playerName = Console.ReadLine();
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
                Console.WriteLine("\nВы проиграли\n\nЗагаданное слово - {0}", word);
                Console.ReadLine();
                //Console.Write("Введите ваше имя: ");
                //playerName = Console.ReadLine();
                
                if (wins > 0)
                {
                    DataBase.AddPlayerToLeaderboard(playerName, wins);
                }
                    DataBase.RemoveSavedGame(playerName);                
                    Menu.MainMenu();
            }
            else
            {
                Console.WriteLine("\nВы выиграли");
                Console.Write("Нажмите любую клавишу, чтобы начать новую игру, или 'выход', чтобы выйти: ");
                string restartChoice = Console.ReadLine();
                if (restartChoice.ToLower() == "выход")
                {
                    //Console.Write("Введите ваше имя для списка лидеров: ");
                    //playerName = Console.ReadLine();
                    DataBase.AddPlayerToLeaderboard(playerName, wins);
                    DataBase.RemoveSavedGame(playerName);
                    Menu.MainMenu();
                }
                else
                {

                    while (true)
                    {
                        Console.Clear();
                        
                        //Непрерывная игра
                        while (true)
                        {
                            word = DataBase.GetRandomWord(savedDifficulty);
                            hiddenword = new char[word.Length];

                            for (int i = 0; i < word.Length; i++)
                            {
                                hiddenword[i] = '_';
                            }

                            Output.Response(lives, hiddenword);

                            while (lives > 0 && new string(hiddenword) != word)
                            {
                                char letter = char.ToLower(Console.ReadKey().KeyChar);

                                Console.Clear();

                                bool letterFound = false;

                                if (letter == '0')
                                {
                                    Console.Write("Введите ваше имя для сохранения: ");
                                    
                                    playerName = Output.NameException();
                                    
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
                                Console.WriteLine("\nВы проиграли\n\nЗагаднное слово - {0}", word);

                                if (wins > 0)
                                {
                                    Console.Write("Введите ваше имя: ");

                                    playerName = Output.NameException();

                                    DataBase.AddPlayerToLeaderboard(playerName, wins);
                                }
                                
                                Menu.MainMenu();
                            }

                            else
                            {
                                Console.WriteLine("\nВы выиграли");
                                wins++;
                                Console.Write("Нажмите любую клавишу, чтобы начать новую игру, или 'выход', чтобы выйти: ");
                                restartChoice = Console.ReadLine();
                                if (restartChoice.ToLower() == "выход")
                                {
                                    Console.Write("Введите ваше имя для списка лидеров: ");
                                    
                                    playerName = Output.NameException();
                                    
                                    DataBase.AddPlayerToLeaderboard(playerName, wins);
                                    Menu.MainMenu();
                                }
                            }
                            Console.Clear();
                        }
                    }
                }
            }

            Console.Clear();
        }

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
                    int lives = 0;

                    if (SetDifficulty == 1)
                    {
                        lives = 6;
                    }
                    else if (SetDifficulty == 2)
                    {
                        lives = 10;
                    }

                    word = DataBase.GetRandomWord(SetDifficulty);
                    hiddenword = new char[word.Length];

                    for (int i = 0; i < word.Length; i++)
                    {
                        hiddenword[i] = '_';
                    }

                    Output.Response(lives, hiddenword);

                    while (lives > 0 && new string(hiddenword) != word)
                    {
                        char letter = char.ToLower(Console.ReadKey().KeyChar);

                        Console.Clear();

                        bool letterFound = false;

                        if (letter == '0')
                        {
                            Console.Write("Введите ваше имя для сохранения: ");
                            string playerName = Output.NameException();
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
                        Console.WriteLine("\nВы проиграли\n\nЗагаданное слово - {0}", word);
                        Console.ReadLine();
                        if (wins > 0)
                        {
                            Console.Write("Введите ваше имя: ");
                            string? playerName = Output.NameException();
                            DataBase.AddPlayerToLeaderboard(playerName, wins);
                        }
                        
                        Menu.MainMenu();
                    }

                    else
                    {
                        Console.WriteLine("\nВы выиграли");
                        wins++;
                        Console.Write("Нажмите любую клавишу, чтобы начать новую игру, или 'выход', чтобы выйти: ");
                        string restartChoice = Console.ReadLine();
                        if (restartChoice.ToLower() == "выход")
                        {
                            Console.Write("Введите ваше имя для списка лидеров: ");
                            string? playerName = Output.NameException();
                            DataBase.AddPlayerToLeaderboard(playerName, wins);
                            Menu.MainMenu();
                        }
                    }
                    Console.Clear();
                }
            }
        }
            }
}
