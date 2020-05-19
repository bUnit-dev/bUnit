namespace SampleApp.Data
{
    public class Todo
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;

        public static readonly Todo Empty = new Todo();
    }
}
