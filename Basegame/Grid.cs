using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using DefaultEcs;

namespace Basegame {

	public class Grid {
		public readonly ImmutableDictionary<Coord, Node> Nodes;
		public readonly Spawn[] Spawns;

		public Grid(ImmutableDictionary<Coord, Node> nodes, Spawn[] spawns = null) {
			Nodes = nodes;
			Spawns = (spawns != null) ? spawns : new Spawn[0];
		}

		public Grid(LdtkWorld world) : this(world.GetNodes(), world.GetSpawns()) {}

		public Node Get(long x, long y) {
			return Get(new Coord(x, y));
		}

		public Node Get(Coord coord) {
			Node res;
			Nodes.TryGetValue(coord, out res);
			return res;
		}

		public bool IsSolid(Coord coord) {
			var node = Get(coord);
			if (node == null) {
				return true;
			} else {
				if (node.Solid) {
					return true;
				} else {
					return false;
				}
			}
		}

		public bool PointIsSolid(long x, long y) {
			return IsSolid(new Coord(x, y));
		}

		public bool RectIsSolid(float x, float y, float width, float height) {
			var ay = Calc.Floor(y);
			var by = Calc.Floor(y + height);
			if ((y + height) % 1 == 0) by -= 1;
			var ax = Calc.Floor(x);
			var bx = Calc.Floor(x + width);
			if ((x + width) % 1 == 0) bx -= 1;
			for (var yy = ay; yy <= by; yy++) {
				for (var xx = ax; xx <= bx; xx++) {
					if (PointIsSolid(xx, yy)) {
						return true;
					}
				}
			}
			return false;
		}

		public bool RadiusIsSolid(float x, float y, float radius) {
			var diameter = radius * 2;
			return RectIsSolid(x - radius, y - radius, diameter, diameter);
		}

		// https://stackoverflow.com/a/3706260
		public Node GetOpenNearby(EntityMap<Position> positions, long x, long y) {
			long vx = 1;
			long vy = 0;
			long len = 1;
			long ox = 0;
			long oy = 0;
			long p = 0;
			for (var _ = 0; _ < 64; _++) {

				var node = Get(x + ox, y + oy);
				if (node != null && !node.Solid) {
					if (!positions.ContainsKey(node.Position)) return node;
				}

				ox += vx;
				oy += vy;
				p += 1;
				if (p >= len) {
					p = 0;
					var f = vx;
					vx = -vy;
					vy = f;
					if (vy == 0) {
						len += 1;
					}
				}
			}
			return null;
		}

		public Node GetOpenNearby(EntityMap<Position> positions, Coord coord) {
			return GetOpenNearby(positions, coord.X, coord.Y);
		}

		public void Add(Entity entity, Bounds bounds) {
			for (var y = bounds.Min.Y; y <= bounds.Max.Y; y++) {
				for (var x = bounds.Min.X; x <= bounds.Max.X; x++) {
					var node = Get(x, y);
					node.Entities.Add(entity);
				}
			}
		}

		public HashSet<Entity> Get(Bounds bounds) {
			var result = new HashSet<Entity>();
			for (var y = bounds.Min.Y; y <= bounds.Max.Y; y++) {
				for (var x = bounds.Min.X; x <= bounds.Max.X; x++) {
					var node = Get(x, y);
					foreach (var entity in node.Entities) {
						result.Add(entity);
					}
				}
			}
			return result;
		}

		public void Move(Entity entity, Bounds from, Bounds to) {
			for (var y = from.Min.Y; y <= from.Max.Y; y++) {
				for (var x = from.Min.X; x <= from.Max.X; x++) {
					if (to.Contains(x, y)) continue;
					var node = Get(x, y);
					node.Entities.Remove(entity);
				}
			}
			for (var y = to.Min.Y; y <= to.Max.Y; y++) {
				for (var x = to.Min.X; x <= to.Max.X; x++) {
					if (from.Contains(x, y)) continue;
					var node = Get(x, y);
					node.Entities.Add(entity);
				}
			}
		}

		public void Remove(Entity entity, Bounds bounds) {
			for (var y = bounds.Min.Y; y <= bounds.Max.Y; y++) {
				for (var x = bounds.Min.X; x <= bounds.Max.X; x++) {
					var node = Get(x, y);
					node.Entities.Remove(entity);
				}
			}
		}

	}

}
