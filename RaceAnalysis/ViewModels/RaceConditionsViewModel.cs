using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceAnalysis.Models
{
    public class RaceConditionsViewModel
    {
        public Race Race { get; set; }

        public List<Tag> Tags { get; set; }

        public List<Tag> SwimLayoutTags {
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



        private List<int> _selectedSwimLayout;
        public List<int> SelectedSwimLayout
        {
            get
            {
                if (_selectedSwimLayout == null)
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



        List<WordItem> _swimCloud = null;
        public List<WordItem> SwimCloud
        {
            get
            {
                if (_swimCloud != null)
                    return _swimCloud;

                _swimCloud = new List<WordItem>();
                populateWordCloud(Race.Conditions.SwimLayout,_swimCloud);
                populateWordCloud(Race.Conditions.SwimMedium, _swimCloud);
                populateWordCloud(Race.Conditions.SwimWeather, _swimCloud);
                populateWordCloud(Race.Conditions.SwimOther, _swimCloud);

                return _swimCloud;
            }

        }

        List<WordItem> _bikeCloud = null;
        public List<WordItem> BikeCloud
        {
            get
            {
                if (_bikeCloud != null)
                    return _bikeCloud;

                _bikeCloud = new List<WordItem>();
                populateWordCloud(Race.Conditions.BikeLayout, _bikeCloud);
                populateWordCloud(Race.Conditions.BikeMedium, _bikeCloud);
                populateWordCloud(Race.Conditions.BikeWeather, _bikeCloud);
                populateWordCloud(Race.Conditions.BikeOther, _bikeCloud);

                return _bikeCloud;
            }

        }

        List<WordItem> _runCloud = null;
        public List<WordItem> RunCloud
        {
            get
            {
                if (_runCloud != null)
                    return _runCloud;

                _runCloud = new List<WordItem>();
                populateWordCloud(Race.Conditions.RunLayout, _runCloud);
                populateWordCloud(Race.Conditions.RunMedium, _runCloud);
                populateWordCloud(Race.Conditions.RunWeather, _runCloud);
                populateWordCloud(Race.Conditions.RunOther, _runCloud);

                return _runCloud;
            }

        }

        private void populateWordCloud(List<RaceConditionTag> rcTags,List<WordItem> words)
        {
            foreach (RaceConditionTag rcTag in rcTags)
                words.Add(new WordItem
                {
                    text = rcTag.Tag.Value,
                    weight = rcTag.Count
                });
        }

    }


    public class WordItem
    {
        public string text { get; set; }
        public int weight { get; set; }
    }
}
