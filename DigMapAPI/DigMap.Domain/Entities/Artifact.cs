namespace DigMap.Domain.Entities
{
    public class Artifact : FindItem
    {
        public string Era { get; set; }
        public string Material { get; set; }
        public string Class { get; set; }
    }
}