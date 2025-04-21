namespace hw_3;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;

public class HelpCommandHandler : ICommandHandler
{
    private readonly IToDoService _toDoService;
    
    public string Command => "/help";
    
    public Dictionary<string, ICommandHandler> Handlers { get; set; }
    
    public HelpCommandHandler(IToDoService toDoService)
    {
        _toDoService = toDoService;
    }

    public void Handle(ITelegramBotClient botClient, Update update, ToDoUser? user)
    { 
        botClient.SendMessage(update.Message.Chat, $"Список доступных команд:");
        foreach(var handler in Handlers)
        {
            handler.Value.Help(botClient,update, user);
        }
    }

    public  void Help (ITelegramBotClient botClient,Update update,ToDoUser? user)
    {
        botClient.SendMessage(update.Message.Chat, $"/help - вызов справки");
    }
}


//               """;
// Console.WriteLine(text);