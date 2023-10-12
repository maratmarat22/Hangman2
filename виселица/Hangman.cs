using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace виселица
{
    public class Hangman
    {       
        public static int Output(int mistakes) 
        {
            char[,] hangman =
                    {
                    { ' ' , '*' , '-' , '-' , '-' , '-' , '*' , ' ' , ' ' },
                    { ' ' , '|' , ' ' , ' ' , ' ' , ' ' , '|' , ' ' , ' ' },
                    { ' ' , ' ' , ' ' , ' ' , ' ' , ' ' , '|' , ' ' , ' ' },
                    { ' ' , ' ' , ' ' , ' ' , ' ' , ' ' , '|' , ' ' , ' ' },
                    { ' ' , ' ' , ' ' , ' ' , ' ' , ' ' , '|' , ' ' , ' ' },
                    { ' ' , ' ' , ' ' , ' ' , '=' , '=' , '=' , '=' , '=' }
                    };

            switch (mistakes)
            {
                case 0:                    
                    break;
                    
                case 1:
                    hangman[2, 1] = 'O';                    
                    break;

                case 2:
                    hangman[2, 1] = 'O';
                    hangman[3, 1] = '|';
                    break;
                    
                case 3:
                    hangman[2, 1] = 'O';
                    hangman[3, 1] = '|';
                    hangman[3, 0] = '/';
                    break;
                
                case 4:
                    hangman[2, 1] = 'O';
                    hangman[3, 1] = '|';
                    hangman[3, 0] = '/';
                    hangman[3, 2] = '\\';
                    break;
                case 5:
                    hangman[2, 1] = 'O';
                    hangman[3, 1] = '|';
                    hangman[3, 0] = '/';
                    hangman[3, 2] = '\\';
                    hangman[4, 0] = '/';
                    break;
                case 6:
                    hangman[2, 1] = 'O';
                    hangman[3, 1] = '|';
                    hangman[3, 0] = '/';
                    hangman[3, 2] = '\\';
                    hangman[4, 0] = '/';
                    hangman[4, 2] = '\\';
                    break;
            }

            for (int i = 0; i < hangman.GetLength(0); i++)
            {
                Console.Write("\t\t\t\t");
                for (int j = 0; j < hangman.GetLength(1); j++)
                {
                    Console.Write(hangman[i, j]);
                }
                Console.WriteLine();
            }
            return 0;
        }
    }
}
