using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBobble.MacOS
{
    public interface IGameInput : IUpdateable
    {
        MoveDirection Direction { get; }
        bool Jumping { get; }
        bool BlowBubble { get; }
    }

    public enum MoveDirection
    {
        None,
        Left, 
        Right
    }
}
