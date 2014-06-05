using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BubbleBobble1.Win8
{
    public class GameArea : IUpdateable
    {
        private const int WorldWidth = 608;
        private const int WorldHeight = 608;
        private static readonly Color BackgroundColor = new Color(40, 40, 40, 255);

        private readonly ContentManager _content;
        private readonly SpriteBatch _spriteBatch;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly IGameInput _gameInput;
        private readonly Camera2D _camera;

        private readonly List<Wall> _walls = new List<Wall>();
        private Dragon _dragon;

        private Texture2D _background;

        public GameArea(ContentManager content, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, IGameInput gameInput)
        {
            _content = content;
            _spriteBatch = spriteBatch;
            _graphicsDevice = graphicsDevice;
            _gameInput = gameInput;
            _camera = new Camera2D(graphicsDevice);
        }

        public void Setup()
        {
            // Add the dragon
            _dragon = new Dragon(_content, _spriteBatch, _gameInput) { Position = new Vector2(WorldWidth / 2f, WorldHeight - 16 - 16) };

            // Setup walls
            _walls.Add(new Wall(_content, _spriteBatch, new Rectangle(0, 0, WorldWidth, 16)));
            _walls.Add(new Wall(_content, _spriteBatch, new Rectangle(0, 0, 16, WorldHeight)));
            _walls.Add(new Wall(_content, _spriteBatch, new Rectangle(0, WorldHeight - 16, WorldWidth, 16)));
            _walls.Add(new Wall(_content, _spriteBatch, new Rectangle(WorldWidth - 16, 0, 16, WorldHeight)));

            CreateBackgroudTexture();
            SetupCamera();
        }

        private void SetupCamera()
        {
            // Place the camera at the middle of the game world
            _camera.Pos = new Vector2(WorldWidth / 2, WorldHeight / 2);

            // Zoom so that the game world fits the screen
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
            _dragon.Update(gameTime);

            //_camera.Rotation += 0.01f;
        }

        public void Draw(GameTime gameTime)
        {
            var transform = _camera.GetTransformation();

            // Draw the walls 
            // Notice the use of SamplerState.LinearWrap - this causes the textures to repeat instead of being stretched
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null, null, transform);
            _spriteBatch.Draw(_background, new Rectangle(0, 0, WorldWidth, WorldHeight), null, BackgroundColor);
            for (var i = 0; i < _walls.Count; i++)
            {
                _walls[i].Draw();
            }
            _spriteBatch.End();
            

            // Draw the dragon
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, transform);
            _dragon.Draw();
            _spriteBatch.End();

        }
    }
}
