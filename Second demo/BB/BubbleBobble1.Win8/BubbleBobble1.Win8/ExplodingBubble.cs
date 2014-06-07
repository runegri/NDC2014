using Microsoft.Xna.Framework;

namespace BubbleBobble.MacOS
{
    public class ExplodingBubble : Bubble
    {
        private BubbleState _state = BubbleState.Normal;
        public const float ExplosionScale = 5f;

        private double _frameTimer;
        private const float FrameTime = 100;

        public ExplodingBubble(GameWorld gameWorld)
            : base(gameWorld)
        {

        }

        private enum BubbleState
        {
            Normal,
            Explode1,
            Explode2
        }

        protected override Rectangle TextureSource
        {
            get
            {
                switch (_state)
                {
                    case BubbleState.Explode1:
                        return new Rectangle(32, 0, 32, 32);
                    case BubbleState.Explode2:
                        return new Rectangle(64, 0, 32, 32);
                }
                return base.TextureSource;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Age > MaxAge - 2)
            {
                _frameTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (_frameTimer > FrameTime)
                {
                    _frameTimer = 0;
                    if (_state == BubbleState.Explode1)
                    {
                        _state = BubbleState.Explode2;
                    }
                    else
                    {
                        _state = BubbleState.Explode1;
                    }
                }
            }
        }

        protected override void Pop()
        {
            foreach (var gameObject in GameWorld.AllGameObjects)
            {
                if (gameObject != this)
                {
                    var explosionVector = gameObject.Position - Position;
                    var distance = explosionVector.Length();
                    if (distance < 40)
                    {
                        var explosionForce = Vector2.Normalize(explosionVector) * (40 * 40 - distance * distance) * ExplosionScale;
                        gameObject.Body.ApplyForce(explosionForce);
                    }
                }
            }

            base.Pop();
        }
    }
}