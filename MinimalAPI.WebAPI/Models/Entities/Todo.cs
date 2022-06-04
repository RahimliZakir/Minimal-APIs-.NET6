namespace MinimalAPI.WebAPI.Models.Entities
{
    public class Todo
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public bool IsCompleted { get; set; }
    }
}
