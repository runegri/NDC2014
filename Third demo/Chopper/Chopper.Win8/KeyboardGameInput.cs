using BubbleBobble1.Win8;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Chopper
{
    public class KeyboardGameInput : IGameInput
    {
        public bool Left { get; private set; }
        public bool Right { get; private set; }
        public bool Up { get; private set; }
        public bool Down { get; private set; }
        public bool Thrust { get; private set; }
        public bool ShowDebug { get; private set; }

        public void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            Left = (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left));
            Right = (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right));
            Up = (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up));
            Down = (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down));

            Thrust = keyboardState.IsKeyDown(Keys.Space);
            ShowDebug = keyboardState.IsKeyDown(Keys.Tab);

            ZoomIn = keyboardState.IsKeyDown(Keys.OemPlus);
            ZoomOut = keyboardState.IsKeyDown(Keys.OemMinus);

            ShowDebug = keyboardState.IsKeyDown(Keys.Tab);
        }

        public bool ZoomIn { get; private set; }
        public bool ZoomOut { get; private set; }
    }
}