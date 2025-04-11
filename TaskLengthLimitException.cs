namespace hw_3;

public class TaskLengthLimitException : Exception
{
    public TaskLengthLimitException(int taskLenght, int taskCountLimit)
        : base(string.Format($"Длина задачи {taskLenght}  превышает допустимое значение {taskCountLimit}"))
    {
    }        
}