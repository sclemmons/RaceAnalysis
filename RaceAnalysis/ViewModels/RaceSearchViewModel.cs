using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceAnalysis.Models
{
    public class RaceSearchViewModel : BaseViewModel
    {
        public List<Tag> Tags { get; set; }

        public List<Tag> SwimTags
        {
            get
            {
                return Tags.Where(t => t.Type == TagType.SwimLayout ||
                                    t.Type == TagType.SwimMedium ||
                                    t.Type == TagType.SwimWeather ||
                                    t.Type == TagType.SwimOther
                                    ).ToList();
            }

        }
        public List<Tag> BikeTags
        {
            get
            {
                return Tags.Where(t => t.Type == TagType.BikeLayout ||
                                    t.Type == TagType.BikeMedium ||
                                    t.Type == TagType.BikeWeather ||
                                    t.Type == TagType.BikeOther
                                    ).ToList();
            }

        }
        public List<Tag> RunTags
        {
            get
            {
                return Tags.Where(t => t.Type == TagType.RunLayout ||
                                    t.Type == TagType.RunMedium ||
                                    t.Type == TagType.RunWeather ||
                                    t.Type == TagType.RunOther
                                    ).ToList();
            }

        }


        private List<int> _selectedSwimTags = new List<int>();
        public List<int> SelectedSwimTags
        {
            get
            {

                return _selectedSwimTags;
            }
            set
            {
                _selectedSwimTags = value;
            }
        }

        private List<int> _selectedBikeTags = new List<int>();
        public List<int> SelectedBikeTags
        {
            get
            {

                return _selectedBikeTags;
            }
            set
            {
                _selectedBikeTags = value;
            }
        }

        private List<int> _selectedRunTags = new List<int>();
        public List<int> SelectedRunTags
        {
            get
            {

                return _selectedRunTags;
            }
            set
            {
                _selectedRunTags = value;
            }
        }

    }
}