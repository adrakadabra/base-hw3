namespace hw_3;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;

public class RemoveTaskCommandHandler : ICommandHandler
{
   private readonly IToDoService _toDoService;
   
   public string Command => "/removetask";

   public RemoveTaskCommandHandler(IToDoService toDoService)
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
       var (_,strNumberTask) = TaskHelper.ParsMessageText(messageText);
       if (!int.TryParse(strNumberTask, out int numberTask))
       {   
         botClient.SendMessage(update.Message.Chat,
             $"\"{strNumberTask}\" - неверный номер задачи");
         return;
       }
       //получаем все активные задачи
       var tasks = _toDoService.GetActiveByUserId(user.UserId);
       int index = numberTask - 1; //задачи пронумерованы с 1
       if (index < 0 || index >= tasks.Count)
       {
            botClient.SendMessage(update.Message.Chat, 
                 $"Задача с  номером {numberTask} не найдена");
            return;
       }
       var task = tasks[index];
       
       _toDoService.Delete(task.Id);
       
       botClient.SendMessage(update.Message.Chat, $"Задача \"{task.Name}\" удалена.");
   }

   public void Help(ITelegramBotClient botClient, Update update, ToDoUser? user)
   {
       if (user is null) return;
       
       botClient.SendMessage(update.Message.Chat, $"/removtask <номер задачи>  - удаляет задачу с указанным номером");
   }
}
