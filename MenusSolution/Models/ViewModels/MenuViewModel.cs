using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenusSolution.Models.ViewModels
{
    public class MenuViewModel
    {
        public string ID { get; set; }
        public string Content { get; set; }
        public string IconClass { get; set; }
        public string Href { get; set; }
        public IList<MenuViewModel> Children { get; set; }
    }
}
