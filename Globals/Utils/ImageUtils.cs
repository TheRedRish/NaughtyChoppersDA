using NaughtyChoppersDA.Services;

namespace NaughtyChoppersDA.Globals.Utils
{
    public class ImageUtils
    {
        public static string ConvertProfileImageToSrcImage(byte[] imageInByteArray)
        {
            return "data:image/*;base64," + Convert.ToBase64String(imageInByteArray);
        }
    }
}
