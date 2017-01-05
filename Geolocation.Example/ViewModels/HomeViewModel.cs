using System.Collections.Generic;
using System.Web.Mvc;
using Geolocation.Example.Models;

namespace Geolocation.Example.ViewModels
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            Results = new List<ResultModel>();    
        }

        public int SelectedRadius { get; set; }

        public IEnumerable<SelectListItem> Radii
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem {Text = "5 Miles", Value = "5"},
                    new SelectListItem {Text = "10 Miles", Value = "10"},
                    new SelectListItem {Text = "25 Miles", Value = "25"}
                };
            }
        }

        public IEnumerable<ResultModel> Results { get; set; }
    }
}