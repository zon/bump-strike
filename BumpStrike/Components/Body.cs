using System.Numerics;
using Basegame;

namespace BumpStrike {

	public struct Body {
		public float Radius;
		public float Mass;
		public Vector2 Position;
		public Vector2 Velocity;
		public Vector2 Impluse;
		public Bounds Bounds;

		public static Body Create(float x, float y, float radius = 0.375f) {
			var position = new Vector2(x, y);
			return new Body {
				Radius = radius,
				Mass = 1,
				Position = position,
				Bounds = Bounds.Create(position, radius)
			};
		}

	}

}
