/*
 * Домашнее задание
 * Обработка ошибок и валидация данных
 */
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;
using System.Linq.Expressions;

namespace hw_3;

class Program
{
   static void Main()
   {
       //начальные параменты 
       int maxTaskCount = 0;
       int lenghtTaskLimit = 0;
       while (true)
       {
           try
           {
               //начальные параметры
               maxTaskCount = GetMaxTaskCount();
               lenghtTaskLimit = GetLenghtTaskLimit();

               //чат бот 
               var handler = new UpdateHandler(new UserService(), new ToDoService(maxTaskCount, lenghtTaskLimit));
               var botClient = new ConsoleBotClient();
               botClient.StartReceiving(handler);

           }
           catch (ArgumentException ex)
           {
               PrintException(ex);
           }
           catch (Exception ex)
           {
               PrintExceptionAll(ex);
           }
 
       }
   }

   private static void PrintExceptionAll(Exception ex)
       {
           Console.WriteLine("\n=== Произошла непредвиденная ошибка ===");
           Console.WriteLine($"Тип: {ex.GetType()}");
           Console.WriteLine($"Сообщение: {ex.Message}");
           Console.WriteLine($"Стек вызова: {ex.StackTrace}");
           if (ex.InnerException != null)
           {
               Console.WriteLine("\n=== Внутреннее исключение ===");
               Console.WriteLine($"Тип: {ex.InnerException.GetType()}");
               Console.WriteLine($"Сообщение: {ex.InnerException.Message}");
               Console.WriteLine($"Стек вызова: {ex.InnerException.StackTrace}");
           }
       }

   private static void PrintException(Exception ex)
   {
           Console.ForegroundColor = ConsoleColor.DarkRed;
           Console.WriteLine($"=== Исключение  {ex.GetType()} ===");
           Console.WriteLine($"Сообщение: {ex.Message}");
           Console.ResetColor();
    }


   private static int GetMaxTaskCount()
   {
           Console.Write($"Введите максимальное количество задач: ");
           string? str = Console.ReadLine();
           return ParseAndValidateInt(str, 1, 100);
    }

   private static int GetLenghtTaskLimit()
   {
           Console.Write($"Введите максимально доступную длину задачи: ");
           string? str = Console.ReadLine();
           return ParseAndValidateInt(str, 1, 100);
    }

   private static int ParseAndValidateInt(string? str, int min, int max)
       {
           if (int.TryParse(str, out int maxTaskCount) && !(maxTaskCount >= 1 & maxTaskCount <= 100))
           {
               throw new ArgumentException($"Значение должно быть от {min} до {max}", nameof(str));
           }

           return maxTaskCount;
       }
   }