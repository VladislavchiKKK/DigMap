namespace DigMap.Domain.Entities
{
    public class Coin : FindItem
    {
        public int Year { get; set; }
        public string Metal { get; set; }
        public string Denomination { get; set; }
    }
}