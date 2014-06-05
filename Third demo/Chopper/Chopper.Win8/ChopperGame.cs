using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chopper
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ChopperGame : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        private IGameInput _gameInput;
        private GameWorld _gameWorld;

        public ChopperGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _gameInput = new KeyboardGameInput();
            Content.RootDirectory = "Content";
        }

        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

        public IGameInput GameInput
        {
            get { return _gameInput; }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _gameWorld = new GameWorld(this);
            _gameWorld.Setup();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _gameWorld.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _gameWorld.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
