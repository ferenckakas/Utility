using System.Collections.Generic;

namespace Services.Chart
{
    public class ChartInfo
    {
        public class DropDownElement
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public bool Selected { get; set; }
        }

        public static List<DropDownElement> CountryList = new List<DropDownElement>();
        public static List<DropDownElement> ChartList = new List<DropDownElement>();
        public static List<DropDownElement> TimeList = new List<DropDownElement>();

        public static void GetInfo(string country, string chartName, string time, string timeName, string genre, ref CChart chart, ref List<CSong> list, ref bool latest)
        {
            chart = null;
            list = null;
            //bool bExistNewChart = false;
            latest = false;

            if (country == "custom")
            {
                chart = new CustomChart("Custom", "Private", timeName);
                if (chart == null) return;

                list = chart.GetCurrentChart();
            }
            else
            {
                if (time == null || time.Contains("latest"))
                    latest = true;

                if (latest)
                {
                    chart = CChart.Instantiate(country + "/" + chartName + "/" + genre);

                    if (chart != null)
                    {
                        //bExistNewChart = chart.ExistsNew();
                        list = chart.GetCurrentChart();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    chart = new No1Chart(country, chartName, timeName);
                    if (chart == null) return;

                    list = chart.GetCurrentChart();

                    //chart = CChart.Instantiate(sCountry + "/" + sChartName + "/" + sGenre);
                    //string sYear = sTime; //.Split('-')[3];
                    //list = chart.GetNo1Chart(sYear + ".");
                }
            }
        }
    }
}