namespace DiplomService.ViewModels.News
{
    public class NewsViewModel
    {
        public virtual Models.News News { get; set; } = new();
        private IFormFile? imageFile;

        public IFormFile? ImageFile
        {
            get { return imageFile; }
            set
            {
                imageFile = value;
                if (value != null)
                {
                    using var memoryStream = new MemoryStream();
                    value.CopyTo(memoryStream);
                    News.Image = memoryStream.ToArray();
                }
            }
        }
    }
}
