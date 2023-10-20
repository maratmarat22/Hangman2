using System.Data.SQLite;
using System.Diagnostics.Tracing;

namespace виселица
{
    internal class Program
    {
        public static void DisplayLeaderboard()
        {
            string connectionString = "Data Source=C:\\Users\\ASUS\\Desktop\\шарп\\виселица\\виселица\\bin\\test.db; Version=3;";
            //string connectionString = @"Data Source=C:\\Users\\Nezna\\OneDrive\\Desktop\\ggithub\\виселица\\bin\\test.db; Version=3;";

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
            Runtime.Gameplay();
        }
    }
}
