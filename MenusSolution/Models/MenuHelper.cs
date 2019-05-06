using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Data.SqlClient;

namespace MenusSolution.Models
{
    public class MenuHelper
    {
        private static string connStr =$"DataSource ={ Path.Combine(Directory.GetCurrentDirectory(),"demo.db")}";        
        public static IList<Menu> GetAllMenuItems()
        {
            Console.WriteLine(connStr);
            using (IDbConnection conn = new SqliteConnection(connStr))
            {
                try
                {
                    conn.Open();
                    var sql = @"SELECT * FROM menu";
                    return conn.Query<Menu>(sql).ToList();
                }
                catch (Exception ex)
                {
                    return new List<Menu>();
                }
            }
        }

        public static IList<Menu> GetAllMenuItemsMS()
        {
            List<Menu> menuList = new List<Menu>();

            using (SqlConnection cn = new SqlConnection("Server=DESKTOP-UKJJB1E\\WINSQL;Database=TestMenu;User Id=test;password=Test1663$!;Trusted_Connection=False;MultipleActiveResultSets=true;"))
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [TestMenu].[dbo].[Menu]", cn);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    Menu menu = new Menu();
                    menu.ID = (string)reader["ID"];
                    menu.ParentID = (reader["ParentID"] == DBNull.Value) ? null : reader["ParentID"].ToString();
                    menu.Content = (string)reader["Content"];
                    menu.IconClass = (string)reader["IconClass"];
                    menu.Href = (string)reader["Url"];

                    menuList.Add(menu);
                }
                cn.Close();
                return menuList;
            }
        }

        public static IList<Menu> GetChildrenMenu(IList<Menu> menuList, string parentId = null)
        {
            return menuList.Where(x => x.ParentID == parentId).OrderBy(x => x.Order).ToList();
        }

        public static Menu GetMenuItem(IList<Menu> menuList, string id)
        {
            return menuList.FirstOrDefault(x => x.ID == id);
        }

        public static IList<MenuViewModel> GetMenu(IList<Menu> menuList, string parentId)
        {
            var children = MenuHelper.GetChildrenMenu(menuList, parentId);

            if (!children.Any())
            {
                return new List<MenuViewModel>();
            }

            var vmList = new List<MenuViewModel>();
            foreach (var item in children)
            {
                var menu = MenuHelper.GetMenuItem(menuList, item.ID);

                var vm = new MenuViewModel();

                vm.ID = menu.ID;
                vm.Content = menu.Content;
                vm.IconClass = menu.IconClass;
                vm.Href = menu.Href;
                vm.Children = GetMenu(menuList, menu.ID);

                vmList.Add(vm);
            }

            return vmList;
        }

    }
}
