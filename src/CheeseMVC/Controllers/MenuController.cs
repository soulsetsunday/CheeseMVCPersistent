using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        private CheeseDbContext context;

        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            List<Menu> menus = context.Menus.ToList();

            return View(menus);
        }

        public IActionResult Add()
        {
            AddMenuViewModel newMenu = new AddMenuViewModel();
            return View(newMenu);
        }

        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (ModelState.IsValid)
            {
                Menu newMenu = new Menu
                {
                    Name = addMenuViewModel.Name
                };

                context.Menus.Add(newMenu);
                context.SaveChanges();

                return Redirect("/Menu/ViewMenu/" + newMenu.ID);
            }
            return View(addMenuViewModel);
        }

        //url like /Menu/ViewMenu/5
        public IActionResult ViewMenu(int id)
        {
            //Cheese theCheese = context.Cheeses.Single(c => c.ID == cheeseId);
            Menu viewMenu = context.Menus.Single(c => c.ID == id);

            List<CheeseMenu> items = context
        .CheeseMenus
        .Include(item => item.Cheese)
        .Where(cm => cm.MenuID == id)
        .ToList();

            ViewMenuViewModel viewModel = new ViewMenuViewModel
            {
                Menu = viewMenu,
                Items = items
            };

            return View(viewModel);
        }

        public IActionResult AddItem(int id)
        {
            //List<Cheese> cheeses = context.Cheeses.Include(c => c.Category).ToList();
            List<Cheese> cheeses = context.Cheeses.ToList();
            //Menu passedMenu = context.Menus.Single(m => m.ID == menuId);
            Menu viewMenu = context.Menus.Single(c => c.ID == id);
            //IList<Cheese> cheeses = context.Cheeses.Include(c => c.Category).ToList();

            //AddMenuItemViewModel viewMenu = new AddMenuItemViewModel(passedMenu, context.Cheeses.Include(c => c.Category).ToList());
            AddMenuItemViewModel passToViewMenu = new AddMenuItemViewModel(viewMenu, cheeses);
            return View(passToViewMenu);
            //return View(new AddMenuItemViewModel(viewMenu, cheeses));
        }

        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel addMenuItemViewModel)
        {
            if (ModelState.IsValid)
            {
                var cheeseID = addMenuItemViewModel.CheeseID;
                var menuID = addMenuItemViewModel.MenuID;

                IList<CheeseMenu> existingItems = context.CheeseMenus
                        .Where(cm => cm.CheeseID == cheeseID)
                        .Where(cm => cm.MenuID == menuID).ToList();

                if (existingItems.Count==0)
                {
                    CheeseMenu menuItem = new CheeseMenu
                    {
                        //Cheese = context.Cheeses.Single(c => c.ID == cheeseID),
                        //Menu = context.Menus.Single(m => m.ID == menuID)
                        CheeseID = addMenuItemViewModel.CheeseID,
                        MenuID = addMenuItemViewModel.MenuID
                    };

                    context.CheeseMenus.Add(menuItem);
                    context.SaveChanges();
                }


                return Redirect($"/Menu/ViewMenu/{addMenuItemViewModel.MenuID}");
            }

            return View(addMenuItemViewModel);
        }
    }
}
