using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBobble1.Win8
{
    public static class Rnd
    {
        private static readonly Random Random = new Random();

        public static int Next()
        {
            return Random.Next();
        }

        public static int Next(int min, int max)
        {
            return Random.Next(min, max);
        }

        public static int Next(int max)
        {
            return Random.Next(0, max);
        }

        public static TEnum Next<TEnum>() where TEnum : struct
        {
            var values = Enum.GetValues(typeof(TEnum));
            return (TEnum)values.GetValue(Next(values.Length));
        }
    }
}
