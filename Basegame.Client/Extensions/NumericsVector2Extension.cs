using System.Numerics;

namespace Basegame.Client {

	public static class NumericsVector2Extension {

		public static Microsoft.Xna.Framework.Vector2 ToXNA(this Vector2 vector) {
			return new Microsoft.Xna.Framework.Vector2(vector.X, vector.Y);
		}

	}

}
