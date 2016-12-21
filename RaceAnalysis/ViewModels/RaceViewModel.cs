using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceAnalysis.Models
{
    public class RaceViewModel
    {
        public Race Race { get; set; }

        public List<Tag> Tags{get;set;}


        private List<int> _selectedSwimLayout;
        public List<int> SelectedSwimLayout
        {
            get
            {
               if(_selectedSwimLayout == null)
                   _selectedSwimLayout = Race.Conditions.SwimLayout.Select(m => m.TagId).ToList();
                return _selectedSwimLayout;
            }
            set
            {
               _selectedSwimLayout = value;
            }
        }
        public List<Tag> SelectedBikeLayout { get; set; }
        public List<Tag> SelectedRunLayout { get; set; }
            
            
    }
}