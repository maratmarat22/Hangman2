using System.Data.SQLite;
using System.Diagnostics.Tracing;

namespace виселица
{
    internal class Program
    {

        static string Maindsdf()
        {
            string connectionString = "Data Source=C:\\Users\\ASUS\\Source\\Repos\\тест\\тест\\bin\\test.db; Version=3;";

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



        static void Main(string[] args)
        {
            //меню
            Hangman hangman = new Hangman();

            Console.WriteLine("1 - начать игру\n2 - список лидеров");

            int menuchoice = int.Parse(Console.ReadLine());
            bool gamestart = false, leaderboard = false;

            switch (menuchoice)
            {
                case 1:
                    gamestart = true; break;
                case 2:
                    leaderboard = true; break;
            }

            Console.Clear();

            //один уровень
            if (gamestart)
            {
                string word_random = Maindsdf();
                string word = word_random;
                char[] hiddenword = new char[word.Length];
                int lives = 6;

                for (int i = 0; i < word.Length; i++)
                {
                    hiddenword[i] = '_';
                }

                Program.Graphics(lives, hiddenword);

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

                    Program.Graphics(lives, letter, hiddenword);
                }

                if (lives == 0)
                {
                    Console.WriteLine("Вы проиграли");
                }
                else
                {
                    Console.WriteLine("Вы выиграли");
                }
                Console.ReadKey();
            }

            //список лидеров
            if (leaderboard)
            {
                Console.WriteLine("1 - петя\n2 - вася\n3 - коля");
            }


            
        }

        static void Graphics(int mistakes, char[] hiddenword)
        {
            Console.WriteLine("Ошибок: {0}", mistakes);
            Console.WriteLine("Вы ввели: -\n");
            Hangman.Output(mistakes);
            Console.WriteLine("\n{0}", string.Join(" ", hiddenword));
        }
        static void Graphics(int mistakes, char letter, char[] hiddenword)
        {
            Console.WriteLine("Ошибок: {0}", mistakes);
            Console.WriteLine("Вы ввели: {0}\n", letter);
            Hangman.Output(mistakes);
            Console.WriteLine("\n{0}", string.Join(" ", hiddenword));
        }
    }
}