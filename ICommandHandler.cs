using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;

namespace hw_3;

public interface ICommandHandler
{
    string Command { get; }
    
    public void Handle(ITelegramBotClient botClient, Update update, ToDoUser user);
    
    public void Help (ITelegramBotClient botClient, Update update, ToDoUser? user);
}