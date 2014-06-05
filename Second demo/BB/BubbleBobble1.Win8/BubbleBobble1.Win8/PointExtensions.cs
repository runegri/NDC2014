using Microsoft.Xna.Framework;

namespace BubbleBobble1.Win8
{
    public static class PointExtensions
    {
        public static Vector2 ToVector2(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }
    }
}