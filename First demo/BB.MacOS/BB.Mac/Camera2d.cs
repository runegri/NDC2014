using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BubbleBobble.MacOS
{

    // Adapted from http://www.david-amador.com/2009/10/xna-camera-2d-with-zoom-and-rotation/
    public class Camera2D
    {
        private float _zoom;
        private readonly int _viewportWidth;
        private readonly int _viewportHeight;

        public Camera2D(GraphicsDevice graphicsDevice)
        {
            _viewportWidth = graphicsDevice.Viewport.Width;
            _viewportHeight = graphicsDevice.Viewport.Height;            
            _zoom = 1.0f;
            Rotation = 0.0f;
            Pos = Vector2.Zero;
        }

        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; }
        }

        public float Rotation { get; set; }
        public Vector2 Pos { get; set; }

        public void Move(Vector2 amount)
        {
            Pos += amount;
        }

        public Matrix GetTransformation()
        {
            var transform = Matrix.CreateTranslation(new Vector3(-Pos.X, -Pos.Y, 0)) *
                            Matrix.CreateRotationZ(Rotation) *
                            Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                            Matrix.CreateTranslation(new Vector3(_viewportWidth * 0.5f, _viewportHeight * 0.5f, 0));
            return transform;
        }
    }
}
