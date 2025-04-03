/*
Описание/Пошаговая инструкция выполнения домашнего задания:
Создайте новую команду /addtask
Пользователь сможет добавлять задачи в список.
После ввода команды /addtask, бот должен попросить ввести описание задачи.
Сохраните задачу в список (или массив) и отобразите сообщение о том, что задача добавлена.

Создайте новую команду /showtasks
При вводе команды /showtasks бот должен отобразить список всех добавленных задач.
Если задачи ещё не добавлены, необходимо вывести сообщение о том, что список пуст.

Создайте новую команду /removetask
Бот должен позволить пользователю удалять задачи по номеру в списке.
После ввода команды /removetask, бот должен отобразить список задач с номерами.
Затем бот должен запросить у пользователя номер задачи для удаления и удалить выбранную задачу из списка.

Модифицируйте команду /help
Обновите команду /help, добавив к ней описание новых команд: /addtask, /showtasks и /removetask.

Реализуйте обработку ошибок
Если пользователь пытается удалить задачу, когда список пуст, программа должна уведомить его об этом.
Также, если введён неверный номер задачи при удалении, бот должен уведомить об этом и попросить ввести корректный номер.
 */

namespace hw_3;

class Program
{
    private static string? _userName  = "";
    private static bool _startComplete; 
    private static List<string> _task = new List<string>(); 

    static void Main()
    {
        RunBot();
    }
    
     private static void RunBot()
        {
            PrintCommandWelcome();
            PrintCommandList();
            do
            {
                Console.Write(">");
                string? str = Console.ReadLine();
                if (string.IsNullOrEmpty(str))
                {
                    Console.WriteLine("Команда не распознана.");
                    PrintCommandList(_startComplete);
                    continue;
                }
                
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
            Console.Write("Введите описание задачи: ");
            //Console.Write(">");
            string? task = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(task))
            {
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
}
