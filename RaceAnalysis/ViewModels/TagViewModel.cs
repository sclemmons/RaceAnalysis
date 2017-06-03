using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceAnalysis.Models
{
    public class TagViewModel : BaseViewModel
    {
        public List<Tag> Tags { get; set; }

        public List<Tag> SwimLayoutTags
        {
            get
            {
                return Tags.Where(t => t.Type == TagType.SwimLayout).ToList();
            }

        }
        public List<Tag> SwimMediumTags
        {
            get
            {
                return Tags.Where(t => t.Type == TagType.SwimMedium).ToList();
            }

        }
        public List<Tag> SwimWeatherTags
        {
            get
            {
                return Tags.Where(t => t.Type == TagType.SwimWeather).ToList();
            }

        }
        public List<Tag> SwimOtherTags
        {
            get
            {
                return Tags.Where(t => t.Type == TagType.SwimOther).ToList();
            }

        }


        public List<Tag> BikeLayoutTags
        {
            get
            {
                return Tags.Where(t => t.Type == TagType.BikeLayout).ToList();
            }

        }
        public List<Tag> BikeMediumTags
        {
            get
            {
                return Tags.Where(t => t.Type == TagType.BikeMedium).ToList();
            }

        }
        public List<Tag> BikeWeatherTags
        {
            get
            {
                return Tags.Where(t => t.Type == TagType.BikeWeather).ToList();
            }

        }
        public List<Tag> BikeOtherTags
        {
            get
            {
                return Tags.Where(t => t.Type == TagType.BikeOther).ToList();
            }

        }

        public List<Tag> RunLayoutTags
        {
            get
            {
                return Tags.Where(t => t.Type == TagType.RunLayout).ToList();
            }

        }
        public List<Tag> RunMediumTags
        {
            get
            {
                return Tags.Where(t => t.Type == TagType.RunMedium).ToList();
            }

        }
        public List<Tag> RunWeatherTags
        {
            get
            {
                return Tags.Where(t => t.Type == TagType.RunWeather).ToList();
            }

        }
        public List<Tag> RunOtherTags
        {
            get
            {
                return Tags.Where(t => t.Type == TagType.RunOther).ToList();
            }

        }



        private List<int> _selectedSwimLayout = new List<int>();
        public List<int> SelectedSwimLayout
        {
            get
            {
               
                return _selectedSwimLayout;
            }
            set
            {
                _selectedSwimLayout = value;
            }
        }

        private List<int> _selectedSwimMedium = new List<int>();
        public List<int> SelectedSwimMedium
        {
            get
            {
               
                return _selectedSwimMedium;
            }
            set
            {
                _selectedSwimMedium = value;
            }
        }
        private List<int> _selectedSwimWeather = new List<int>();
        public List<int> SelectedSwimWeather
        {
            get
            {
             
                return _selectedSwimWeather;
            }
            set
            {
                _selectedSwimWeather = value;
            }
        }
        private List<int> _selectedSwimOther = new List<int>();
        public List<int> SelectedSwimOther
        {
            get
            {
                
                return _selectedSwimOther;
            }
            set
            {
                _selectedSwimOther = value;
            }
        }


        private List<int> _selectedBikeLayout = new List<int>();
        public List<int> SelectedBikeLayout
        {
            get
            {
              
                return _selectedBikeLayout;
            }
            set
            {
                _selectedBikeLayout = value;
            }
        }

        private List<int> _selectedBikeMedium = new List<int>();
        public List<int> SelectedBikeMedium
        {
            get
            {
             
                return _selectedBikeMedium;
            }
            set
            {
                _selectedBikeMedium = value;
            }
        }
        private List<int> _selectedBikeWeather = new List<int>();
        public List<int> SelectedBikeWeather
        {
            get
            {
              
                return _selectedBikeWeather;
            }
            set
            {
                _selectedBikeWeather = value;
            }
        }
        private List<int> _selectedBikeOther = new List<int>();
        public List<int> SelectedBikeOther
        {
            get
            {
               
                return _selectedBikeOther;
            }
            set
            {
                _selectedBikeOther = value;
            }
        }


        private List<int> _selectedRunLayout = new List<int>();
        public List<int> SelectedRunLayout
        {
            get
            {
               
                return _selectedRunLayout;
            }
            set
            {
                _selectedRunLayout = value;
            }
        }

        private List<int> _selectedRunMedium = new List<int>();
        public List<int> SelectedRunMedium
        {
            get
            {
               
                return _selectedRunMedium;
            }
            set
            {
                _selectedRunMedium = value;
            }
        }
        private List<int> _selectedRunWeather = new List<int>();
        public List<int> SelectedRunWeather
        {
            get
            {
               
                return _selectedRunWeather;
            }
            set
            {
                _selectedRunWeather = value;
            }
        }
        private List<int> _selectedRunOther = new List<int>();
        public List<int> SelectedRunOther
        {
            get
            {
               
                return _selectedRunOther;
            }
            set
            {
                _selectedRunOther = value;
            }
        }


    }
}