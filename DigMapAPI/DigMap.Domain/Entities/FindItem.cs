namespace DigMap.Domain.Entities
{
    public abstract class FindItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime DateFound { get; set; }
        public string UserId { get; set; }
    }
}