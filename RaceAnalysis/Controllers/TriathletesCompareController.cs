using RaceAnalysis.Models;
using RaceAnalysis.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaceAnalysis.Controllers
{
    public class TriathletesCompareController : BaseController
    {

        public TriathletesCompareController(IRaceService service) : base(service) { }

        // GET: TriathletesCompare
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SelectedAthletes(bool[] IsSelected,int[] TriathleteId)
        {
          List<int> selectedIds = new List<int>();
            for(int i=0; i < IsSelected.Length;i++)
            {
                if (IsSelected[i] == true)
                    selectedIds.Add(TriathleteId[i]);
            }

            var model = new TriathletesCompareViewModel();
            model.Triathletes = _DBContext.Triathletes
                    .Where(t => selectedIds.Contains(t.TriathleteId));

            return View("~/Views/TriathletesCompare/Compare.cshtml",model);
        }

        protected override ActionResult DisplayResultsView(RaceFilterViewModel model)
        {
            throw new NotImplementedException();

        }

    }
}