namespace DigMap.Application.DTOs.Finds
{
    //Те, що ми будемо віддавати клієнту
    public class ArtifactDto : FindItemDto
    {
        public string Era { get; set; }
        public string Material { get; set; }
        public string Class { get; set; }
    }
}
