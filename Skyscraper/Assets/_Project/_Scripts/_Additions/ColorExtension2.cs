using UnityEngine;

namespace ThreeDent.DevelopmentTools.Option
{
    public static class ColorExtension2
    {
        public static Color WithHsv(this Color color, float? h = null, float? s = null, float? v = null)
        {
            Color.RGBToHSV(color, out var thisH, out var thisS, out var thisV);
            thisH = h ?? thisH;
            thisS = s ?? thisS;
            thisV = v ?? thisV;
            return Color.HSVToRGB(thisH, thisS, thisV);
        }
    }
}
