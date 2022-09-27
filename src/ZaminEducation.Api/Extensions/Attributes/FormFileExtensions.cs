namespace ZaminEducation.Api.Extensions.Attributes
{
    public static class FormFileExtensions
    {
        public static bool IsValid(this IFormFile file)
        {
            string[] extensions = new string[] { ".png", ".jpg" };
            var extension = Path.GetExtension(file.FileName);

            return extensions.Contains(extension.ToLower());
        }

        public static bool NoMoreThanRequiredFileSize(this IFormFile file) =>
             file.Length <= 1048576;
    }
}
