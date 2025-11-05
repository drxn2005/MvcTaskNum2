namespace MvcTaskNum2.Models
{
    public class TodoTask
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime DeadLine { get; set; }
        public string? FilePath { get; set; }
    }
}
