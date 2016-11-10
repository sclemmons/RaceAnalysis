using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RaceAnalysis.Models
{
    public class AgeGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AgeGroupId { get; set; }

        public string Value { get; set; }
        public string DisplayName { get; set; }


        static public IList<int> Expand(IList<int> selectedAgeGroupIds)
        {
            if(selectedAgeGroupIds.Count > 0 && selectedAgeGroupIds[0] == 0)
            {
                using (var db = new RaceAnalysisDbContext())
                {
                    var x = db.AgeGroups.Where(a => a.Value.ToLower()=="pro");
                    var allexceptPro = db.AgeGroups.Except(x).Select(a => a.AgeGroupId);
                    return allexceptPro.ToList();                    
                }
            }
            else
            {
                return selectedAgeGroupIds;
            }
        }
    }
}