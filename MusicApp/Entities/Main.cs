namespace Entities
{
    public static class Main
    {
        public static void Save(Chart chart)
        {
            using (Entities.MusicEntities entities = new Entities.MusicEntities())
            {
                entities.Charts.Add(chart);
                entities.SaveChanges();
            }
        }
    }
}
