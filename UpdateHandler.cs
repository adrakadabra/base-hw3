using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;
using System.Text.Json;

namespace hw_3;

public class UpdateHandler : IUpdateHandler
{
    private readonly IUserService _userService;
    private readonly IToDoService _toDoService;
    private readonly Dictionary<string, ICommandHandler> _handlers;
    private ToDoUser? _toDoUser;
    
    public UpdateHandler (IUserService? userService, IToDoService toDoService)
    {
        _userService = userService;
        _toDoService = toDoService;
        
        //создаем список команд и преобразуем в словарь, ключ название команды
        _handlers  = new List<ICommandHandler>
        {
            new StartCommandHandler(_userService),
            new InfoCommandHandler(_toDoService),
            new AddTaskCommandHandler(_toDoService),
            new ShowTasksCommandHandler(_toDoService),
            new ShowAllTasksCommandHandler(_toDoService),
            new CompleteTaskCommandHandler(_toDoService),
            new RemoveTaskCommandHandler(_toDoService)
        }.ToDictionary(h => h.Command);
        //добавим команду help 
        var helpCommanHandler = new HelpCommandHandler(_toDoService);
        helpCommanHandler.Handlers = _handlers;
        _handlers.Add(helpCommanHandler.Command, helpCommanHandler);
    }

    public void HandleUpdateAsync(ITelegramBotClient botClient, Update update)
    {
        try
        {
            string st = JsonSerializer.Serialize(update);

            var (command, args) = ParsMessageText(update.Message?.Text);
            if (string.IsNullOrEmpty(command))
            {
                botClient.SendMessage(update.Message.Chat, $"Неизвестная команда. /help - доступные команды");
                return;
            }

            var user = _userService.GetUser(update.Message.From.Id);
            //проверяем команду и в  handler получаем объект соответсвующий команде
            //для команды /start - StartCommandHandler
            if (!_handlers.TryGetValue(command, out var handler))
            {
                botClient.SendMessage(update.Message.Chat, $"Команда '{command}' не поддерживается.");
                return;
            }
            handler.Handle(botClient, update, user);
        }
        catch (Exception ex)
        {
            botClient.SendMessage(update.Message.Chat, ex.Message);
        }
    }
    /// <summary>
    /// Разбивает строку на две части, первая часть до первого пробела, вторая часть - вся остальная строка
    /// </summary>
    /// <param name="messageText"> Исходная строка</param>
    /// <returns>Кортеж команда, значение</returns>
    private static (string Command, string Text) ParsMessageText (string messageText)
    {
        //первый параметр команда 
        var parts = messageText.Split(new[] { ' ' }, 2);
        var command = parts[0];
        var text = parts.Length > 1 ? parts[1] : string.Empty;
        return (command, text);
    }
}