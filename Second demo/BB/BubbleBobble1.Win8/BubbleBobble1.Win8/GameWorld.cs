using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.DebugView;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BubbleBobble.MacOS
{
    public class GameWorld : IUpdateable
    {
        private const int WorldWidth = 608;
        private const int WorldHeight = 608;
        private const double MinTimeBeforeFirstDeath = 5;
        private static readonly Color BackgroundColor = new Color(80, 80, 80, 255);

        private const bool AddEnemies = true;

        private readonly ContentManager _content;
        private readonly SpriteBatch _spriteBatch;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly IGameInput _gameInput;
        private readonly Camera2D _camera;
        private readonly World _world;

		private readonly DebugViewXNA _debugView;

        private readonly float _timeStep;

        private readonly List<Wall> _walls = new List<Wall>();
        private readonly List<Bubble> _bubbles = new List<Bubble>();
        private readonly List<GameObject> _allGameOjbects = new List<GameObject>();
        private readonly List<Enemy> _enemies = new List<Enemy>();

        private Dragon _dragon;
        private readonly Vector2 _dragonStartPosition = new Vector2(WorldWidth / 2f, WorldHeight - 16 - 100);

        private Texture2D _background;

        static GameWorld()
        {
            ConvertUnits.SetDisplayUnitToSimUnitRatio(32f); // 32 pixels == 1 m in the game world
        }

        public GameWorld(BubbleBobbleGame game)
        {
            _content = game.Content;
            _spriteBatch = game.SpriteBatch;
            _graphicsDevice = game.GraphicsDevice;
            _gameInput = game.GameInput;
            _camera = new Camera2D(_graphicsDevice);
            
            // Creates the farseer world with a gravity
            _world = new World(new Vector2(0, 20));
        
            _timeStep = (float)game.TargetElapsedTime.TotalSeconds;

            // Setup the debug view
            _debugView = new DebugViewXNA(_world);
            _debugView.AppendFlags(DebugViewFlags.Shape);
            _debugView.AppendFlags(DebugViewFlags.PolygonPoints);
            _debugView.AppendFlags(DebugViewFlags.CenterOfMass);
        }

        public ContentManager Content
        {
            get { return _content; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

        public World World
        {
            get { return _world; }
        }

        public IGameInput GameInput
        {
            get { return _gameInput; }
        }

        public List<GameObject> AllGameObjects
        {
            get { return _allGameOjbects; }
        }

        public void Setup()
        {
            _dragon = new Dragon(this) { Position = _dragonStartPosition };
            _allGameOjbects.Add(_dragon);

            _walls.Add(new Wall(this, new Rectangle(0, 0, WorldWidth, 16)));
            _walls.Add(new Wall(this, new Rectangle(0, 0, 16, WorldHeight)));
            _walls.Add(new Wall(this, new Rectangle(0, WorldHeight - 16, WorldWidth, 16)));
            _walls.Add(new Wall(this, new Rectangle(WorldWidth - 16, 0, 16, WorldHeight)));


            _walls.Add(new Wall(this, new Rectangle(200, 450, 208, 16)) { SolidWall = false });
            _walls.Add(new Wall(this, new Rectangle(408, 350, 16, 116)));

            CreateBackgroudTexture();
            SetupCamera();

			//_debugView.LoadContent(_graphicsDevice, _content);
        }

        private void SetupCamera()
        {
            _camera.Pos = new Vector2(WorldWidth / 2, WorldHeight / 2);
            _camera.Zoom = _graphicsDevice.Viewport.Height / (1f * WorldWidth);
        }

        private void CreateBackgroudTexture()
        {
            _background = new Texture2D(_graphicsDevice, 1, 1);
            _background.SetData(new[] { 0xFFFFFFFF });
        }

        public void Update(GameTime gameTime)
        {
            _world.Step(_timeStep);
            _gameInput.Update(gameTime);

            for (var i = 0; i < _allGameOjbects.Count; i++)
            {
                _allGameOjbects[i].Update(gameTime);
            }


            if (AddEnemies && Rnd.Next(0, 100) == 0)
            {
                var enemy = new Enemy(this)
                    {
                        Position = new Vector2(Rnd.Next(0, WorldWidth - 100) + 50, 100)
                    };
                _allGameOjbects.Add(enemy);
                _enemies.Add(enemy);
            }
        }

        public void Draw(GameTime gameTime)
        {
            var transform = _camera.GetTransformation();

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null, null, transform);
            _spriteBatch.Draw(_background, new Rectangle(0, 0, WorldWidth, WorldHeight), null, BackgroundColor);
            for (var i = 0; i < _walls.Count; i++)
            {
                _walls[i].Draw();
            }
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, transform);
            _dragon.Draw();

            for (var i = 0; i < _bubbles.Count; i++)
            {
                _bubbles[i].Draw();
            }

            for (var i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].Draw();
            }

            _spriteBatch.End();

            if (_gameInput.ShowDebug)
            {
                var projection = Matrix.CreateOrthographicOffCenter(0f, _camera.ViewportWidth, _camera.ViewportHeight, 0f, 0f, 1f);
                var scale = Matrix.CreateScale(32f) * transform;
                _debugView.RenderDebugData(projection, scale);
            }
        }

        public void AddBubble()
        {
            var bubble = BubbleFactory.CreateBubble(this, _dragon);
            _bubbles.Add(bubble);
            _allGameOjbects.Add(bubble);
        }

        public void PopBubble(Bubble bubble)
        {
            _bubbles.Remove(bubble);
            _allGameOjbects.Remove(bubble);
            _world.RemoveBody(bubble.Body);
        }

        public void PlayerDied(Dragon dragon)
        {
            if (!dragon.IsDead && dragon.Age > MinTimeBeforeFirstDeath)
            {
                dragon.Die();
                dragon.Position = _dragonStartPosition;
            }
        }

        public void CaptureEnemy(Enemy enemy)
        { }

        public void KillEnemy(Enemy enemy)
        {
            _enemies.Remove(enemy);
            _allGameOjbects.Remove(enemy);
            _world.RemoveBody(enemy.Body);
        }
    }
}
