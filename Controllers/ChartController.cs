using Automata_DTaylor_Bugtracker.Models;
using Automata_DTaylor_Bugtracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Automata_DTaylor_Bugtracker.Controllers
{
    public class ChartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Chart
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public JsonResult GetTicketTypeData()
        {
            var ticketTypes = db.TicketTypes.ToList();
            var chartdata = new D3DataChartDataSeries();

            foreach (var type in ticketTypes)
            {
                chartdata.Data.Add(new D3ChartData()
                {
                    TicketType = type.Name,
                    Count = db.Tickets.AsNoTracking().Where(t => t.TicketType.Name == type.Name).Count()
                });
            }
            return Json(chartdata);

        }

        public JsonResult GetStackedAreaData()
        {            
            var data = new List<StackedAreaData>();
            var dp1 = new StackedAreaData
            {
                key = "Defects",
                color = "#f59c1a"
            };
            for(var i = -6; i <= 0; i++)
            {
                var time = DateTimeOffset.UtcNow.AddDays(i).ToUniversalTime();
                var time24earlier = DateTimeOffset.UtcNow.AddDays(i-1).ToUniversalTime();
                dp1.values.Add(new List<long> { (long)time.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds, db.Tickets.Where(t => t.TicketType.Name == "Defect" && t.Created > time24earlier && t.Created < time).Count() });
            }
            data.Add(dp1);

            var dp2 = new StackedAreaData
            {
                key = "New Funtionality Requests",
                color = "#fb5597"
            };
            for (var i = -6; i <= 0; i++)
            {
                var time = DateTimeOffset.UtcNow.AddDays(i).ToUniversalTime();
                var time24earlier = DateTimeOffset.UtcNow.AddDays(i - 1).ToUniversalTime();
                dp2.values.Add(new List<long> { (long)time.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds, db.Tickets.Where(t => t.TicketType.Name == "New functionality Request" && t.Created > time24earlier && t.Created < time).Count() });
            }
            data.Add(dp2);

            var dp3 = new StackedAreaData
            {
                key = "Calls For Documentation",
                color = "COLOR_BLACK"
            };
            for (var i = -6; i <= 0; i++)
            {
                var time = DateTimeOffset.UtcNow.AddDays(i).ToUniversalTime();
                var time24earlier = DateTimeOffset.UtcNow.AddDays(i - 1).ToUniversalTime();
                dp3.values.Add(new List<long> { (long)time.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds, db.Tickets.Where(t => t.TicketType.Name == "Call for documentation" && t.Created > time24earlier && t.Created < time).Count() });
            }
            data.Add(dp3);

            return Json(data);
        }

    }
}