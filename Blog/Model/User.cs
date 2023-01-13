using System.Text.Json.Serialization;

namespace introducao.Model
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; } = null!;
        public string? Email { get; set; } = null!;
        [JsonIgnore]
        public string? PasswordHash { get; set; } = null!;
        public string? Image { get; set; } = null!;
        public string? Slug { get; set; } = null!;
        public string? Bio { get; set; } = null!;
        // public string? github { get; set; }

        public IEnumerable<Post> Posts { get; set; } = new List<Post>();
        public IEnumerable<Role> Roles { get; set; } = new List<Role>();
    }
}