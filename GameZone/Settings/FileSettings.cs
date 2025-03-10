namespace GameZone.Settings
{
    public class FileSettings
    {
        public  const string ImagesPath = "/Assets/Images/Games";
        public  const string AllowedExtensions = ".png,.jpg,.jpeg";
        public  const int MaxFileSizeInMB = 1;
        public  const int MaxFileSizeInByte = MaxFileSizeInMB * 1024 * 1024;
    }
}
