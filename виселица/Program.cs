using System.Data.SQLite;
using System.Diagnostics.Tracing;

namespace виселица
{
    internal class Program
    {
        private static bool gameOver = false;

        static string GetRandomWordFromDatabase()
        {
            string connectionString = "Data Source=C:\\Users\\Nezna\\OneDrive\\Desktop\\ggithub\\виселица\\bin\\test.db; Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT slovo FROM words";
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                SQLiteDataReader reader = command.ExecuteReader();
                var words = new List<string>();
                while (reader.Read())
                {
                    string word = reader.GetString(0);
                    words.Add(word);
                }
                reader.Close();

                Random rnd = new Random();
                int index = rnd.Next(words.Count);
                return words[index];
            }
        }

        private static void AddPlayerToLeaderboard(string playerName, int wins)
        {
            string connectionString = @"Data Source=C:\Users\Nezna\OneDrive\Desktop\ggithub\виселица\bin\test.db; Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Leaderboard (Name, Wins) VALUES (@name, @wins)";
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", playerName);
                    command.Parameters.AddWithValue("@wins", wins);
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void DisplayLeaderboard()
        {
            string connectionString = @"Data Source=C:\\Users\\Nezna\\OneDrive\\Desktop\\ggithub\\виселица\\bin\\test.db; Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT Name, Wins FROM Leaderboard ORDER BY Wins DESC";
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                SQLiteDataReader reader = command.ExecuteReader();
                Console.WriteLine("Лидеры:");
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    int wins = reader.GetInt32(1);
                    Console.WriteLine("{0}: {1} побед", name, wins);
                }
                reader.Close();
            }
        }

        private static void Main(string[] args)
        {
            bool gamestart = false, leaderboard = false;
            int wins = 0;

            while (true)
            {
                if (gameOver)
                {
                    Console.Write("Введите 'да', чтобы начать новую игру, или любое другое слово для выхода: ");
                    string restartChoice = Console.ReadLine();
                    if (restartChoice.ToLower() != "да")
                    {
                        break;
                    }

                    gameOver = false;
                    gamestart = true;
                    continue;
                }

                MainMenu(out gamestart, out leaderboard);

                Console.Clear();

                //Непрерывная игра
                while (true)
                {
                    if (gamestart)
                    {
                        string word_random = GetRandomWordFromDatabase();
                        string word = word_random;
                        char[] hiddenword = new char[word.Length];
                        int lives = 6;

                        for (int i = 0; i < word.Length; i++)
                        {
                            hiddenword[i] = '_';
                        }

                        Output(lives, hiddenword);

                        while (lives > 0 && new string(hiddenword) != word)
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

                            Output(lives, hiddenword);
                        }

                        if (lives == 0)
                        {
                            Console.WriteLine("Вы проиграли");
                            gameOver = true;
                            Console.Write("Введите ваше имя: ");
                            string playerName = Console.ReadLine();
                            AddPlayerToLeaderboard(playerName, wins);
                            gamestart = false;

                            MainMenu(out gamestart, out leaderboard);


                        }
                        else
                        {
                            Console.WriteLine("Вы выиграли");
                            wins++;
                            Console.Write("Введите 'да', чтобы начать новую игру, или 'выход', чтобы выйти: ");
                            string restartChoice = Console.ReadLine();
                            if (restartChoice.ToLower() != "да")
                            {
                                Console.Write("Введите ваше имя для списка лидеров: ");
                                string playerName = Console.ReadLine();
                                AddPlayerToLeaderboard(playerName, wins);
                                MainMenu(out gamestart, out leaderboard);

                            }
                        }
                        Console.Clear();
                    }

                    if (leaderboard)
                    {
                        Console.WriteLine("1 - вернуться в главное меню");
                        DisplayLeaderboard();
                        bool validOption = false;
                        while (!validOption)
                        {
                            string leaderboardChoice = Console.ReadLine();
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

        static void Output(int lives, char[] hiddenword)
        {
            Graphics.Hearts(lives);
            Console.WriteLine("Вы ввели: -\n");
            Graphics.Hangman(lives);
            Console.WriteLine("\n{0}", string.Join(" ", hiddenword));
        }

        static void MainMenu(out bool gamestart, out bool leaderboard)
        {
            Console.WriteLine("ВИСЕЛИЦА\n\n1 - начать игру\n2 - список лидеров\n3 - выход");

            int menuchoice = int.Parse(Console.ReadLine());

            gamestart = false;
            leaderboard = false;

            switch (menuchoice)
            {
                case 1:
                    gamestart = true;
                    break;
                case 2:
                    leaderboard = true;
                    break;
                case 3:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Пожалуйста, попробуйте снова.");
                    MainMenu(out gamestart, out leaderboard);
                    break;
            }
        }
    }
}