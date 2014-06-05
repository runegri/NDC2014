using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IUpdateable = BubbleBobble1.Win8.IUpdateable;

namespace Chopper
{
    public abstract class GameObject : IUpdateable
    {
        protected readonly Texture2D Texture;
        protected readonly SpriteBatch SpriteBatch;
        private readonly Rectangle _rectangle;

        private readonly World _world;
        private readonly GameWorld _gameWorld;
        private Body _body;

        protected GameObject(GameWorld gameWorld, string textureName)
        {
            _gameWorld = gameWorld;
            SpriteBatch = gameWorld.SpriteBatch;
            Texture = gameWorld.Content.Load<Texture2D>(textureName);

            _world = gameWorld.World;
            _rectangle = new Rectangle(0, 0, Texture.Width, Texture.Width);
        }

        public Body Body
        {
            get { return _body; }
            set
            {
                _body = value;
                _body.UserData = this; // So that the body knows which GameObject it controls
            }
        }

        protected World World
        {
            get { return _world; }
        }

        public GameWorld GameWorld
        {
            get { return _gameWorld; }
        }

        /// <summary>
        /// Gets or sets the objects position in display units
        /// </summary>
        public virtual Vector2 Position
        {
            get { return ConvertUnits.ToDisplayUnits(Body.Position); }
            set { Body.Position = ConvertUnits.ToSimUnits(value); }
        }

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

        /// <summary>
        /// Gets the rotation in radians
        /// </summary>
        protected virtual float Rotation
        {
            get { return Body.Rotation; }
        }

        public virtual float Depth
        {
            get { return 0f; }
        }

        public virtual void Draw()
        {
            SpriteBatch.Draw(Texture, Position, TextureSource, Color.White, Rotation, Origin, 1f, SpriteEffect, Depth);
        }

        public virtual void Update(GameTime gameTime)
        {
        }
    }
}