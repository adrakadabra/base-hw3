using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;

namespace hw_3;

public class StartCommandHandler : ICommandHandler
{
    private readonly IUserService _userService;
    
    public string Command => "/start";

    public StartCommandHandler(IUserService userService) //конструктор
    {
        _userService = userService;
    }

    public void Handle(ITelegramBotClient botClient, Update update, ToDoUser user)
    {
        var existingUser = _userService.GetUser(update.Message.From.Id);
        if (existingUser != null)
        {
            botClient.SendMessage(update.Message.Chat, $"Добро пожаловать, {existingUser.TelegramUserName}!");
        }
        else
        { 
            //регистрируем нового пользователя
            var newUser = _userService.RegisterUser(update.Message.From.Id, update.Message.From.Username ?? "");
            botClient.SendMessage(update.Message.Chat, $"Добро пожаловать, {newUser.TelegramUserName}! Вы зарегистрированы.");
        }
    }
    
    public  void Help (ITelegramBotClient botClient,Update update, ToDoUser? user)
    {
        botClient.SendMessage(update.Message.Chat, $"/start - начало работы с ботом");
    }
}

// private ToDoUser? HandleStartCommand(ITelegramBotClient botClient, Update update)
// {
//     if (update.Message.From == null)
//     {
//         botClient.SendMessage(update.Message.Chat, "Не удалось получить информацию о пользователе.");
//         return null;
//     }
//     var user = _userService.GetUser(update.Message.From.Id);
//     if (user is not null)
//     {
//         botClient.SendMessage(
//             update.Message.Chat,
//             $"Добро пожаловать, {user.TelegramUserName}!");
//         return user;
//     }
//     else
//     {
//         var newuser = _userService.RegisterUser(
//             update.Message.From.Id,
//             update.Message.From.Username ?? string.Empty);
//
//         botClient.SendMessage(
//             update.Message.Chat,
//             $"Добро пожаловать, {newuser.TelegramUserName}! Вы зарегистрированы с ID: {newuser.UserId}");
//         return newuser; 
//     }
// }