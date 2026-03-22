namespace DigMap.Application.DTOs.Finds
{
    //Те, що ми будемо віддавати клієнту
    public abstract class FindItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime DateFound { get; set; }
    }
}