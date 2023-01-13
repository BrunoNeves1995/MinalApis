using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.ViewModels.Pots
{
    public class ListPotsViewModel
    {   
        public int Id { get; set; }
        public string? Title { get; set; } = null!;
        public string? Slug { get; set; } = null!;
        public DateTime LastUpdateDate { get; set; }
        public string? Category { get; set; } = null!;
        public string? User { get; set; } = null!;
        
    }
}