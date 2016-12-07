using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceAnalysis.Models
{
    public class Gender
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GenderId { get; set; }

        public string Value { get; set; }
        public string DisplayName { get; set; }


        //if selectedId==0 then select all
        static public IList<int> Expand(IList<int> selectedGenders)
        {
            if (selectedGenders.Count > 0 && selectedGenders.Contains(0)) //id==0
            {
                using (var db = new RaceAnalysisDbContext())
                {
                    return db.Genders.Select(g => g.GenderId).ToList();
                }
            }
            else
            {
                return selectedGenders;
            }
        }
    }
}