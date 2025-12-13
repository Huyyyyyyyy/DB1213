using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DBUtils
{
    public static class Const
    {
        public enum FileType
        {
            [Description("image_jpeg")]
            image_jpeg,
            [Description("image_png")]
            image_png,
            [Description("image_gif")]
            image_gif,
            [Description("video_mp4")]
            video_mp4,
            [Description("video_mpeg")]
            video_mpeg
        }
        public static int MAX_FILE_LENGTH = 10 * 1024 * 1024;
        public static string ROOT_MEDIA_DIRECTORY = "/var/www/app/uploads";
        public static string getFilePath(string fileName)
        {
            return $"/uploads/{fileName}";
        }
        public static string toDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field?.GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description ?? value.ToString();
        }
    }
}
