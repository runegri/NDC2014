using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace BubbleBobble.MacOS
{
    public class Wall : GameObject
    {
        private Rectangle _target;

        public Wall(GameWorld gameWorld, Rectangle target)
            : base(gameWorld, "wall")
        {
            _target = target;
            Body = BodyFactory.CreateRectangle(World, ConvertUnits.ToSimUnits(_target.Width), ConvertUnits.ToSimUnits(_target.Height), 1f);
            Position = new Vector2(_target.X, _target.Y);
            SolidWall = true;
        }

        public override Vector2 Position
        {
            get { return base.Position; }
            set
            {
                base.Position = new Vector2(value.X + _target.Width / 2f, value.Y + _target.Height / 2f);
                _target = new Rectangle((int)value.X, (int)value.Y, _target.Width, _target.Height);
            }
        }

        public bool SolidWall { get; set; }

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
