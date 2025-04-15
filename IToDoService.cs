namespace hw_3;

public interface IToDoService
{
    public int MaxTaskCount { get; set; }
    public int LenghtTaskLimit {get; set;}
    
    IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId);
    //Возвращает ToDoItem для UserId со статусом Active
    IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId);
    ToDoItem Add(ToDoUser user, string name);
 
    void MarkCompleted(Guid id);
    
    void Delete(Guid id);
}