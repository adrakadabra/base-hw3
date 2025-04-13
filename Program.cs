/*
 * Домашнее задание
 * Обработка ошибок и валидация данных
*/

using System.Linq.Expressions;

namespace hw_3;

class Program
{
    private static string? _userName  = "";
    private static bool _startComplete; 
    private static List<string> _task = new List<string>(); 
    private static int _maxTaskCount;
    private static int _lenghtTaskLimit;
    static void Main()
    {
        
        while (true)
        {
            try
            {
                if (_maxTaskCount == 0) GetMaxTaskCount();
                if (_lenghtTaskLimit == 0) GetLenghtTaskLimit();
                RunBot();
                break;
            }
            catch (TaskCountLimitException ex)
            {
                PrintException(ex);
            }     
            catch (TaskLengthLimitException ex)
            {
                PrintException(ex);
            }
            catch (DuplicateTaskException ex)
            {
                PrintException(ex);  
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
    
     private static void RunBot()
        {
            if (!_startComplete) PrintCommandWelcome();
            PrintCommandList(_startComplete);  
            do
            {
                Console.Write(">");
                string? str = Console.ReadLine();
                ValidateString(str);
                //if (string.IsNullOrEmpty(str))
                //{
                //    Console.WriteLine("Команда не распознана.");
                //    PrintCommandList(_startComplete);
                //    continue;
                //}
                
                string userCommand = "";
                if (str.IndexOf(' ') == -1)
                    userCommand = str;
                else
                    userCommand = str.Substring(0, str.IndexOf(' '));

                string strArgument = "";
                if (str.Length > 0 && str.IndexOf(' ') > 0)
                    strArgument = str.Substring(str.IndexOf(' ') + 1);

                switch (userCommand)
                {
                    case "/start":
                        _startComplete = CommandStart();
                        if (!_startComplete)
                            return;
                        else
                        {
                            PrintUserGreeting();
                            PrintCommandList(_startComplete);
                        }
                        break;
                    case "/echo" when _startComplete:
                        CommandEcho(strArgument);
                        //Console.WriteLine();
                        break;
                    case "/help":
                        CommandHelp();
                        break;
                    case "/info":
                        CommandInfo();
                        //Console.WriteLine();
                        break;
                    case "/addtask" when _startComplete:
                        if (!AddTask())
                        {
                            Console.WriteLine();
                            PrintCommandList(_startComplete); 
                        }
                        break;
                    case "/showtask" when _startComplete:
                        ShowTasks();
                        break;
                    case "/removetask" when _startComplete:
                        RemoveTask();
                        break;
                    case "/exit":
                        return;
                    default:
                        {
                            Console.WriteLine("Команда не распознана.");
                            PrintCommandList(_startComplete);
                            break;
                        }
                }
            }
            while (true);
        }
        private static void PrintCommandWelcome()
        {
            Console.WriteLine($"Здравствуйте, я чат-бот.");
        }
        private static void PrintUserGreeting()
        {
            Console.WriteLine();
            Console.WriteLine($"Добро пожаловать, {_userName} !");
        }
        private static void PrintCommandList(bool startMenu = false)
        {
            string textMainMenu = $"Вам доступные следующие команды: ";
            textMainMenu += startMenu ? "/echo /addtask /showtask /removetask/  " : "";
            textMainMenu += "/start /help /info /exit";
            Console.WriteLine(textMainMenu);
        }
        private static bool  CommandStart()
        {
            bool strNameEmpty = false;
            do
            {
                if (!strNameEmpty)
                    Console.Write("Введите своё имя: ");
                else
                    Console.WriteLine("Имя не может быть пустым!\nВведите своё имя или команду /exit");

                string? strName = Console.ReadLine();

                if (strName == "/exit")
                {
                    return false;
                }

                if (string.IsNullOrEmpty(strName))
                {
                    strNameEmpty = true;
                }
                else
                {
                    _userName = strName;
                    return true;
                }

            } while (true);
        }
        private static void CommandInfo() => Console.WriteLine("Чат - бот версия 1.1; дата создания 03.04.2025");
        private static void CommandEcho(string str) => Console.WriteLine($"\"{str}\"");
        private static void CommandHelp()
        {
            string text = """
                Справка:
                Для доступа к основному набору команд:
                    1.Выполнте команду /start
                        1.1 Введи своё имя
                    2.Вам будет доступна команда /echo /addtask /showtasks /removetask
                
                /echo <параметр> - команда чат-бота, выводит сообщение, переданное в параметр
                /start - инициализация чат-бота
                /exit  - завершение работы программы
                /info  - информация о чат-боте
                /addtask - добавить задачу
                /showtasks - показать список задач
                /removetask - удалить задачу
                """;
            Console.WriteLine(text);
        }

        private static bool AddTask()
        {
            if (_task.Count == _maxTaskCount)
            {
                throw new TaskCountLimitException(_maxTaskCount);
            }
            Console.Write("Введите описание задачи: ");
            string? task = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(task))
            {   
                if (task.Length > _lenghtTaskLimit)
                    throw new TaskLengthLimitException(task.Length,_lenghtTaskLimit);
                if (_task.Contains(task))
                    throw new DuplicateTaskException(task);
                _task.Add(task);
                Console.WriteLine("Задача добавлена");
                return true;
            }
            else
            {
                Console.WriteLine("Задача не добавлена. Описание задачи не может быть пустым!");
                return false;
            }
        }
        
        private static void ShowTasks()
        {
            if (_task.Count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }
            Console.WriteLine("Список задач:");
            for (int i = 0; i < _task.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_task[i]}");
            }
        }

        private static void RemoveTask()
        {
            if (_task.Count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }
            
            ShowTasks();
            
            Console.Write("Введите номер задачи для удаления: ");
            string? input = Console.ReadLine();
            //пытаемся преобразовать ввод в число
            if (int.TryParse(input, out int numberTask))
            {
                //задачи пронумерованы с 1
                int index = numberTask-1;
                //проверяем индекс в допустимых границах
                if (index >= 0 && index < _task.Count)
                {
                    _task.RemoveAt(index);
                    Console.WriteLine("Задача {0} удалена", numberTask);    
                }
                else
                {
                    Console.WriteLine("Задачи с номером {0} нет в списке", numberTask);
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
        
        private static void GetMaxTaskCount()
        { 
            Console.Write($"Введите максимальное количество задач: ");
            string? str = Console.ReadLine();
            _maxTaskCount = ParseAndValidateInt(str, 1,100);
        }

        private static void GetLenghtTaskLimit()
        { 
            Console.Write($"Введите максимально доступную длину задачи: ");
            string? str = Console.ReadLine();
            _lenghtTaskLimit = ParseAndValidateInt(str, 1,100);
        }
        
        private static int ParseAndValidateInt(string? str, int min, int max)
        {
            if (int.TryParse(str, out int maxTaskCount) && !(maxTaskCount >= 1 & maxTaskCount <= 100))
            {
                throw new ArgumentException($"Значение должно быть от {min} до {max}", nameof(str));
            }
          return maxTaskCount; 
        }

        private static void ValidateString(string? str)
        {
            if (str == null || str.Length == 0)
            {
                throw new ArgumentException("Введена пустая строка");
            }
        }
}
