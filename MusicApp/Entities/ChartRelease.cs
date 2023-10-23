using System;
using System.Linq;

namespace Entities
{
    public partial class ChartReleas
    {
        public static ChartReleas GetChartRelease(DateTime date, Chart chart, MusicEntities entities)
        {
            ChartReleas chartRelease = entities.ChartReleases.FirstOrDefault(a => a.Released == date && a.Chart.ID == chart.ID);
            if (chartRelease == null)
            {
                chartRelease = new ChartReleas();
                chartRelease.Released = date;
                chart.ChartReleases.Add(chartRelease);
            }

            return chartRelease;
        }
    }
}
