using виселица;

namespace виселица
{
    internal class Gameplay
    {

        public static int wins = 0;

        private static void SaveGame(string playerName, string word, char[] hiddenword, int lives, string difficultyString, int wins)
        {
            string hiddenwordString = new(hiddenword);
            DataBase.SaveGame(playerName, word, hiddenwordString, lives, difficultyString, wins);
        }

        public static void ContinueLevel(string playerName)
        {
            string word = DataBase.GetSavedWord(playerName);
            string hiddenwordString = DataBase.GetSavedHiddenWord(playerName);
            char[] hiddenword = hiddenwordString.ToCharArray();
            int difficulty = DataBase.GetSavedDifficulty(playerName);
            int lives = DataBase.GetSavedLives(playerName);
            int wins = DataBase.GetSavedWins(playerName);


            string difficultyString = "";

            if (difficulty == 1)
            {
                difficultyString = "легкий";
            }
            else if (difficulty == 2)
            {
                difficultyString = "сложный";
            }


            Console.Clear();

            Output.Response(lives, hiddenword);


            while (lives > 0 && new string(hiddenword) != word)
            {
                Console.Write("   > ");
                char letter = char.ToLower(Console.ReadKey().KeyChar);

                Console.Clear();

                bool letterFound = false;

                if (letter == '0')
                {
                    DataBase.RemoveSavedGame(playerName);
                    SaveGame(playerName, word, hiddenword, lives, difficultyString, wins);
                    Menu.MainMenu();
                }

                for (int i = 0; i < word.Length; i++)
                {
                    if (letter == hiddenword[i])
                    {
                        Console.Write("\n   Вы уже вводили {0}\n", letter);
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
                Console.Write("\n   Вы проиграли :(\n\n   Загаданное слово - {0}, нажмите любую клавишу, чтобы продолжить\n\n   > ", word);
                Console.ReadLine();

                if (wins > 0)
                {
                    DataBase.AddPlayerToLeaderboard(playerName, wins);
                }

                DataBase.RemoveSavedGame(playerName);
                Menu.MainMenu();
            }
            else
            {
                Console.WriteLine("\n   Вы выиграли :)\n");
                wins++;
                Console.Write("   Нажмите любую клавишу, чтобы начать новую игру, или введите 'выход', чтобы выйти:\n\n   > ");
                string? restartChoice = Console.ReadLine();
                if (restartChoice.ToLower() == "выход")
                {
                    DataBase.AddPlayerToLeaderboard(playerName, wins);
                    DataBase.RemoveSavedGame(playerName);
                    Menu.MainMenu();
                }
                else
                {
                    Level(difficulty, difficultyString, playerName, wins);
                }
            }
            Console.Clear();
        }

        public static void Level(int difficulty)
        {
            while (true)
            {
                Console.Clear();

                string? difficultyString = default;

                if (difficulty == 1)
                {
                    difficultyString = "легкий";
                }
                else if (difficulty == 2)
                {
                    difficultyString = "сложный";
                }


                while (true)
                {
                    string word;
                    char[] hiddenword;
                    int lives = 0;

                    if (difficulty == 1)
                    {
                        lives = 6;
                    }
                    else if (difficulty == 2)
                    {
                        lives = 10;
                    }


                    word = DataBase.GetRandomWord(difficulty);
                    hiddenword = new char[word.Length];

                    for (int i = 0; i < word.Length; i++)
                    {
                        hiddenword[i] = '_';
                    }


                    Output.Response(lives, hiddenword);


                    while (lives > 0 && new string(hiddenword) != word)
                    {
                        Console.Write("   > ");
                        char letter = char.ToLower(Console.ReadKey().KeyChar);

                        Console.Clear();

                        bool letterFound = false;


                        if (letter == '0')
                        {
                            Console.Write("\n   Введите ваше имя для сохранения:\n\n   > ");
                            string playerName = Output.NameException();
                            DataBase.RemoveSavedGame(playerName);
                            SaveGame(playerName, word, hiddenword, lives, difficultyString, wins);
                            Menu.MainMenu();
                        }


                        for (int i = 0; i < word.Length; i++)
                        {
                            if (letter == hiddenword[i])
                            {
                                Console.Write("\n   Вы уже вводили {0}\n", letter);
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
                        Console.Write("\n   Вы проиграли :(\n\n   Загаданное слово - {0}, нажмите любую клавишу, чтобы продолжить\n\n   > ", word);
                        Console.ReadLine();
                        if (wins > 0)
                        {
                            Console.Clear();
                            Console.Write("\n   Введите ваше имя для списка лидеров:\n\n   > ");
                            string? playerName = Output.NameException();
                            DataBase.AddPlayerToLeaderboard(playerName, wins);
                        }

                        Menu.MainMenu();
                    }

                    else
                    {
                        Console.WriteLine("\n   Вы выиграли :)\n");
                        wins++;
                        Console.Write("   Нажмите любую клавишу, чтобы начать новую игру, или введите 'выход', чтобы выйти:\n\n   > ");
                        string? restartChoice = Console.ReadLine();
                        if (restartChoice.ToLower() == "выход")
                        {
                            Console.Clear();
                            Console.Write("\n   Введите ваше имя для списка лидеров:\n\n   > ");
                            string? playerName = Output.NameException();
                            DataBase.AddPlayerToLeaderboard(playerName, wins);
                            Menu.MainMenu();
                        }
                    }
                    Console.Clear();
                }
            }
        }

        public static void Level(int difficulty, string difficultyString, string playerName, int wins)
        {
            while (true)
            {
                Console.Clear();

                int lives = 0;

                while (true)
                {
                    if (difficulty == 1)
                    {
                        lives = 6;
                    }
                    else if (difficulty == 2)
                    {
                        lives = 10;
                    }


                    string word = DataBase.GetRandomWord(difficulty);
                    char[] hiddenword = new char[word.Length];


                    for (int i = 0; i < word.Length; i++)
                    {
                        hiddenword[i] = '_';
                    }

                    Output.Response(lives, hiddenword);

                    while (lives > 0 && new string(hiddenword) != word)
                    {
                        Console.Write("   > ");
                        char letter = char.ToLower(Console.ReadKey().KeyChar);

                        Console.Clear();

                        bool letterFound = false;

                        if (letter == '0')
                        {
                            DataBase.RemoveSavedGame(playerName);
                            SaveGame(playerName, word, hiddenword, lives, difficultyString, wins);
                            Console.Clear();
                            Menu.MainMenu();
                        }

                        for (int i = 0; i < word.Length; i++)
                        {
                            if (letter == hiddenword[i])
                            {
                                Console.Write("\n   Вы уже вводили {0}\n", letter);
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
                        Console.Write("\n   Вы проиграли :(\n\n   Загаданное слово - {0}, нажмите любую клавишу, чтобы продолжить\n\n   > ", word);

                        if (wins > 0)
                        {
                            DataBase.AddPlayerToLeaderboard(playerName, wins);
                        }

                        Menu.MainMenu();
                    }

                    else
                    {
                        Console.WriteLine("\n   Вы выиграли :)\n");
                        wins++;
                        Console.Write("   Нажмите любую клавишу, чтобы начать новую игру, или введите 'выход', чтобы выйти:\n\n   > ");
                        string restartChoice = Console.ReadLine();
                        if (restartChoice.ToLower() == "выход")
                        {
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
