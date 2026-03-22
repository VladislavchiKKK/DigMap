namespace DigMap.Application.DTOs.Finds
{
    //Те, що ми будемо приймати від клієнта при створенні монети
    public class CreateCoinDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime DateFound { get; set; }
        public int Year { get; set; }
        public string Metal { get; set; }
        public string Denomination { get; set; }
    }
}