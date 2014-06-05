using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BubbleBobble1.Win8
{
    public abstract class Drawable
    {
        protected readonly Texture2D Texture;
        protected readonly SpriteBatch SpriteBatch;
        private readonly Rectangle _rectangle;

        protected Drawable(Texture2D texture, SpriteBatch spriteBatch)
        {
            SpriteBatch = spriteBatch;
            Texture = texture;
            _rectangle = new Rectangle(0, 0, Texture.Width, Texture.Width);
        }

        public virtual Vector2 Position { get; set; }

        protected virtual Rectangle TextureSource
        {
            get { return _rectangle; }
        }

        protected virtual SpriteEffects SpriteEffect
        {
            get { return SpriteEffects.None; }
        }

        protected virtual Vector2 Origin
        {
            get { return Vector2.Zero; }
        }

        protected virtual float Rotation
        {
            get { return 0f; }
        }

        public virtual void Draw()
        {
            SpriteBatch.Draw(Texture, Position, TextureSource, Color.White, Rotation, Origin, 1f, SpriteEffect, 0f);
        }
    }
}