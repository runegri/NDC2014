using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BubbleBobble1.Win8
{
    public class KeyboardGameInput : IGameInput
    {
        public MoveDirection Direction { get; private set; }
        public bool Jumping { get; private set; }
        public bool BlowBubble { get; private set; }

        public void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.A))
            {
                Direction = MoveDirection.Left;
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                Direction = MoveDirection.Right;
            }
            else
            {
                Direction = MoveDirection.None;
            }

            if (keyboardState.IsKeyDown(Keys.W))
            {
                Jumping = true;
            }
            else
            {
                Jumping = false;
            }

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                BlowBubble = true;
            }
            else
            {
                BlowBubble = false;
            }
        }
    }
}