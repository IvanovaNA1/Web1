namespace Web1.Models.HeaderModels
{
    public static class ListData
    {
        public static List<MenuItem> menuItems = new List<MenuItem>
        {
            new MenuItem { Controller = "Home", Action = "Flowers", Text = "ЦВЕТЫ" },
            new MenuItem { Controller = "Home", Action = "Services", Text = "УСЛУГИ" },
            new MenuItem { Controller = "Home", Action = "About", Text = "ДЛЯ ПОКУПАТЕЛЕЙ" }
        };

    }
}
