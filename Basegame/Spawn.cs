using ldtk;

namespace Basegame {

	public class Spawn {
		public readonly Coord Coord;
		public readonly EntityInstance Entity;

		public Spawn(Coord coord, EntityInstance entity) {
			Coord = coord;
			Entity = entity;
		}

	}

}
