namespace hw_3;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;

public class ShowAllTasksCommandHandler : ICommandHandler
{
    private readonly IToDoService _toDoService;
    public string Command => "/showalltasks";

    public ShowAllTasksCommandHandler(IToDoService toDoService)
    {
        _toDoService = toDoService;
    }

    public void Handle(ITelegramBotClient botClient, Update update, ToDoUser? user)
    {
        if (user is null) //доступна после авторизации
        {
            botClient.SendMessage(update.Message.Chat, $"Неизвестная команда");
            return;
        }

        var tasks = _toDoService.GetAllByUserId(user.UserId);
        if (tasks.Count == 0)
        {
            botClient.SendMessage(update.Message.Chat, "Список задач пуст.");
            return;
        }

        var taskListComplet = tasks.Where(item => item.State == ToDoItemState.Completed);
        //botClient.SendMessage(update.Message.Chat, $"Список задач:");
        foreach (var item in taskListComplet)
        {
            botClient.SendMessage(update.Message.Chat, GetTaskView(item, 0));
        }

        var taskListActive = tasks.Where(item => item.State == ToDoItemState.Active);
        int i = 1;
        foreach (var item in taskListActive)
        {
            botClient.SendMessage(update.Message.Chat, GetTaskView(item, i));
            i++;
        }
    }

    public void Help(ITelegramBotClient botClient, Update update, ToDoUser? user)
    {
        if (user is null) return; //доступна после авторизации
        botClient.SendMessage(update.Message.Chat, $"/showalltasks - отображает список выполненных и активных задач");
    }

    private string GetTaskView(ToDoItem item, int number)
    {
        string task = ((number == 0) ? "[x] " : $"{number}.") + 
                       $" ({item.State.ToString()}) {item.Name} - " +
                       item.CreatedAt.ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss") + $" - {item.Id}";
        return task;
    }
}