using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace BubbleBobble1.Win8
{
    public class Bubble : GameObject
    {
        public const int Radius = 16;
        public const double BubbleLifeTimeBase = 3;

        private static readonly Vector2 OriginVector = new Vector2(16, 16);

        protected double Age;
        protected readonly double MaxAge;
        private readonly Rectangle _textureSource = new Rectangle(0, 0, 32, 32);

        public Bubble(GameWorld gameWorld)
            : base(gameWorld, "bubble")
        {
            Body = BodyFactory.CreateCircle(gameWorld.World, 
                ConvertUnits.ToSimUnits(Radius), 
                0.1f, bodyType: BodyType.Dynamic);
            Body.LinearDamping = 8f;
            Body.Friction = 1f;
            Body.OnCollision += OnCollision;

            MaxAge = SetMaxAge();
        }

        public bool CapturedEnemy { get; set; }

        protected virtual bool OnCollision(Fixture me, Fixture that, Contact contact)
        {
            var enemy = that.Body.UserData as Enemy;
            if (enemy != null)
            {
                GameWorld.KillEnemy(enemy);
                GameWorld.PopBubble(this);
                return false;
            }
            return true;
        }

        private double SetMaxAge()
        {
            double age = BubbleLifeTimeBase;
            while (Rnd.Next(0, 10) > 3)
            {
                age += BubbleLifeTimeBase;
            }
            return age;
        }

        protected override Vector2 Origin
        {
            get { return OriginVector; }
        }

        public override void Update(GameTime gameTime)
        {
            Age += gameTime.ElapsedGameTime.TotalSeconds;
            if (Age > MaxAge)
            {
                Pop();
            }
        }

        protected override Rectangle TextureSource
        {
            get { return _textureSource; }
        }

        protected virtual void Pop()
        {
            GameWorld.PopBubble(this);
        }
    }
}