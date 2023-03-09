namespace ZamaraWebApp.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int UserId { get; set; }
        public List<string> Tags { get; set; }
        public int Reactions { get; set; }
    }

    public class Root
    {
        public List<Post> Posts { get; set; }
        public int Total { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
    }
}
