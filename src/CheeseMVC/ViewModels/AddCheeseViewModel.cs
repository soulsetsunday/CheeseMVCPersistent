using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseMVC.ViewModels
{
    public class AddCheeseViewModel
    {
        [Required]
        [Display(Name = "Cheese Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must give your cheese a description")]
        public string Description { get; set; }

        //public CheeseType Type { get; set; }

        //public List<SelectListItem> CheeseTypes { get; set; }
        [Required]
        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        public List<SelectListItem> Categories { get; set; }

        public AddCheeseViewModel() { }

        public AddCheeseViewModel(IEnumerable<CheeseCategory> categories) {

            //CheeseTypes = new List<SelectListItem>();

            Categories = new List<SelectListItem>();

            foreach (var cat in categories)
            {
                Categories.Add(new SelectListItem { 
                
                    Value = cat.ID.ToString(),
                    Text = cat.Name

                });
            }
            // <option value="0">Hard</option>
            //CheeseTypes.Add(new SelectListItem {
            //    Value = ((int) CheeseType.Hard).ToString(),
            //    Text = CheeseType.Hard.ToString()
            //});
            

        }
    }
}
