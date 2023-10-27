using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace виселица
{
    public class DataBase
    {
        public static void RemoveSavedGame(string playerName)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=.\\test.db;Version=3;"))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand("DELETE FROM saves WHERE name = @name", connection))
                {
                    command.Parameters.AddWithValue("@name", playerName);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static int GetSavedWins(string playerName)
        {
            int savedWins = 0;

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=.\\test.db;Version=3;"))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand("SELECT wins FROM saves WHERE name = @name", connection))
                {
                    command.Parameters.AddWithValue("@name", playerName);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        savedWins = reader.GetInt32(0);
                    }
                }
            }
            return savedWins;
        }

        public static int GetSavedDifficulty(string playerName)
        {
            string sqlDifficulty = string.Empty;
            int savedDifficulty = 0;

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=.\\test.db;Version=3;"))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand("SELECT difficulty FROM saves WHERE name = @name", connection))
                {
                    command.Parameters.AddWithValue("@name", playerName);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            sqlDifficulty = reader.GetString(0);

                            if (sqlDifficulty == "легкий")
                            {
                                savedDifficulty = 1;
                            }
                            else if (sqlDifficulty == "сложный")
                            {
                                savedDifficulty = 2;
                            }
                        }
                    }
                }
            }
            return savedDifficulty;
        }


        public static string GetSavedWord(string playerName)
        {
            string savedWord = string.Empty;

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=.\\test.db;Version=3;"))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand("SELECT word FROM saves WHERE name = @name", connection))
                {
                    command.Parameters.AddWithValue("@name", playerName);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            savedWord = reader.GetString(0);
                        }
                    }
                }
            }

            return savedWord;
        }

        public static string GetSavedHiddenWord(string playerName)
        {
            string savedHiddenWord = string.Empty;

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=.\\test.db;Version=3;"))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand("SELECT hidden_word FROM saves WHERE name = @name", connection))
                {
                    command.Parameters.AddWithValue("@name", playerName);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            savedHiddenWord = reader.GetString(0);
                        }
                    }
                }
            }

            return savedHiddenWord;
        }

        public static int GetSavedLives(string playerName)
        {
            int savedLives = 0;

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=.\\test.db;Version=3;"))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand("SELECT lives FROM saves WHERE name = @name", connection))
                {
                    command.Parameters.AddWithValue("@name", playerName);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            savedLives = reader.GetInt32(0);
                        }
                    }
                }
            }

            return savedLives;
        }


        public static string GetRandomWord(int difficulty)
        {
            string connectionString = "Data Source=.\\test.db; Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                string sql = "";

                connection.Open();
                if (difficulty == 2)
                {
                    sql = "SELECT slovo FROM words WHERE uroven = 'сложный'";
                }
                
                else if (difficulty == 1)
                {
                    sql = "SELECT slovo FROM words WHERE uroven = 'легкий'";
                }

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

        public static void AddPlayerToLeaderboard(string playerName, int wins)
        {
            string connectionString = "Data Source=test.db; Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string checkSql = "SELECT COUNT(*) FROM Leaderboard WHERE Name = @name";
                using (SQLiteCommand checkCommand = new SQLiteCommand(checkSql, connection))
                {
                    checkCommand.Parameters.AddWithValue("@name", playerName);
                    int existingCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (existingCount > 0)
                    {
                        Console.WriteLine("Имя уже существует. Выберите новое имя:");
                        playerName = Console.ReadLine();

                        while (true)
                        {
                            if (playerName == "0")
                            {
                                Console.WriteLine("Простите, имя '0' не доступно");
                                playerName = Console.ReadLine();
                            }
                            else break;
                        }
                        AddPlayerToLeaderboard(playerName, wins);
                        return; 
                    }
                }

                string insertSql = "INSERT INTO Leaderboard (Name, Wins) VALUES (@name, @wins)";
                using (SQLiteCommand insertCommand = new SQLiteCommand(insertSql, connection))
                {
                    insertCommand.Parameters.AddWithValue("@name", playerName);
                    insertCommand.Parameters.AddWithValue("@wins", wins);
                    insertCommand.ExecuteNonQuery();
                }
            }
        }

        public static void SaveGame(string playerName, string word, string hiddenwordString, int lives, string difficulty)
        {
            string connectionString = "Data Source=.\\test.db; Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO saves (name, word, hidden_word, lives, difficulty) VALUES (@name, @word, @hiddenWord, @lives, @difficulty)";
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", playerName);
                    command.Parameters.AddWithValue("@word", word);
                    command.Parameters.AddWithValue("@hiddenWord", hiddenwordString);
                    command.Parameters.AddWithValue("@lives", lives);
                    command.Parameters.AddWithValue("@difficulty", difficulty);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DisplayLeaderboard()
        {
            string connectionString = "Data Source=.\\test.db; Version=3;";

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

        public static string LoadGame()
        {
            Console.Clear();
            string connectionString = "Data Source=.\\test.db; Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM saves";
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                SQLiteDataReader reader = command.ExecuteReader();
                Console.WriteLine("0 - вернуться в главное меню");
                Console.WriteLine("Сохраненные игры: ");
                while (reader.Read())
                {
                    string playerName = reader.GetString(0);
                    string hiddenWord = reader.GetString(2);
                    int lives = reader.GetInt32(3);
                    string difficulty = reader.GetString(4);

                    Console.WriteLine("Имя игрока: {0}, Угаданное слово: {1}, Жизни: {2}, Уровень {3}", playerName, hiddenWord, lives, difficulty);                    
                }
                reader.Close();

                Console.Write("Введите имя игрока, чью игру вы хотите продолжить: ");
                string playerNameInput = Console.ReadLine();
                if (playerNameInput == "0")
                {
                    Menu.MainMenu();
                    return "0";
                }
                else
                {
                    sql = "SELECT * FROM saves WHERE name = @playerName";
                    command = new SQLiteCommand(sql, connection);
                    command.Parameters.AddWithValue("@playerName", playerNameInput);
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        string playerName = reader.GetString(0);
                        //string word = reader.GetString(1);
                        string hiddenWord = reader.GetString(2);
                        char[] hiddenWordArray = hiddenWord.ToCharArray();
                        int lives = reader.GetInt32(3);
                        string difficulty = reader.GetString(4);

                        Console.Clear();
                        Console.WriteLine("Продолжаем сохраненную игру:");
                        Console.WriteLine("Имя игрока: {0}", playerName);
                        Console.WriteLine("Угаданное слово: {0}", string.Join(" ", hiddenWordArray));
                        Console.WriteLine("Жизни: {0}", lives);
                        Console.WriteLine("Уровень: {0}", difficulty);
                        Console.ReadLine();

                        reader.Close();
                        return playerNameInput;
                    }
                    else
                    {
                        Console.WriteLine("Сохраненная игра для игрока {0} не найдена.", playerNameInput);

                        reader.Close();
                        return "0";
                    }
                }                                
            }
        }
    }
}
