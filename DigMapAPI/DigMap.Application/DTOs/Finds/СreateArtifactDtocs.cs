namespace DigMap.Application.DTOs.Finds
{
    //Те, що ми будемо приймати від клієнта при створенні артефакту
    public class CreateArtifactDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime DateFound { get; set; }
        public string Era { get; set; }
        public string Material { get; set; }
        public string Class { get; set; }
    }
}