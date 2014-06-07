using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BubbleBobble.MacOS
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BubbleBobbleGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameArea _gameArea;
        private readonly IGameInput _gameInput;

        public BubbleBobbleGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _gameInput = new KeyboardGameInput();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _gameArea = new GameArea(Content, _spriteBatch, GraphicsDevice, _gameInput);
            _gameArea.Setup();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _gameArea.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _gameArea.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
