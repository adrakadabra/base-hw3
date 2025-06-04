namespace hw_3;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;

public class InfoCommandHandler : ICommandHandler
{
    private readonly IToDoService _toDoService;
    
    public string Command => "/info";
    
    public InfoCommandHandler(IToDoService toDoService)
    {
        _toDoService = toDoService;
    }

    public void Handle(ITelegramBotClient botClient, Update update, ToDoUser? user)
    {
        string info = "Чат бот \"Список задач\". Версия 0.2";
        botClient.SendMessage(update.Message.Chat, info);
    }

    public  void Help (ITelegramBotClient botClient,Update update,ToDoUser? user)
    {
        botClient.SendMessage(update.Message.Chat, $"/info - информация о программе");
    }
}