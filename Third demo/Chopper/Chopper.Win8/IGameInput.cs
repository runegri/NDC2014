using BubbleBobble1.Win8;
using Microsoft.Xna.Framework;

namespace Chopper
{
    public interface IGameInput
    {
        bool Left { get; }
        bool Right { get; }
        bool Up { get; }
        bool Down { get; }
        bool Thrust { get; }
        bool ShowDebug { get; }
        void Update(GameTime gameTime);
        bool ZoomIn { get; }
        bool ZoomOut { get; }
    }
}
