public class Task
{
    // 必须添加 Id，否则迁移时会提示没有主键
    public int Id { get; set; }
    public string? Title { get; set; }
    public TaskState State { get; set; } = TaskState.Todo;
}
