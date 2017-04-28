using System;

namespace SnakeGame {

public class Apples {
	private System.Random rnd;
	public Location Current { get ; private set; }

	public Int32 Count { get ; private set; }

	public Apples() {
		Count = -1;
		rnd = new System.Random();
	}

	public void GrowNew(Int32 w, Int32 h, Func<Location,bool> filter ) {
		Location possible = new Location() {
			X = (Byte)rnd.Next(w),
			Y = (Byte)rnd.Next(h)
		};
		if( filter(possible) ) {
			GrowNew(w,h,filter);
		} else {
			Current = possible;
			Count++;
		}
	}

	public bool Eaten(Location l) => l.Equals(Current);
}

} // end namespace
