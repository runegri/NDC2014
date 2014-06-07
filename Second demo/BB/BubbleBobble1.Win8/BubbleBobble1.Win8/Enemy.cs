
using System;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BubbleBobble.MacOS
{
    public class Enemy : GameObject
    {
        private double _frameTime;
        private int _frame;
        private MoveDirection _moveDirection;

        private static readonly Vector2 OriginVector = new Vector2(16, 16);
        private SpriteEffects _spriteEffect;
        public const float MaxVelocity = 10f;

        public Enemy(GameWorld gameWorld)
            : base(gameWorld, "enemy")
        {
            Body = BodyFactory.CreateRectangle(World, ConvertUnits.ToSimUnits(32), ConvertUnits.ToSimUnits(32), 1f, bodyType: BodyType.Dynamic);
            Body.FixedRotation = true;
            Body.OnCollision += OnCollision;
            _spriteEffect = SpriteEffects.None;
        }

        bool OnCollision(Fixture me, Fixture that, Contact contact)
        {
            var dragon = that.Body.UserData as Dragon;
            if (dragon != null)
            {
                GameWorld.PlayerDied(dragon);
            }
            return true;
        }

        protected override Rectangle TextureSource
        {
            get { return new Rectangle(_frame * 32, 0, 32, 32); }
        }

        protected override Vector2 Origin
        {
            get { return OriginVector; }
        }

        protected override SpriteEffects SpriteEffect
        {
            get { return _spriteEffect; }
        }
        public override void Update(GameTime gameTime)
        {
            HandleAnimation(gameTime);

            if (WantsToJump && NotJumpingOrFalling)
            {
                // Jump!
                Body.ApplyLinearImpulse(new Vector2(0, -15));
            }

            HandleMovement();

            base.Update(gameTime);
        }

        private void HandleAnimation(GameTime gameTime)
        {
            _frameTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_frameTime > 200)
            {
                _frameTime -= 200;
                _frame++;
                if (_frame > 3)
                {
                    _frame = 0;
                }
            }
        }

        private void HandleMovement()
        {
            var velocityChange = Math.Max(1 - Math.Abs(Body.LinearVelocity.X) / MaxVelocity, 0);
            var direction = GetDirection();

            if (direction == MoveDirection.Left)
            {
                Body.ApplyLinearImpulse(new Vector2(-velocityChange, 0));
                _spriteEffect = SpriteEffects.None;
            }
            else if (direction == MoveDirection.Right)
            {
                Body.ApplyLinearImpulse(new Vector2(velocityChange, 0));
                _spriteEffect = SpriteEffects.FlipHorizontally;
            }
            else
            {
                // No direction, slow down
                Body.ApplyLinearImpulse(new Vector2(-Body.LinearVelocity.X / 5, 0));
            }
        }

        private bool WantsToJump
        {
            get { return Rnd.Next(200) >= 199; }
        }

        private bool NotJumpingOrFalling
        {
            get { return Math.Abs(Body.LinearVelocity.Y) < 0.0001; }
        }

        private MoveDirection GetDirection()
        {
            if (Rnd.Next(200) >= 190)
            {
                _moveDirection = Rnd.Next<MoveDirection>();
            }
            return _moveDirection;
        }
    }
}
