using System.ComponentModel.DataAnnotations;

namespace DiplomService.ViewModels.Measures
{
    public class MeasureViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано имя")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Не указано место проведения")]
        public string Place { get; set; } = "";

        [Required]
        public bool OneTime { get; set; } = true;

        [Required]
        public bool WeekDays { get; set; } = false;

        public string? Descrition { get; set; } = "";

        public TimeSpan Length { get; set; } = TimeSpan.Zero;

        public bool SameForAll { get; set; } = false;



        public int EventId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool DivisionsExists { get; set; } = true;

        public virtual List<MeasureDatesViewModel> MeasureDates { get; set; } = new();
        public virtual List<MeasureDaysViewModel> MeasureDays { get; set; } = new()
        {
            new()
            {DayNumber = 0,},
            new()
            {DayNumber = 1,},
            new()
            {DayNumber = 2,},
            new()
            {DayNumber = 3},
            new()
            {DayNumber = 4},
            new()
            {DayNumber = 5},
            new()
            {DayNumber = 6},
        };

    }
}
