using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace виселица
{
    public class DBCooperation
    {
        public static string GetRandomWordFromDatabase(int difficulty)
        {
            string connectionString = "Data Source=C:\\Users\\ASUS\\Desktop\\шарп\\виселица\\виселица\\bin\\test.db; Version=3;";
            //string connectionString = "Data Source=C:\\Users\\Nezna\\OneDrive\\Desktop\\ggithub\\виселица\\bin\\test.db; Version=3;";

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
            string connectionString = "Data Source=C:\\Users\\ASUS\\Desktop\\шарп\\виселица\\виселица\\bin\\test.db; Version=3;";
            //string connectionString = @"Data Source=C:\Users\Nezna\OneDrive\Desktop\ggithub\виселица\bin\test.db; Version=3;";

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

    }
}
