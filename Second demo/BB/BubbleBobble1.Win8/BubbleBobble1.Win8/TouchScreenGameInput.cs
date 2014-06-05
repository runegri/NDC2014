using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace BubbleBobble1.Win8
{
    public class TouchScreenGameInput : IGameInput
    {
        private readonly Texture2D _texture;

        private readonly Rectangle _joystickArea;
        private readonly Vector2 _touchCenter;

        private Vector2 _touchPoint = Vector2.Zero;
        private readonly int _margin;
        private Rectangle _blowBubbleArea;
        private Rectangle _debugArea;

        static TouchScreenGameInput ()
        {
            TouchPanel.EnableMouseTouchPoint = true;
            TouchPanel.EnableMouseGestures = true;
        }

        public TouchScreenGameInput(ContentManager content, Viewport viewport)
        {
            _texture = content.Load<Texture2D>("touchpad");

            var width = viewport.Width;
            var height = viewport.Height;

            var freeWidth = (width - height) / 2; // The amout of free space on the sides of the game screen
            _margin = (int)(freeWidth * 0.1f); // We want a margin so the controllers don't touch the edges
            var joystickSize = freeWidth - _margin;

            // Place the joystick in the lower left corner
            _joystickArea = new Rectangle(_margin, height - joystickSize - _margin, joystickSize, joystickSize);
            _touchCenter = _joystickArea.Center.ToVector2();

            // Place the blow bubble button in the lower right corner
            _blowBubbleArea = new Rectangle(width - joystickSize - _margin, height - joystickSize - _margin, joystickSize, joystickSize);

            // Place the debug display button in the upper right corner
            _debugArea = new Rectangle(width - joystickSize - _margin, _margin, joystickSize, joystickSize);
        }

        public void Update(GameTime gameTime)
        {
            var touchState = TouchPanel.GetState();

            _touchPoint = Vector2.Zero;
            BlowBubble = false;
            ShowDebug = false;

            if (touchState.Count == 0)
            {
                Direction = MoveDirection.None;
                Jumping = false;
            }

            foreach (var touch in touchState)
            {
                var location = touch.Position;
                if (_touchPoint == Vector2.Zero)
                {
                    if (_joystickArea.Contains(location))
                    {
                        var touchVector = location - _touchCenter;
                        if (touchVector.X < -_margin)
                        {
                            Direction = MoveDirection.Left;
                        }
                        else if (touchVector.X > _margin)
                        {
                            Direction = MoveDirection.Right;
                        }
                        else
                        {
                            Direction = MoveDirection.None;
                        }

                        Jumping = (touchVector.Y < -_margin);

                        _touchPoint = location;
                    }
                }

                if (_blowBubbleArea.Contains(location))
                {
                    BlowBubble = true;
                }

                if (_debugArea.Contains(location))
                {
                    ShowDebug = true;
                }
            }
        }

        public MoveDirection Direction { get; private set; }
        public bool Jumping { get; private set; }
        public bool BlowBubble { get; private set; }
        public bool ShowDebug { get; private set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the joystick
            spriteBatch.Draw(_texture, _joystickArea, Color.Orange);

            // Draw the jump button
            spriteBatch.Draw(_texture, _blowBubbleArea, BlowBubble ? Color.Red : Color.White);

            // Draw the debug button
            spriteBatch.Draw(_texture, _debugArea, ShowDebug ? Color.Blue : Color.Yellow);

            Rectangle touchArea;
            if (_touchPoint == Vector2.Zero)
            {
                touchArea = new Rectangle((int)_touchCenter.X - _margin, (int)_touchCenter.Y - _margin, _margin * 2, _margin * 2);
            }
            else
            {
                touchArea = new Rectangle((int)_touchPoint.X - _margin, (int)_touchPoint.Y - _margin, _margin * 2, _margin * 2);
            }

            spriteBatch.Draw(_texture, touchArea, null, Color.Red);

        }
    }
}