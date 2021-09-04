using System;
using Microsoft.Xna.Framework;
using Basegame;

namespace Basegame.Client {

	public static class View {
		public const int SCALE = 4;
		public const int TILE = 16;
		public const float RADIAN = MathF.PI * 2;

		public static Vector2 ToVector(float radians) {
			return new Vector2(MathF.Cos(radians), MathF.Sin(radians));
		}

		public static float ToRadians(Vector2 vector) {
			return MathF.Atan2(vector.Y, vector.X);
		}

		public static float ToRadians(Coord coord) {
			return MathF.Atan2(coord.Y, coord.X);
		}

		public static Point ToPoint(long[] pair) {
			return new Point((int) pair[0], (int) pair[1]);
		}

	}

}
