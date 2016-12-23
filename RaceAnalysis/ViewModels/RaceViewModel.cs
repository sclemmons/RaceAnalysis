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

        private List<int> _selectedSwimMedium;
        public List<int> SelectedSwimMedium
        {
            get
            {
                if (_selectedSwimMedium == null)
                    _selectedSwimMedium = Race.Conditions.SwimMedium.Select(m => m.TagId).ToList();
                return _selectedSwimMedium;
            }
            set
            {
                _selectedSwimMedium = value;
            }
        }
        private List<int> _selectedSwimWeather;
        public List<int> SelectedSwimWeather
        {
            get
            {
                if (_selectedSwimWeather == null)
                    _selectedSwimWeather = Race.Conditions.SwimWeather.Select(m => m.TagId).ToList();
                return _selectedSwimWeather;
            }
            set
            {
                _selectedSwimWeather = value;
            }
        }
        private List<int> _selectedSwimOther;
        public List<int> SelectedSwimOther
        {
            get
            {
                if (_selectedSwimOther == null)
                    _selectedSwimOther = Race.Conditions.SwimOther.Select(m => m.TagId).ToList();
                return _selectedSwimOther;
            }
            set
            {
                _selectedSwimOther = value;
            }
        }


        private List<int> _selectedBikeLayout;
        public List<int> SelectedBikeLayout
        {
            get
            {
                if (_selectedBikeLayout == null)
                    _selectedBikeLayout = Race.Conditions.BikeLayout.Select(m => m.TagId).ToList();
                return _selectedBikeLayout;
            }
            set
            {
                _selectedBikeLayout = value;
            }
        }

        private List<int> _selectedBikeMedium;
        public List<int> SelectedBikeMedium
        {
            get
            {
                if (_selectedBikeMedium == null)
                    _selectedBikeMedium = Race.Conditions.BikeMedium.Select(m => m.TagId).ToList();
                return _selectedBikeMedium;
            }
            set
            {
                _selectedBikeMedium = value;
            }
        }
        private List<int> _selectedBikeWeather;
        public List<int> SelectedBikeWeather
        {
            get
            {
                if (_selectedBikeWeather == null)
                    _selectedBikeWeather = Race.Conditions.BikeWeather.Select(m => m.TagId).ToList();
                return _selectedBikeWeather;
            }
            set
            {
                _selectedBikeWeather = value;
            }
        }
        private List<int> _selectedBikeOther;
        public List<int> SelectedBikeOther
        {
            get
            {
                if (_selectedBikeOther == null)
                    _selectedBikeOther = Race.Conditions.BikeOther.Select(m => m.TagId).ToList();
                return _selectedBikeOther;
            }
            set
            {
                _selectedBikeOther = value;
            }
        }

                
        private List<int> _selectedRunLayout;
        public List<int> SelectedRunLayout
        {
            get
            {
                if (_selectedRunLayout == null)
                    _selectedRunLayout = Race.Conditions.RunLayout.Select(m => m.TagId).ToList();
                return _selectedRunLayout;
            }
            set
            {
                _selectedRunLayout = value;
            }
        }

        private List<int> _selectedRunMedium;
        public List<int> SelectedRunMedium
        {
            get
            {
                if (_selectedRunMedium == null)
                    _selectedRunMedium = Race.Conditions.RunMedium.Select(m => m.TagId).ToList();
                return _selectedRunMedium;
            }
            set
            {
                _selectedRunMedium = value;
            }
        }
        private List<int> _selectedRunWeather;
        public List<int> SelectedRunWeather
        {
            get
            {
                if (_selectedRunWeather == null)
                    _selectedRunWeather = Race.Conditions.RunWeather.Select(m => m.TagId).ToList();
                return _selectedRunWeather;
            }
            set
            {
                _selectedRunWeather = value;
            }
        }
        private List<int> _selectedRunOther;
        public List<int> SelectedRunOther
        {
            get
            {
                if (_selectedRunOther == null)
                    _selectedRunOther = Race.Conditions.RunOther.Select(m => m.TagId).ToList();
                return _selectedRunOther;
            }
            set
            {
                _selectedRunOther = value;
            }
        }



    }
}