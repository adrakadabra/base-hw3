using Otus.ToDoList.ConsoleBot.Types;

namespace hw_3;

public class ToDoItem
{
    public Guid Id { get; set; }
    public ToDoUser User { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public ToDoItemState State { get; set; }
    public DateTime? StateChangedAt  { get; set; }

    public ToDoItem(string? name, ToDoUser user)
    {
        Id = Guid.NewGuid();
        User = user;
        Name = name;
        CreatedAt = DateTime.UtcNow;
        State = ToDoItemState.Active;
    }
}