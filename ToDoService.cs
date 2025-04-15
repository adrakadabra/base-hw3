namespace hw_3;

public class ToDoService : IToDoService
{
    private readonly Dictionary<Guid, List<ToDoItem>> _toDoItems = new();
    private readonly Dictionary<Guid, ToDoItem> _toDoItemById =new();  //для поиска по ид задачи

    public int MaxTaskCount { get; set; }
    public int LenghtTaskLimit {get; set;}
    
    public IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId)
    {
        if (_toDoItems.TryGetValue(userId, out var items))
        {
            return items.AsReadOnly(); // Возвращаем ReadOnly коллекцию
        }

        return new List<ToDoItem>().AsReadOnly();
    }

    public IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId)
    {
        if (_toDoItems.TryGetValue(userId, out var items))
        {
            return items.Where(item => item.State == ToDoItemState.Active).ToList().AsReadOnly();
        }

        return new List<ToDoItem>().AsReadOnly(); //возращаем пустой список если нет списка по ид пользователя 
    }

    public ToDoItem Add(ToDoUser user, string name)
    {
        var toDoItem = new ToDoItem(name, user);
        var userId = user.UserId;
        if (_toDoItems.TryGetValue(user.UserId, out var items)) //есть ид пользователя 
        {
            items.Add(toDoItem);
        }
        else
        {
            var item = new List<ToDoItem> { toDoItem };
            _toDoItems[userId] = item;
        }
        //второй словарь для быстрого поиска _toDoItemById
        _toDoItemById[toDoItem.Id] = toDoItem; 
        
        return toDoItem;
    }

    public void MarkCompleted(Guid id)
    {
        if (_toDoItemById.TryGetValue(id, out var item))
        {
            item.State = ToDoItemState.Completed;
        }
    }

    public void Delete(Guid id)
    {
        foreach (var (userId, toDoItemList) in _toDoItems)
        {
            for (int i = 0; i < toDoItemList.Count; i++)
            {
                if (toDoItemList[i].Id == id)
                {
                    toDoItemList.RemoveAt(i);
                    _toDoItemById.Remove(id);
                    return;
                }
            }
        }
    }
}
    
    