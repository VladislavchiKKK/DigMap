namespace DigMap.Application.DTOs.Finds
{
    //Те, що ми будемо віддавати клієнту
    public class CoinDto : FindItemDto
    {
        public int Year { get; set; }
        public string Metal { get; set; }
        public string Denomination { get; set; }
    }
}
