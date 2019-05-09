using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MenusSolution.Models
{
    //[Table("vNavigationList")]
    public class NavigationList
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public int Sequence { get; set; }
        public string StoredType { get; set; }
        public string Owner { get; set; }
        public string DefaultName { get; set; }
        public string DefaultDescription { get; set; }
        public string Source { get; set; }
        public string ImageUrl { get; set; }
        public string LargeImageUrl { get; set; }
        public string IsMobile { get; set; }
        public string DocumentMode { get; set; }

    }
}
