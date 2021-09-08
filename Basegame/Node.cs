using System.Collections.Generic;
using DefaultEcs;

namespace Basegame {

	public class Node {
		public readonly Position Position;
		public readonly bool Solid;
		public readonly HashSet<Entity> Entities;

		public Coord Coord {
			get {
				return Position.Coord;
			}
		}

		public long X {
			get {
				return Position.Coord.X;
			}
		}

		public long Y {
			get {
				return Position.Coord.Y;
			}
		}

		public Node(Coord coord, bool solid) {
			Position = new Position { Coord = coord };
			Solid = solid;
			Entities = new HashSet<Entity>();
		}

		public override string ToString() => $"Node {Position.Coord.X}, {Position.Coord.Y}";

	}

}
