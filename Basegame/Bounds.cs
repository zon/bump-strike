using System;
using System.Numerics;

namespace Basegame {

	public struct Bounds : IEquatable<Bounds> {
		public readonly Coord Min;
		public readonly Coord Max;

		public static Bounds Create(Vector2 position, float radius) {
			return new Bounds(
				new Coord(
					Calc.Floor(position.X - radius),
					Calc.Floor(position.Y - radius)
				),
				new Coord(
					Calc.Floor(position.X + radius),
					Calc.Floor(position.Y + radius)
				)
			);
		}

		public static bool operator ==(Bounds left, Bounds right) => left.Equals(right);

		public static bool operator !=(Bounds left, Bounds right) => !left.Equals(right);

		public Bounds(Coord min, Coord max) {
			Min = min;
			Max = max;
		}

		public bool Contains(long x, long y) {
			return
				x >= Min.X &&
				y >= Min.Y &&
				x <= Max.X &&
				y <= Max.Y;
		}

		public override int GetHashCode() => (int) (Min.GetHashCode() * 7649 + Max.GetHashCode() * 7541);

		public override bool Equals(object obj) => obj is Bounds other && Equals(other);
		
		public bool Equals(Bounds other) => Min == other.Min && Max == other.Max;

		public override string ToString() => $"Bounds: {Min.X}, {Min.Y}, {Max.X}, {Max.Y}";

	}

}
