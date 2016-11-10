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

        /// <summary>
        /// Expand the list if needed. For example, ID=0 is "all age-groupers" so we need to get all age group Ids
        /// </summary>
        /// <param name="selectedAgeGroupIds"></param>
        /// <returns></returns>
        static public IList<int> Expand(IList<int> selectedAgeGroupIds)
        {
            if(selectedAgeGroupIds.Count > 0 && selectedAgeGroupIds.Contains(0)) //id==0
            {
                using (var db = new RaceAnalysisDbContext())
                {
                    /*note: a little kludgy and not the way I started coding it, but got errors try to include Pro in the Except List. */
                    var pro = db.AgeGroups.Where(a => a.Value.ToLower() == "pro");
                    var allExceptPro = db.AgeGroups.Except(pro).Select(a => a.AgeGroupId);
                    var list = allExceptPro.ToList();
                    if (selectedAgeGroupIds.Contains(pro.First().AgeGroupId))
                        list.Insert(0,pro.First().AgeGroupId);

                    return list;
                }
            }
            else
            {
                return selectedAgeGroupIds;
            }
        }
    }
}