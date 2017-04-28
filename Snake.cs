using System;

namespace SnakeGame {

    public struct Location : IEquatable<Location> { 
		public byte X; public byte Y; 
		public bool Equals(Location other)  =>
			(X == other.X)	&& (Y == other.Y);
	}

    public class MovementResult {
        public Location NewHead;
        public Location OldHead;
        public Location MovedTail;
    }

    public class Snake {
        private Location[] Segments;
        private int HeadIdx;
        private int deltaX;
        private int deltaY;

        public Snake(Location init) {
            Segments = new Location[] { init };
            HeadIdx = 0;
            deltaX = 1;
            deltaY = 0;
        }
        
        public void Move(MovementResult mr, bool grow) {
            // save off the old head location, and compute the new one
            mr.OldHead = Segments[HeadIdx];
            mr.NewHead.X = (byte)(mr.OldHead.X + deltaX);
            mr.NewHead.Y = (byte)(mr.OldHead.Y + deltaY);

            // grow the array if necessary, and remember the last
            // segment of the tail.
            if(grow) {
                Array.Resize(ref Segments, Segments.Length+1);
                HeadIdx = HeadIdx + 1;
                if(HeadIdx < (Segments.Length -1)) {
                    Array.Copy(Segments, HeadIdx, Segments, HeadIdx+1, Segments.Length - HeadIdx - 1);
                }
                mr.MovedTail.X = 255;  // 255 == growing, ignore
                mr.MovedTail.Y = 255;  // 255 == growing, ignore
            } else {
                HeadIdx = HeadIdx + 1;
                if(HeadIdx == Segments.Length) {
                    HeadIdx = 0;
                }
                mr.MovedTail = Segments[HeadIdx];
            }

            // Set the new head location
            Segments[HeadIdx] = mr.NewHead;
        }

        public void ChangeDirection(int dx, int dy) {
            deltaX = dx;
            deltaY = dy;
        }

        private bool CollisionTest(Location target, bool skiphead=false) {
            bool found = false;
            for(var i = 0; i < Segments.Length; i++) {
                if( (i == HeadIdx) && skiphead ) continue;
                if(target.Equals(Segments[i])) {
                     found = true;
                     break;
                }
            }
            return found;
        }

        public bool Collision(Location tgt) => CollisionTest(tgt, false);
        public bool SelfCollision() => CollisionTest(Segments[HeadIdx], true);
    }
    
}
