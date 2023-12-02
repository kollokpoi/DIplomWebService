namespace DiplomService.Models.EventsFolder.Division
{
    public class MeasureDates
    {
        public int Id { get; set; }
        public DateTime Datetime { get; set; }
        public int MeasureDivisionsInfosId { get; set; }
        public virtual MeasureDivisionsInfo MeasureDivisionsInfos { get; set; } = new();
        public string? Place { get; set; } = "";
    }
}
