namespace hw_3;

public interface IUserService
{
    ToDoUser RegisterUser(long telegramUserId, string telegramUserName);
    ToDoUser? GetUser(long telegramUserId);
} 