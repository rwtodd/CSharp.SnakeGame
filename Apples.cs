using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{

    public class Apples
    {
        private readonly System.Random rnd; /* random generator for placing apples */
        public Location Current { get; private set; }
        public Int32 Count { get; private set; }

        public Apples()
        {
            Count = -1;
            rnd = new System.Random();
        }

        public void GrowNew(Int32 w, Int32 h, Func<Location, bool> filter)
        {
            Current = randomLocations().SkipWhile(filter).First();
            Count++;

            IEnumerable<Location> randomLocations()
            {
                while (true)
                {
                    yield return new Location() { X = (Byte)rnd.Next(w), Y = (Byte)rnd.Next(h) };
                }
            }
        }
        public bool Eaten(Location l) => l.Equals(Current);
    }

} // end namespace
