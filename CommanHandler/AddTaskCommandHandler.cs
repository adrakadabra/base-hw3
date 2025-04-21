using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;

namespace hw_3;

public class AddTaskCommandHandler : ICommandHandler
{
    private readonly IToDoService _toDoService;
    public string Command => "/addtask";
    
    public AddTaskCommandHandler(IToDoService toDoService)
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
        
        var messageText = update.Message?.Text ?? "";
        var (_, taskName) = TaskHelper.ParsMessageText(messageText);
        if (string.IsNullOrWhiteSpace(taskName))
        {
                botClient.SendMessage(update.Message.Chat, "Не указано название задачи.");
                return;
        }
       
        _toDoService.Add(user, taskName);
        
        botClient.SendMessage(update.Message.Chat, $"Задача \"{taskName}\" добавлена.");
    }

    public void Help(ITelegramBotClient botClient, Update update, ToDoUser? user)
    {
        if (user is null) return; //доступна после авторизации
        botClient.SendMessage(update.Message.Chat, $"/addtask <имя задачи> - добавить новую задач");
    }
}
