using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace SurfsUp.Models
{
    public class BoardTypeViewModel
    {
        public List<Board>? Boards { get; set; }
        public SelectList? Types { get; set; }
        public string? BoardType { get; set; }
        public string? SearchString { get; set; }

    }
}
