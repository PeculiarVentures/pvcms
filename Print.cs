using System;
namespace pvcms
{

    public class Print
    {
        public Print()
        {


        }

        public static void Write(string message)
        {
            Console.Write(message);
        }

        public static void Write(string message, ConsoleColor foreColor)
        {
            try
            {
                Console.ForegroundColor = foreColor;
                Console.Write(message);
            }
            finally
            {
                Console.ResetColor();
            }
        }

        public static void WriteLine()
        {
            Console.WriteLine();
        }

        public static void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public static void WriteLine(string message, ConsoleColor foreColor)
        {
            try
            {
                Console.ForegroundColor = foreColor;
                Console.WriteLine(message);
            }
            finally
            {
                Console.ResetColor();
            }
        }

        public static void Info(string message)
        {
            Write("info:  ", ConsoleColor.Blue);
            WriteLine(message);
        }

        public static void Debug(string message)
        {
            Write("debug: ", ConsoleColor.Blue);
            WriteLine(message, ConsoleColor.Gray);
        }

        public static void Error(string message)
        {
            Write("error: ", ConsoleColor.Red);
            WriteLine(message, ConsoleColor.Red);
        }

        public static void Error(Exception error)
        {
            Write("error: ", ConsoleColor.Red);
            WriteLine(error.Message, ConsoleColor.Red);
            WriteLine(error.StackTrace, ConsoleColor.Gray);
        }
    }
}
