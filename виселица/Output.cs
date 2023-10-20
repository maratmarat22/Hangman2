using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace виселица
{
    public class Output
    {
        public static void Response(int lives, char[] hiddenword)
        {
            Graphics.Hearts(lives);
            Console.WriteLine("Вы ввели: -\n");
            Graphics.Hangman(lives);
            Console.WriteLine("\n{0}", string.Join(" ", hiddenword));
        }

        public static void Response(int lives, char letter, char[] hiddenword)
        {
            Graphics.Hearts(lives);
            Console.WriteLine("Вы ввели: {0}\n", letter);
            Graphics.Hangman(lives);
            Console.WriteLine("\n{0}", string.Join(" ", hiddenword));
        }
    }
}
