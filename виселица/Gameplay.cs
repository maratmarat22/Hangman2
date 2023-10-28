namespace виселица
{
    internal class Gameplay
    {
        private static void SaveGame(string playerName, string word, char[] hiddenword, int lives, string difficulty, int wins)
        {
            string hiddenwordString = new(hiddenword);
            DataBase.SaveGame(playerName, word, hiddenwordString, lives, difficulty, wins);
        }


        public static void ContinueSavedGame(string playerName)
        {
            string savedWord = DataBase.GetSavedWord(playerName);
            string savedHiddenWord = DataBase.GetSavedHiddenWord(playerName);
            int savedLives = DataBase.GetSavedLives(playerName);
            int savedDifficulty = DataBase.GetSavedDifficulty(playerName);
            int savedWins = DataBase.GetSavedWins(playerName);
            //playerName = DataBase.LoadGame();

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

            int wins = savedWins;

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
                    SaveGame(playerName, word, hiddenword, lives, difficulty, wins);
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
                            if (savedDifficulty == 1)
                            {
                                lives = 6;
                            }
                            else if (savedDifficulty == 2)
                            {
                                lives = 10;
                            }
                            word = DataBase.GetRandomWord(savedDifficulty);
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
                                    //Console.Write("Введите ваше имя для сохранения: ");

                                    //playerName = Output.NameException();
                                    
                                    DataBase.RemoveSavedGame(playerName);
                                    SaveGame(playerName, word, hiddenword, lives, difficulty, wins);
                                    Console.Clear();
                                    Console.WriteLine("Игра сохранена!");
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
                                    //Console.Write("Введите ваше имя: ");

                                    //playerName = Output.NameException();

                                    DataBase.AddPlayerToLeaderboard(playerName, wins);
                                }

                                Menu.MainMenu();
                            }

                            else
                            {
                                Console.WriteLine("\n   Вы выиграли :)\n");
                                wins++;
                                Console.Write("   Нажмите любую клавишу, чтобы начать новую игру, или введите 'выход', чтобы выйти:\n\n   > ");
                                restartChoice = Console.ReadLine();
                                if (restartChoice.ToLower() == "выход")
                                {
                                    //Console.Write("Введите ваше имя для списка лидеров: ");

                                    //playerName = Output.NameException();

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

        public static int wins = 0;

        public static void Level()
        {
            while (true)
            {
                Console.Clear();
                int SetDifficulty = int.Parse(Menu.SetDifficulty());
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
                        Console.Write("   > ");
                        char letter = char.ToLower(Console.ReadKey().KeyChar);

                        Console.Clear();

                        bool letterFound = false;

                        if (letter == '0')
                        {
                            Console.Write("\n   Введите ваше имя для сохранения:\n\n   > ");
                            string playerName = Output.NameException();
                            DataBase.RemoveSavedGame(playerName);
                            SaveGame(playerName, word, hiddenword, lives, difficulty, wins);
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
    }
}
