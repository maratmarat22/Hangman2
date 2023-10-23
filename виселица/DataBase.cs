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
            string connectionString = "Data Source=.\\test.db; Version=3;";

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

        public static void SaveGame(string playerName, string word, string hiddenwordString, int lives)
        {
            string connectionString = "Data Source=.\\test.db; Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO saves (name, word, hidden_word, lives) VALUES (@name, @word, @hiddenWord, @lives)";
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", playerName);
                    command.Parameters.AddWithValue("@word", word);
                    command.Parameters.AddWithValue("@hiddenWord", hiddenwordString);
                    command.Parameters.AddWithValue("@lives", lives);
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
            string connectionString = "Data Source=.\\test.db; Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM saves";
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                SQLiteDataReader reader = command.ExecuteReader();
                Console.WriteLine("Сохраненные игры: ");
                while (reader.Read())
                {
                    string playerName = reader.GetString(0);
                    string hiddenWord = reader.GetString(2);
                    int lives = reader.GetInt32(3);

                    Console.WriteLine("Имя игрока: {0}, Угаданное слово: {1}, Жизни: {2}", playerName, hiddenWord, lives);
                }
                reader.Close();

                Console.Write("Введите имя игрока, чью игру вы хотите продолжить: ");
                string playerNameInput = Console.ReadLine();

                sql = "SELECT * FROM saves WHERE name = @playerName";
                command = new SQLiteCommand(sql, connection);
                command.Parameters.AddWithValue("@playerName", playerNameInput);
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string playerName = reader.GetString(0);
                    string word = reader.GetString(1);
                    string hiddenWord = reader.GetString(2);
                    char[] hiddenWordArray = hiddenWord.ToCharArray();
                    int lives = reader.GetInt32(3);

                    Console.Clear();
                    Console.WriteLine("Продолжаем сохраненную игру:");
                    Console.WriteLine("Имя игрока: {0}", playerName);
                    Console.WriteLine("Угаданное слово: {0}", string.Join(" ", hiddenWordArray));
                    Console.WriteLine("Жизни: {0}", lives);
                    Console.ReadLine();

                    //string[] GameData = { playerName, ",", word, ",", hiddenWord, ",", Convert.ToString(lives), "." };
                    //return GameData;

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
