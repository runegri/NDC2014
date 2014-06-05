using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace Chopper
{
    /// <summary>
    /// The crates can be picked up by the UFO 
    /// </summary>
    public class Crate : GameObject
    {
        private const int CrateSize = 20;

        public Crate(GameWorld gameWorld)
            : base(gameWorld, "crate")
        {
            Body = BodyFactory.CreateRectangle(World, ConvertUnits.ToSimUnits(CrateSize), ConvertUnits.ToSimUnits(CrateSize), 2f, bodyType: BodyType.Dynamic);
            Body.Friction = 0.7f; // Add friciton to it will stick on slopes 
            Body.Restitution = 0.01f; // The crates shouldn't bounce
        }

        protected override Vector2 Origin
        {
            get { return new Vector2(32, 32); }
        }

        public override void Draw()
        {
            var destination = new Rectangle((int)Position.X, (int)Position.Y, CrateSize, CrateSize);
            SpriteBatch.Draw(Texture, destination, TextureSource, Color.White, Rotation, Origin, SpriteEffect, Depth);
        }

        public WeldJoint MagnetJoint { get; set; }

        /// <summary>
        /// If there exists a joint then the crate is stuck
        /// </summary>
        public bool IsStuck
        {
            get { return MagnetJoint != null; }
        }
    }
}