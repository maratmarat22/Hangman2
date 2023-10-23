using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace виселица
{
    public class Runtime
    {
        /*public static void ContinueGame()
        {
            string connectionString = "Data Source=.\\test.db; Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM saves";
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                SQLiteDataReader reader = command.ExecuteReader();
                Console.WriteLine("Сохраненные игры:");
                while (reader.Read())
                {
                    string playerName = reader.GetString(0);
                    string word = reader.GetString(1);
                    string hiddenWord = reader.GetString(2);
                    int lives = reader.GetInt32(3);

                    Console.WriteLine("Имя игрока: {0}, Слово: {1}, Угаданное слово: {2}, Жизни: {3}", playerName, word, hiddenWord, lives);
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
                    int lives = reader.GetInt32(3);

                    Console.Clear();
                    Console.WriteLine("Продолжаем сохраненную игру:");
                    Console.WriteLine("Имя игрока: {0}", playerName);
                    Console.WriteLine("Загаданное слово: {0}", word);
                    char[] hiddenWordArray = hiddenWord.ToCharArray();
                    Console.WriteLine("Угаданное слово: {0}", string.Join(" ", hiddenWordArray));
                    Console.WriteLine("Жизни: {0}", lives);
                    Console.ReadLine();

                }
                else
                {
                    Console.WriteLine("Сохраненная игра для игрока {0} не найдена.", playerNameInput);
                }
                reader.Close();
            }
        }



        private static bool isGameSaved = false;

        private static void SaveGame(string playerName, string word, char[] hiddenword, int lives)
        {
            string hiddenWordString = new string(hiddenword);
            DataBase.SaveGame(playerName, word, hiddenWordString, lives);
        }*/

        

        

        public static bool _gameOver = false;
        
        public static void Level()
        {
            bool gamestart = true; 
            bool leaderboard = false;
            int wins = 0;

            while (true)
            {
                /*if (gameOver)
                {
                    Console.Write("Введите 'да', чтобы начать новую игру, 'продолжить', чтобы загрузить сохраненную игру, или любое другое слово для выхода: ");
                    string restartChoice = Console.ReadLine();
                    
                    if (restartChoice.ToLower() != "да" && restartChoice.ToLower() != "продолжить")
                    {
                        break;
                    }

                    gameOver = false;
                    gamestart = true;
                    continue;
                }*/

                Console.Clear();
                int difficulty = Menu.SetDifficulty();

                //Непрерывная игра
                while (true)
                {
                    if (gamestart)
                    {
                        string word;
                        char[] hiddenword;
                        int lives;

                        /*if (!isGameSaved)
                        {*/
                            word = DataBase.GetRandomWord(difficulty);
                            hiddenword = new char[word.Length];
                            lives = 6;

                            for (int i = 0; i < word.Length; i++)
                            {
                                hiddenword[i] = '_';
                            }
                        /*}
                        else
                        {
                            ContinueGame();
                            gamestart = false;
                            Console.Clear();
                            continue;
                        }*/

                        Output.Response(lives, hiddenword);

                        while (lives > 0 && new string(hiddenword) != word)
                        {                            
                            char letter = Console.ReadKey().KeyChar;

                            Console.Clear();

                            bool letterFound = false;

                            if (letter == '0'/* && !isGameSaved*/)
                            {
                                Console.Write("Введите ваше имя для сохранения: ");
                                string playerName = Console.ReadLine();
                                //SaveGame(playerName, word, hiddenword, lives);
                                //isGameSaved = true;
                                Console.Clear();
                                Console.WriteLine("Игра сохранена!");
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
                            _gameOver = true;
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

                    if (leaderboard)
                    {
                        Console.WriteLine("1 - вернуться в главное меню");
                        DataBase.DisplayLeaderboard();
                        bool validOption = false;
                        while (!validOption)
                        {
                            string? leaderboardChoice = Console.ReadLine();
                            if (leaderboardChoice == "1")
                            {
                                validOption = true;
                                Console.Clear();
                                leaderboard = false;
                                Menu.MainMenu();
                            }
                        }
                    }
                }
            }
        }
    }
}
            