namespace introducao.Model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        
        public IList<Post> Posts { get; set; } = null!;
    }
}