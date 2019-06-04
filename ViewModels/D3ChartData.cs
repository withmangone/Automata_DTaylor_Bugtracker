using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Automata_DTaylor_Bugtracker.ViewModels
{
    public class D3DataChartDataSeries
    {
        public List<D3ChartData> Data { get; set; }

        public D3DataChartDataSeries()
        {
            Data = new List<D3ChartData>();
        }
    }

    public class D3ChartData
    {
        public string TicketType { get; set; }
        public int Count { get; set; }
    }

    public class StackedAreaData
    {
        public string key { get; set; }
        public string color { get; set; }
        public List<List<long>> values = new List<List<long>>();
    }
}
