using System;
using System.Collections.Generic;
using System.Text;

namespace MapsLive.Models
{
    public enum MenuItemType
    {        
        Browse,
        Mapas,
        About        
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
