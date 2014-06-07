using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BubbleBobble.MacOS
{
    public class Wall : Drawable
    {
        private Rectangle _target;

        public Wall(ContentManager content, SpriteBatch spriteBatch, Rectangle target)
            : base(content.Load<Texture2D>("wall"), spriteBatch)
        {
            _target = target;
        }

        public override Vector2 Position
        {
            get { return new Vector2(_target.X, _target.Y); }
            set
            {
                _target = new Rectangle((int)value.X, (int)value.Y, _target.Width, _target.Height);
            }
        }

        protected override Rectangle TextureSource
        {
            get
            {
                return new Rectangle(0, 0, _target.Width, _target.Height);
            }
        }

        public override void Draw()
        {
            SpriteBatch.Draw(Texture, _target, TextureSource, Color.White);
        }
    }
}
