using System;

namespace Basegame {

	public static class Calc {

		public static int Max(int a, int b) {
			return a > b ? a : b;
		}

		public static int Floor(float v) {
			return Convert.ToInt32(Math.Floor(v));
		}

		public static int Ceiling(float v) {
			return Convert.ToInt32(Math.Ceiling(v));
		}

		public static float Sqrt(float v) {
			return Convert.ToInt32(Math.Sqrt(v));
		}

		public static float Dot(float ax, float ay, float bx, float by) {
			return ax * bx + ay * by;
		}

		public static float Progress(float progress, float duration, float dt) {
			return Math.Clamp(progress + dt / duration, 0, 1);
		}

		public static float DistanceSqr(float x, float y) {
			return x * x + y * y;
		}

		public static float Distance(float x, float y) {
			return Sqrt(DistanceSqr(x, y));
		}
		
	}

}
