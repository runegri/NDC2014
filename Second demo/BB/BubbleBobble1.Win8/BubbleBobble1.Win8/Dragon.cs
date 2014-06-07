using System;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BubbleBobble.MacOS
{
    public class Dragon : GameObject
    {
        private readonly IGameInput _gameInput;
        private static readonly Vector2 OriginVector = new Vector2(16, 16);
        private readonly GameWorld _gameWorld;

        private Frame _frame;
        private double _elapsed;
        private SpriteEffects _spriteEffect;

        public const int Width = 32;
        public const int Height = 32;
        public const float MaxVelocity = 10f;
        public const float JumpImpulse = 15f;

        private DateTime _lastBubbleTime = DateTime.MinValue;
        private readonly TimeSpan _bubbleInterval = TimeSpan.FromMilliseconds(500);

        private bool _isDead;
        private double _deadTime;
        private double _age;

        private enum Frame
        {
            StandStill = 0,
            Move1 = 1,
            Move2 = 2,
            BlowBubble = 3
        }

        public Dragon(GameWorld gameWorld)
            : base(gameWorld, "Dragon")
        {
            _gameInput = gameWorld.GameInput;
            _spriteEffect = SpriteEffects.None;
            _gameWorld = gameWorld;

            Body = BodyFactory.CreateRectangle(World, 
                ConvertUnits.ToSimUnits(Width), ConvertUnits.ToSimUnits(Height), 
                1f, bodyType: BodyType.Dynamic);
            Body.FixedRotation = true;
            Body.OnCollision += OnCollision;
        }

        private bool OnCollision(Fixture me, Fixture collidedWith, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (collidedWith.Body.UserData is Wall && Body.LinearVelocity.Y < 0)
            {
                var wall = (Wall)collidedWith.Body.UserData;
                return wall.SolidWall;
            }

            return true;
        }

        public void Die()
        {
            _isDead = true;
            _deadTime = 0;
            _age = 0;
        }

        public bool IsDead
        {
            get { return _isDead; }
        }

        public double Age
        {
            get { return _age; }
        }

        public MoveDirection Direction
        {
            get { return _gameInput.Direction; }
        }

        protected override SpriteEffects SpriteEffect
        {
            get { return _spriteEffect; }
        }

        public LookDirection LookDirection
        {
            get { return _spriteEffect == SpriteEffects.None ? LookDirection.Right : LookDirection.Left; }
        }

        protected override Vector2 Origin
        {
            get { return OriginVector; }
        }

        public bool IsJumping { get; private set; }

        public bool IsFalling
        {
            get { return Math.Abs(Body.LinearVelocity.Y) > 0.00001; }
        }

        public override void Update(GameTime gameTime)
        {
            _elapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_isDead)
            {
                _deadTime += gameTime.ElapsedGameTime.TotalSeconds;
                if (_deadTime > 5)
                {
                    _isDead = false;
                    _deadTime = 0;
                }
                else
                {
                    // Dead player, cannot move or do anything
                    return;
                }
            }

            _age += gameTime.ElapsedGameTime.TotalSeconds;
            UpdateFrame();

            HandleMovement();
        }

        private void HandleMovement()
        {
            var velocityChange = Math.Max(1 - Math.Abs(Body.LinearVelocity.X)/MaxVelocity, 0);

            if (Direction == MoveDirection.Left)
            {
                Body.ApplyLinearImpulse(new Vector2(-velocityChange, 0));
            }
            else if (Direction == MoveDirection.Right)
            {
                Body.ApplyLinearImpulse(new Vector2(velocityChange, 0));
            }
            else
            {
                // No direction, slow down
                Body.ApplyLinearImpulse(new Vector2(-Body.LinearVelocity.X/5, 0));
            }

            if (_gameInput.Jumping && !IsJumping && !IsFalling)
            {
                IsJumping = true;
                Body.ApplyLinearImpulse(new Vector2(0, -JumpImpulse));
            }
            else if (!_gameInput.Jumping)
            {
                IsJumping = false;
            }
        }

        private void UpdateFrame()
        {
            if (Direction == MoveDirection.None)
            {
                _frame = Frame.StandStill;
                _elapsed = 0;
            }
            else
            {
                if (_elapsed > 200)
                {
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
                BlowBubble();
            }
        }

        private void BlowBubble()
        {
            var timeSinceLastBubble = DateTime.Now - _lastBubbleTime;
            if (timeSinceLastBubble >= _bubbleInterval)
            {
                _lastBubbleTime = DateTime.Now;
                _gameWorld.AddBubble();
            }
        }

        protected override Rectangle TextureSource
        {
            get
            {
                switch (_frame)
                {
                    case Frame.StandStill:
                        return new Rectangle(0, 0, Width, Height);
                    case Frame.Move1:
                        return new Rectangle(Width, 0, Width, Height);
                    case Frame.Move2:
                        return new Rectangle(Width * 2, 0, Width, Height);
                    case Frame.BlowBubble:
                        return new Rectangle(Width * 3, 0, Width, Height);
                }
                return new Rectangle(0, 0, Width, Height);
            }
        }

        public override void Draw()
        {
            if (!IsDead)
            {
                base.Draw();
            }
        }
    }
}
