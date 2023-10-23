using System.Linq;

namespace Entities
{
    public partial class Chart
    {
        public static Chart GetChart(string name, MusicEntities entities)
        {
            Chart chart = entities.Charts.FirstOrDefault(a => a.Name == name);
            if (chart == null)
            {
                chart = new Chart();
                chart.Name = name;
                entities.Charts.Add(chart);
            }

            return chart;
        }
    }
}
