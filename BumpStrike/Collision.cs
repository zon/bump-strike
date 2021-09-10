using DefaultEcs;

namespace BumpStrike {

	public struct Collision {
		public readonly Entity A;
		public readonly Entity B;

		public Collision(Entity a, Entity b) {
			A = a;
			B = b;
		}

	}

}
