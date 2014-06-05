using System.Collections.Generic;
using System.Linq;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chopper
{
    public class Ufo : GameObject
    {
        private readonly IGameInput _gameInput;
        private Body _chainEndBall;
        private List<Body> _chainBodies;

        private readonly Texture2D _chainLinkTexture;
        private readonly Texture2D _chainBallTexture;

        private const float EngineForce = 60f;

        public Ufo(GameWorld gameWorld)
            : base(gameWorld, "ufo")
        {
            _gameInput = gameWorld.GameInput;
            _chainLinkTexture = gameWorld.Content.Load<Texture2D>("chain_link");
            _chainBallTexture = gameWorld.Content.Load<Texture2D>("chain_ball");

            var body = CreateUfoBody();
            AddBallAndChain(body);

            Body = body;
            Body.OnCollision += OnCollision;
        }

        private Body CreateUfoBody()
        {
            // Creates a body but with no shape
            // Set it to ignore gravity (this is a UFO after all!). Uses AngularDamping to avoid fast rotation
            var body = new Body(World, bodyType: BodyType.Dynamic) {IgnoreGravity = true, AngularDamping = 20};

            // The body of the ufo consists of an ellipsis and a circle
            var ellipseVertices = PolygonTools.CreateEllipse(ConvertUnits.ToSimUnits(29), ConvertUnits.ToSimUnits(14), 16);
            ellipseVertices.Translate(ConvertUnits.ToSimUnits(0, 9));
            var ellipseShape = new PolygonShape(ellipseVertices, 5f);
            body.CreateFixture(ellipseShape);

            var circleVertices = PolygonTools.CreateCircle(ConvertUnits.ToSimUnits(15), 16);
            circleVertices.Translate(ConvertUnits.ToSimUnits(0, -5));
            var circleShape = new PolygonShape(circleVertices, 5f);
            body.CreateFixture(circleShape);

            return body;
        }

        private void AddBallAndChain(Body body)
        {
            // The chain will be divided into several segments along the following path
            var chainPath = new Path();
            chainPath.Add(ConvertUnits.ToSimUnits(332, 332));
            chainPath.Add(ConvertUnits.ToSimUnits(332, 452));

            // Creates the shape for the chain segments
            var chainLinkShape = new PolygonShape(PolygonTools.CreateRectangle(ConvertUnits.ToSimUnits(1), ConvertUnits.ToSimUnits(2)), 0.5f);

            // Creates the bodies for the chain segments
            _chainBodies = PathManager.EvenlyDistributeShapesAlongPath(World, chainPath,
                new[] {chainLinkShape}, BodyType.Dynamic, 30);

            // Connects the chain segments
            for (var i = 0; i < _chainBodies.Count - 1; i++)
            {
                var joint = new RevoluteJoint(_chainBodies[i], _chainBodies[i + 1], Vector2.Zero, Vector2.Zero);
                World.AddJoint(joint);
            }

            // Connects the start of the chain to the ufo
            var chainStartJoint = new RevoluteJoint(body, _chainBodies.First(), ConvertUnits.ToSimUnits(0, 25), Vector2.Zero);
            World.AddJoint(chainStartJoint);

            // Creates the ball at the end of the chain
            _chainEndBall = BodyFactory.CreateCircle(World, ConvertUnits.ToSimUnits(10), 3f, bodyType: BodyType.Dynamic);
            _chainEndBall.Position = ConvertUnits.ToSimUnits(332, 452);
            _chainEndBall.OnCollision += ChainBallOnCollision;

            // Adds the ball to the chain
            var chainEndJoint = new RevoluteJoint(_chainBodies.Last(), _chainEndBall, Vector2.Zero, Vector2.Zero);
            World.AddJoint(chainEndJoint);

            // Adds a rope join that ensures that the chain won't stretch
            var ropeJoint = new RopeJoint(body, _chainEndBall, ConvertUnits.ToSimUnits(0, 25), Vector2.Zero)
            {
                MaxLength = chainPath.GetLength()
            };
            World.AddJoint(ropeJoint);
        }

        bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            var otherBody = fixtureB.Body;
            var cave = otherBody.UserData as CaveWall;
            if (cave != null)
            {
                // The ufo collided with the cave wall! 
                // Let's make it fall!
                Body.IgnoreGravity = false;
                Body.AngularDamping = 0;
                Body.Mass = 100;
            }

            return true;
        }

        private bool ChainBallOnCollision(Fixture fixturea, Fixture fixtureb, Contact contact)
        {
            var crateBody = fixtureb.Body;
            var crate = crateBody.UserData as Crate;
            if (crate != null && !crate.IsStuck)
            {
                // The ball hit a crate
                // We make them stick together using a weld joint
                var positionDiff = _chainEndBall.Position - crateBody.Position;
                var joint = new WeldJoint(_chainEndBall, crateBody, Vector2.Zero, positionDiff);
                World.AddJoint(joint);
                crate.MagnetJoint = joint;
            }

            return true;
        }

        protected override Vector2 Origin
        {
            get { return new Vector2(32, 32); }
        }

        public override void Update(GameTime gameTime)
        {
            if (_gameInput.Left)
            {
                Body.ApplyForce(new Vector2(-EngineForce, 0));
            }

            if (_gameInput.Right)
            {
                Body.ApplyForce(new Vector2(EngineForce, 0));
            }

            if (_gameInput.Up)
            {
                Body.ApplyForce(new Vector2(0, -EngineForce));
            }

            if (_gameInput.Down)
            {
                Body.ApplyForce(new Vector2(0, EngineForce));
            }

            if (Body.IgnoreGravity)
            {
                if (Body.Rotation > 0)
                {
                    Body.Rotation -= (float) (0.1f*gameTime.ElapsedGameTime.TotalSeconds);
                }
                else if (Body.Rotation < 0)
                {
                    Body.Rotation += (float) (0.1f*gameTime.ElapsedGameTime.TotalSeconds);
                }
            }

            base.Update(gameTime);
        }

        public Vector2 BallPosition
        {
            get { return ConvertUnits.ToDisplayUnits(_chainEndBall.Position); }
        }

        public override void Draw()
        {
            // Draws all parts of the ufo and the chain
            var dest = new Rectangle((int)Position.X, (int)Position.Y, 62, 43);
            SpriteBatch.Draw(Texture, dest, TextureSource, Color.White, Rotation, Origin, SpriteEffect, 0f);

            for (var i = 0; i < _chainBodies.Count; i++)
            {
                var chainPosition = ConvertUnits.ToDisplayUnits(_chainBodies[i].Position);
                var chainDestination = new Rectangle((int)chainPosition.X, (int)chainPosition.Y, 3, 6);

                SpriteBatch.Draw(_chainLinkTexture, chainDestination, null, Color.White, _chainBodies[i].Rotation, new Vector2(32, 32), SpriteEffects.None, 0);
            }

            var chainBallDestination = new Rectangle((int)BallPosition.X, (int)BallPosition.Y, 20, 20);
            SpriteBatch.Draw(_chainBallTexture, chainBallDestination, null, Color.White, _chainEndBall.Rotation, new Vector2(32, 32), SpriteEffects.None, 0);
        }
    }
}
