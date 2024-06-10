using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Web1.Models.HeaderModels
{
    public static class ListData
    {
        
        public static List<MenuItem> ClientMenuItems = new List<MenuItem>
        {
            new MenuItem { Controller = "Home", Action = "Flowers", Text = "ЦВЕТЫ" },
            new MenuItem { Controller = "Home", Action = "Services", Text = "УСЛУГИ" },
            new MenuItem { Controller = "Home", Action = "About", Text = "ДЛЯ ПОКУПАТЕЛЕЙ" }
        };
        public static List<MenuItem> AdminMenuItems = new List<MenuItem>
        {
            new MenuItem { Controller = "Home", Action = "Flowers", Text = "ЦВЕТЫ И УСЛУГИ" },
            new MenuItem { Controller = "Admin", Action = "Team", Text = "КОМАНДА" },
            new MenuItem { Controller = "Admin", Action = "Shipments", Text = "ПОСТАВКИ" }
        };
        public static List<MenuItem> EmployeeMenuItems = new List<MenuItem>
        {
            new MenuItem { Controller = "Home", Action = "Flowers", Text = "ЦВЕТЫ" },
            new MenuItem { Controller = "Home", Action = "Orders", Text = "ЗАКАЗЫ" }
        };
    }
}
