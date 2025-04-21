namespace hw_3;

public static class TaskHelper
{
    public static (string Command, string Text) ParsMessageText (string messageText)
    {
        //первый параметр команда, остальная строка текст сообщения
        var parts = messageText.Split(new[] { ' ' }, 2);
        var command = parts[0];
        var text = parts.Length > 1 ? parts[1] : string.Empty;
        return (command, text);
    }
}