namespace DiplomService.ViewModels.Measures
{
    public class EventMeasuresViewModel
    {
        public int Id { get; set; }
        public byte[]? Icon { get; set; }
        public string EventName { get; set; } = "";
        public DateTime DateTime { get; set; }
        public bool SameForAll { get; set; }
        public TimeSpan Length { get; set; } = TimeSpan.Zero;
        public string? MimeType { get { return GetImageMimeType(); } }
        private string GetImageMimeType()
        {
            if (Icon == null) return "application/octet-stream";
            if (Icon.Length < 4)
            {
                return "application/octet-stream"; // По умолчанию, если массив слишком короткий для определения
            }

            if (Icon[0] == 0xFF && Icon[1] == 0xD8 && Icon[2] == 0xFF)
            {
                return "image/jpeg";
            }
            else if (Icon[0] == 0x89 && Icon[1] == 0x50 && Icon[2] == 0x4E && Icon[3] == 0x47)
            {
                return "image/png";
            }
            // Добавьте другие проверки для других форматов, если это необходимо

            return "application/octet-stream"; // Если формат неизвестен, вернуть по умолчанию
        }
    }
}
