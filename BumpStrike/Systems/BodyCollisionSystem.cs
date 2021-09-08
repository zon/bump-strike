using DefaultEcs;
using DefaultEcs.System;
using Basegame;
using System.Numerics;

namespace BumpStrike {

	public class BodyCollisionSystem : AEntitySetSystem<float> {
		readonly Grid Grid;

		public BodyCollisionSystem(World world, Grid grid) : base(world
			.GetEntities()
			.With<Body>()
			.AsSet()
		) {
			Grid = grid;
		}

		protected override void Update(float dt, in Entity entity) {
			ref var body = ref entity.Get<Body>();

			var entities = Grid.Get(body.Bounds);
			foreach (var otherEntity in entities) {
				var other = otherEntity.Get<Body>();
				var minDistance = other.Radius + body.Radius;
				var delta = other.Position - body.Position;
				var distanceSqr = delta.LengthSquared();
				if (distanceSqr >= minDistance * minDistance) continue;
				var rv = other.Velocity - body.Velocity;
				if (Vector2.Dot(rv, delta) < 0) continue;
				body.Impluse -= rv;
			}

		}

	}

}
