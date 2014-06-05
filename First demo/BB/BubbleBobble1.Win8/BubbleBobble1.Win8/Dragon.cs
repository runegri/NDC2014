using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BubbleBobble1.Win8
{
    public class Dragon : Drawable
    {
        private readonly IGameInput _gameInput;
        private static readonly Vector2 OriginVector = new Vector2(16, 16);

        private Frame _frame;
        private double _elapsed;
        private SpriteEffects _spriteEffect;

        private enum Frame
        {
            StandStill = 0,
            Move1 = 1,
            Move2 = 2,
            BlowBubble = 3
        }

        public Dragon(ContentManager content, SpriteBatch spriteBatch, IGameInput gameInput)
            : base(content.Load<Texture2D>("Dragon"), spriteBatch)
        {
            _gameInput = gameInput;
            _spriteEffect = SpriteEffects.None;
        }

        public MoveDirection Direction
        {
            get { return _gameInput.Direction; }
        }

        protected override SpriteEffects SpriteEffect
        {
            get { return _spriteEffect; }
        }

        protected override Vector2 Origin
        {
            get { return OriginVector; }
        }

        public void Update(GameTime gameTime)
        {
            _elapsed += gameTime.ElapsedGameTime.TotalMilliseconds;
            UpdateAnimationFrame();

            // Very simple handling of movement! Will improve this in the next demo :-)
            if (Direction == MoveDirection.Left)
            {
                Position = new Vector2(Position.X - 2, Position.Y);
            }
            else if (Direction == MoveDirection.Right)
            {
                Position = new Vector2(Position.X + 2, Position.Y);
            }
        }

        private void UpdateAnimationFrame()
        {
            if (Direction == MoveDirection.None)
            {
                // No movement
                _frame = Frame.StandStill;
                _elapsed = 0;
            }
            else
            {
                if (_elapsed > 200)
                {
                    // Movement! Switch frame every 200 ms
                    if (_frame == Frame.Move1)
                    {
                        _frame = Frame.Move2;
                    }
                    else
                    {
                        _frame = Frame.Move1;
                    }
                    _elapsed -= 200;
                }

                if (Direction == MoveDirection.Left)
                {
                    // Flip the sprite if moving to the left
                    _spriteEffect = SpriteEffects.FlipHorizontally;
                }
                else if (Direction == MoveDirection.Right)
                {
                    _spriteEffect = SpriteEffects.None;
                }
            }

            if (_gameInput.BlowBubble)
            {
                _frame = Frame.BlowBubble;
            }
        }

        protected override Rectangle TextureSource
        {
            get
            {
                // Very simple implementation of a texture atlas
                switch (_frame)
                {
                    case Frame.StandStill:
                        return new Rectangle(0, 0, 32, 32);
                    case Frame.Move1:
                        return new Rectangle(32, 0, 32, 32);
                    case Frame.Move2:
                        return new Rectangle(64, 0, 32, 32);
                    case Frame.BlowBubble:
                        return new Rectangle(96, 0, 32, 32);
                }
                return new Rectangle(0, 0, 32, 32);
            }
        }
    }
}
