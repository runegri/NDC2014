using System;
using Microsoft.Xna.Framework;

namespace BubbleBobble1.Win8
{
    public static class BubbleFactory
    {
        public static Bubble CreateBubble(GameWorld gameWorld, Dragon dragon)
        {
            var bubbleType = Rnd.Next(0, 6);

            Bubble bubble;
            float startImpulse = 20;

            switch (bubbleType)
            {
                case 5:
                    bubble = new RockBubble(gameWorld);
                    startImpulse = 60;
                    break;
                case 4:
                case 3:
                    bubble = new ExplodingBubble(gameWorld);
                    break;
                case 2:
                case 1:
                case 0:
                    bubble = new Bubble(gameWorld);
                    break;
                default:
                    throw new InvalidOperationException("Bubble type: " + bubbleType);
            }

            bubble.Position = dragon.Position;

            if (dragon.LookDirection == LookDirection.Right)
            {
                bubble.Position += new Vector2(Dragon.Width, 0);
                bubble.Body.ApplyLinearImpulse(new Vector2(startImpulse, 0));
            }
            else
            {
                bubble.Position -= new Vector2(Dragon.Width, 0);
                bubble.Body.ApplyLinearImpulse(new Vector2(-startImpulse, 0));
            }

            return bubble;
        }
    }
}