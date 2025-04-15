using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;
using System.Text.Json;

namespace hw_3;

public class UpdateHandler : IUpdateHandler
{
    private readonly IUserService _userService;
    //private readonly IToDoService _toDoService;
    private ToDoUser? _toDoUser;

    public IToDoService ToDoService { get; init; }
    public UpdateHandler (IUserService userService)
    {
        _userService = userService;
        ToDoService = new ToDoService();
    }
    public void HandleUpdateAsync(ITelegramBotClient botClient, Update update)
    {
        string st = JsonSerializer.Serialize(update);
        //Console.WriteLine(st);

        if (update.Message?.Text == null)
            return;

        var parsMessageText = ParsMessageText(update.Message?.Text);
        
        switch (parsMessageText.Command)
        {
            case "/start":
               _toDoUser = HandleStartCommand(botClient, update);
                //tClient.SendMessage(update.Message.Chat, $"Получил'{command}'");
                break;
            case "/addtask":
                AddTask(ToDoService,_toDoUser, parsMessageText.Text);
                //IToDoService s = new ToDoService();
                //ToDoService.Add(_toDoUser,parsMessageText.Text);
                //var list = ToDoService.GetAllByUserId(_toDoUser.UserId);
                //foreach (var task in list)
                //    Console.WriteLine(task.Name);
                break;
            case "/removetask":
                RemoveTask(botClient, update,ToDoService,_toDoUser.UserId,parsMessageText.Text);
                break;
            case "/showtask":
                ShowTasks(botClient, update, ToDoService, _toDoUser.UserId);
                break;
            case "/showalltasks":
                ShowAllTasks(botClient, update, ToDoService, _toDoUser.UserId);
                break;
            case "/completetask":
                break;
        }

       
//        botClient.SendMessage(update.Message.Chat, $"Получил eeeeeee'{update.Message.Text}'");
    }
    private ToDoUser? HandleStartCommand(ITelegramBotClient botClient, Update update)
    {
        if (update.Message.From == null)
        {
            botClient.SendMessage(update.Message.Chat, "Не удалось получить информацию о пользователе.");
            return null;
        }
        var user = _userService.GetUser(update.Message.From.Id);
        if (user is not null)
        {
            botClient.SendMessage(
                update.Message.Chat,
                $"Добро пожаловать, {user.TelegramUserName}!");
            return user;
        }
        else
        {
            var newuser = _userService.RegisterUser(
                update.Message.From.Id,
                update.Message.From.Username ?? string.Empty);

            botClient.SendMessage(
                update.Message.Chat,
                $"Добро пожаловать, {newuser.TelegramUserName}! Вы зарегистрированы с ID: {newuser.UserId}");
            return newuser; 
        }
    }
    
    private static (string Command, string Text) ParsMessageText (string MessageText)
    {
        var validCommands = new List<string>
        {
            "/start", "/help", "/exit",
            "/addtask", "/showtask", "/removetask"
        };
        //первый параметр команда 
        string[] parts = MessageText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        string command = parts.Length > 0 ? parts[0] : string.Empty;
        string[] args = parts.Skip(1).ToArray();
        string value = string.Join(" ", args);

        if (validCommands.Contains(command))
        {
            return (command, value);
        }
        else
        {
            return (string.Empty,MessageText);
        }
    }

    private static void AddTask(IToDoService toDoService, ToDoUser toDoUser, string textMessage)
    {
        toDoService.Add(toDoUser,textMessage);
    }
    private static string GetTaskView(ToDoItem toDoItem, int number)
    {
        string stnumber = number == 0 ? "x" : number.ToString();             
        return $"[{stnumber}] ({toDoItem.State.ToString()}) {toDoItem.Name} - " +
               toDoItem.CreatedAt.ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss") +
               $" - {toDoItem.Id}";
    }

    private static void ShowTasks(ITelegramBotClient botClient, Update update, 
                                    IToDoService toDoService,  Guid userId )
    {
        var taskList = toDoService.GetActiveByUserId(userId);
        if (taskList.Count == 0)
        {
            botClient.SendMessage(update.Message.Chat, $"Список задач пуст");
            return;
        }
        botClient.SendMessage(update.Message.Chat, $"Список задач:");
        for (int i = 0; i < taskList.Count; i++)
        {
            botClient.SendMessage(
                update.Message.Chat,
                $"{i + 1}. ({taskList[i].State.ToString()}) {taskList[i].Name} - " +
                taskList[i].CreatedAt.ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss") +
                $" - {taskList[i].Id}");
        }
    }
    
    private static void ShowAllTasks(ITelegramBotClient botClient, Update update,
        IToDoService toDoService, Guid userId)
    {
        var taskList = toDoService.GetAllByUserId(userId);
        if (taskList.Count == 0)
        {
            botClient.SendMessage(update.Message.Chat, $"Список задач пуст");
            return;
        }

        var taskListActive = taskList.Where(item => item.State == ToDoItemState.Active);
        var taskListComplet = taskList.Where(item => item.State == ToDoItemState.Completed);
        botClient.SendMessage(update.Message.Chat, $"Список задач:");
        foreach (var item in taskListComplet)
        {
            botClient.SendMessage(update.Message.Chat, GetTaskView(item, 0));
        }
        int i = 1;
        foreach (var item in taskListActive)
        {
            botClient.SendMessage(update.Message.Chat, GetTaskView(item, i));
            i++;
        }
    }
    
    private static void RemoveTask (ITelegramBotClient botClient,  Update update,
                                IToDoService toDoService, Guid userId, string messageText)
    {
        if (!int.TryParse(messageText, out int numberTask))
        {   
            botClient.SendMessage(update.Message.Chat,
                $"Номер задачи {messageText} не корректен");
            return;
        }
        
        var taskList = toDoService.GetActiveByUserId(userId);
        int index = numberTask - 1; //задачи пронумерованы с 1
        if (index < 0 || index >= taskList.Count)
        {
            botClient.SendMessage(update.Message.Chat, 
                $"Задачи с  номером {numberTask} нет в списке");
            return;
        }
        
        toDoService.Delete(taskList[index].Id);
        botClient.SendMessage(update.Message.Chat,
                $"Задача {numberTask} - {taskList[index].Id}  удалена");
    }
}