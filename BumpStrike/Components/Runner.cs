using System.Numerics;

namespace BumpStrike {

	public struct Runner {
		public Vector2 MoveInput;
		public float Facing;
		public float AccelTime;
		public float MaxVelocity;
		public float Friction;
		public float Accel;

		public static Runner Create(float accelTime = 0.5f, float maxVelocity = 3) {
			var friction = 5 / accelTime;
			var accel = maxVelocity * friction;
			return new Runner {
				AccelTime = accelTime,
				MaxVelocity = maxVelocity,
				Friction = friction,
				Accel = accel
			};
		}

	}

}
