using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace Chopper
{
    public class LandingZone : GameObject
    {
        private Rectangle _target;
        private readonly Vector2 _origin = new Vector2(256, 32);

        public LandingZone(GameWorld gameWorld, Rectangle target)
            : base(gameWorld, "landing_zone")
        {
            _target = target;
            Body = BodyFactory.CreateRectangle(World, ConvertUnits.ToSimUnits(target.Width), ConvertUnits.ToSimUnits(target.Height), 1f);
            Body.OnCollision += OnCollision;
            Position = new Vector2(target.X, target.Y);
        }

        protected override Vector2 Origin
        {
            get { return _origin; }
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

        bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            var crateBody = fixtureB.Body;
            var crate = crateBody.UserData as Crate;
            if (crate != null)
            {
                GameWorld.RemoveCrate(crate);
            }
            return true;
        }

        public override void Draw()
        {
            SpriteBatch.Draw(Texture, _target, TextureSource, Color.White);
        }
    }
}
