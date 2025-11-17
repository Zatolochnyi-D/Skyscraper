using UnityEngine;

namespace ThreeDent.DevelopmentTools.Option
{
    public static class LayerMaskExtension
    {
        public static bool NotIn(this int objectLayer, LayerMask layerMask)
        {
            var objectLayerMask = 1 << objectLayer;
            return (layerMask & objectLayerMask) == 0;
        }
    }
}
