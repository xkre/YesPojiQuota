using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace YesPojiQuota.UWP.Utils
{
    public static class ColourHelper
    {
        public static Color LightenDarkenColor(Color color, double correctionFactor)
        {
            double red = (255 - color.R) * correctionFactor + color.R;
            double green = (255 - color.G) * correctionFactor + color.G;
            double blue = (255 - color.B) * correctionFactor + color.B;
            return Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);
        }
    }
}
