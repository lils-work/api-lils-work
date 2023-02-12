namespace LilsWorkApi.Models
{
    public class Task
    {
        // 必须添加 Id，否则迁移时会提示没有主键
        public int Id { get; set; }
        public string? Title { get; set; }
        public bool IsCompleted { get; set; } = false;
        /// <summary>
        /// 任务是否取消；
        /// 当任务取消时，IsCompleted 无效
        /// </summary>
        public bool IsCancel { get; set; } = false;
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? DueTo { get; set; }
    }
}
