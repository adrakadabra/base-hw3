namespace hw_3;

public class UserService : IUserService
{
    //словарь инициализуем 1 раз, дальше ссылку изменить нельзя
    private readonly Dictionary<long, ToDoUser> _users = new();
    public ToDoUser RegisterUser(long telegramUserId, string telegramUserName)
    {
        if (_users.TryGetValue(telegramUserId, out var existingUser))
        {
            return existingUser;  //нашли по telegramUserId и вернули полльзователя 
        }
        // создаем нового пользователя
        var newUser = new ToDoUser
        {
            UserId = Guid.NewGuid(),
            TelegramUserId = telegramUserId,
            TelegramUserName = telegramUserName,
            RegisteredAt = DateTime.UtcNow
        };
        //добавляем в словарь нового пользователя
        _users[telegramUserId] = newUser;
        return newUser;
    }
    
    public ToDoUser? GetUser(long telegramUserId)
    {
        //возрващаем найденного пользователя, если пользователя нет в словаре - null
        return _users.TryGetValue(telegramUserId, out var user) ? user : null;
    } 
}