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
                    new SelectListItem {Text = "5", Value = "5"},
                    new SelectListItem {Text = "10", Value = "10"},
                    new SelectListItem {Text = "25", Value = "25"},
                    new SelectListItem {Text = "100", Value = "100"},
                    new SelectListItem {Text = "500", Value = "500"}
                };
            }
        }

        public DistanceUnit SelectedDistanceUnit { get; set; }

        public IEnumerable<ResultModel> Results { get; set; }
    }
}