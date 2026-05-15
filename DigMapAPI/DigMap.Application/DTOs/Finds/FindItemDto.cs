using System.Text.Json.Serialization; // Обов'язково додай цей using!

namespace DigMap.Application.DTOs.Finds
{
    [JsonDerivedType(typeof(CoinDto), typeDiscriminator: "coin")]
    [JsonDerivedType(typeof(ArtifactDto), typeDiscriminator: "artifact")]
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