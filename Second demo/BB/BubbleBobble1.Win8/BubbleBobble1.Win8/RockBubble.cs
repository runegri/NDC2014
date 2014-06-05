using Microsoft.Xna.Framework;

namespace BubbleBobble1.Win8
{
    public class RockBubble : Bubble
    {
        private readonly Rectangle _textureSource = new Rectangle(96, 0, 32, 32);

        public RockBubble(GameWorld gameWorld)
            : base(gameWorld)
        {
            Body.Mass = 10f;
            Body.LinearDamping = 0;
        }

        protected override Rectangle TextureSource
        {
            get { return _textureSource; }
        }
    }
}