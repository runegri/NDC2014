using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.DebugView;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Chopper
{
    public class GameWorld
    {
        private const int WorldWidth = 4096;
        private const int WorldHeight = 4096;
        private static readonly Color BackgroundColor = new Color(20, 20, 20, 255);

        private readonly ContentManager _content;
        private readonly SpriteBatch _spriteBatch;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly IGameInput _gameInput;
        private readonly Camera2D _camera;
        private readonly World _world;

        private readonly DebugViewXNA _debugView;

        private readonly float _timeStep;

        private readonly List<GameObject> _allGameOjbects = new List<GameObject>();
        private Ufo _ufo;
        private Texture2D _background;

        private readonly List<Crate> _crates;


        static GameWorld()
        {
            ConvertUnits.SetDisplayUnitToSimUnitRatio(32f); // 32 pixels == 1 m in the game world
        }

        public GameWorld(ChopperGame game)
        {
            _content = game.Content;
            _spriteBatch = game.SpriteBatch;
            _graphicsDevice = game.GraphicsDevice;
            _gameInput = game.GameInput;
            _camera = new Camera2D(_graphicsDevice);
            _world = new World(new Vector2(0, 10));
            _timeStep = (float)game.TargetElapsedTime.TotalSeconds;

            _debugView = new DebugViewXNA(_world);
            _debugView.Flags = 0;
            _debugView.AppendFlags(DebugViewFlags.Shape);
            _debugView.AppendFlags(DebugViewFlags.Joint);
            _debugView.SleepingShapeColor = Color.YellowGreen;

            _crates = new List<Crate>();
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
            CreateBackgroudTexture();
            SetupCamera();

            _ufo = new Ufo(this) { Position = new Vector2(300, 300) };
            _allGameOjbects.Add(_ufo);

            var background = new CaveWall(this, "map1");
            _allGameOjbects.Add(background);

            AddCrates();

            var landingZone = new LandingZone(this, new Rectangle(3600, 1540, 300, 30));
            _allGameOjbects.Add(landingZone);

            _debugView.LoadContent(_graphicsDevice, _content);
        }

        private void AddCrates()
        {
            // Define a path that gives the placements of the crates
            var cratesPath = new Path();
            cratesPath.Add(new Vector2(160, 1000));
            cratesPath.Add(new Vector2(3500, 1000));

            // Split the path into 30 positions - this is where the crates go
            var positions = cratesPath
                .SubdivideEvenly(30)
                .Select(v3 => new Vector2(v3.X, v3.Y));

            foreach (var position in positions)
            {
                var crate = new Crate(this) { Position = position };
                _crates.Add(crate);
                _allGameOjbects.Add(crate);
            }
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
            _gameInput.Update(gameTime);
            _world.Step(_timeStep);

            for (var i = 0; i < _allGameOjbects.Count; i++)
            {
                _allGameOjbects[i].Update(gameTime);
            }

            _camera.Pos = _ufo.Position;

            if (_gameInput.ZoomIn)
            {
                _camera.Zoom *= 1.01f;
            }
            else if (_gameInput.ZoomOut)
            {
                _camera.Zoom *= 0.99f;
            }
        }

        public void Draw(GameTime gameTime)
        {
            var transform = _camera.GetTransformation();

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null, null, transform);
            _spriteBatch.Draw(_background, new Rectangle(0, 0, WorldWidth, WorldHeight), null, BackgroundColor);
            for (var i = 0; i < _allGameOjbects.Count; i++)
            {
                _allGameOjbects[i].Draw();
            }

            _spriteBatch.End();

            var projection = Matrix.CreateOrthographicOffCenter(0f, _camera.ViewportWidth, _camera.ViewportHeight, 0f, 0f, 1f);
            var view = Matrix.CreateScale(32f) * transform;
            if (_gameInput.ShowDebug)
            {
                _debugView.RenderDebugData(projection, view);
            }
        }

        public void RemoveCrate(Crate crate)
        {
            if (crate.IsStuck)
            {
                World.RemoveJoint(crate.MagnetJoint);
            }

            World.RemoveBody(crate.Body);
            _allGameOjbects.Remove(crate);
        }
    }
}
