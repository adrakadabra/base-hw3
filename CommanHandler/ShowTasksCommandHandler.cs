namespace hw_3;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;

public class ShowTasksCommandHandler : ICommandHandler
{
    private readonly IToDoService _toDoService;
    public string Command => "/showtasks";
    
    public ShowTasksCommandHandler(IToDoService toDoService)
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

        var tasks = _toDoService.GetActiveByUserId(user.UserId);
        if (tasks.Count == 0)
        {
            botClient.SendMessage(update.Message.Chat, "Список задач пуст.");
            return;
        }

        for (int i = 0; i < tasks.Count; i++)
        {
            var task = tasks[i];
            botClient.SendMessage(
                update.Message.Chat,
                $"{i + 1}. {task.Name} - " +
                task.CreatedAt.ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss") +
                $" - {task.Id}");
        }
    }

    public void Help(ITelegramBotClient botClient, Update update, ToDoUser? user)
    {
        if (user is null) return; //доступна после авторизации
        botClient.SendMessage(update.Message.Chat, $"/showtasks - отображает список активных задач");
    }
}