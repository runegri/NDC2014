using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBobble1.Win8
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
