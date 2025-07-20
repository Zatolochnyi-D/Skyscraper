using ThreeDent.Helpers.Extensions;
using UnityEngine;

namespace ThreeDent.DevelopmentTools.Option
{
    public static class TransformExtension
    {
        public static void ChangePosition(this Transform transform, float? x = null, float? y = null, float? z = null)
        {
            transform.position = transform.position.With(x, y, z);
        }
    }
}
