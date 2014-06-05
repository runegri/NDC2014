using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace Chopper
{
    /// <summary>
    /// This defines the entire background of the game. The walls are polygon objects in the game 
    /// and can be collided with :-)
    /// </summary>
    public class CaveWall : GameObject
    {
        public CaveWall(GameWorld gameWorld, string textureName)
            : base(gameWorld, textureName)
        {
            // Read the texture data
            var textureData = new uint[Texture.Width * Texture.Height];
            Texture.GetData(textureData);

            // Detect an outline of the texture
            var outline = PolygonTools.CreatePolygon(textureData, Texture.Width);

            // Scale the outline so that it fits the size of my game world
            var scaleVector = ConvertUnits.ToSimUnits(2, 2);
            outline.Scale(scaleVector);

            // Simplify the outline to remove redundant points
            outline = SimplifyTools.CollinearSimplify(outline);

            // Decompose the outline into polygons
            var decomposed = BayazitDecomposer.ConvexPartition(outline);

            // Create the body for the game world
            Body = BodyFactory.CreateCompoundPolygon(World, decomposed, 1f);
        }

        public override void Draw()
        {
            SpriteBatch.Draw(Texture, new Rectangle(0, 0, 4096, 4096), null, Color.White);
        }
    }
}
