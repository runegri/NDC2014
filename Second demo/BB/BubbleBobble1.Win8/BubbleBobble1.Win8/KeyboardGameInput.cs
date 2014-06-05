using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BubbleBobble1.Win8
{
    public class KeyboardGameInput : IGameInput
    {
        public MoveDirection Direction { get; private set; }
        public bool Jumping { get; private set; }
        public bool BlowBubble { get; private set; }
        public bool ShowDebug { get; private set; }

        public void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                Direction = MoveDirection.Left;
            }
            else if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                Direction = MoveDirection.Right;
            }
            else
            {
                Direction = MoveDirection.None;
            }

            Jumping = keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up);
            BlowBubble = keyboardState.IsKeyDown(Keys.Space);
            ShowDebug = keyboardState.IsKeyDown(Keys.Tab);
        }
    }
}