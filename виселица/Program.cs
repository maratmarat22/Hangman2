namespace виселица
{
    internal class Program
    {
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
                string word = "виселица";
                char[] hiddenword = new char[word.Length];
                int mistakes = 0;
                int MaxMistakes = 6;

                for (int i = 0; i < word.Length; i++)
                {
                    hiddenword[i] = '_';
                }

                Program.Graphics(mistakes, hiddenword);

                while (mistakes < MaxMistakes && new string(hiddenword) != word)
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
                        mistakes++;
                    }

                    Program.Graphics(mistakes, letter, hiddenword);
                }

                if (mistakes == MaxMistakes)
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
    }
}