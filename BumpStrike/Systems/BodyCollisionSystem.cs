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
				if (otherEntity == entity) continue;
				var other = otherEntity.Get<Body>();
				var minDistance = other.Radius + body.Radius;
				var delta = other.Position - body.Position;
				var distance = delta.Length();
				if (distance >= minDistance) continue;

				var overlap = minDistance - distance;
				var normal = Vector2.Normalize(delta);
				body.Impulse += (normal * -overlap / 2) / dt;

				var rv = other.Velocity - body.Velocity;
				if (Vector2.Dot(rv, normal) > 0) continue;
				
				var resitution = 1f;
				var invMass = -1f;
				var invMassSum = invMass + invMass;
				var n = -(1 + resitution) * Vector2.Dot(rv, normal);
				var j = n / invMassSum;
				var impulse = normal * j;

				body.Impulse += impulse * invMass * -1;
			}

		}

	}

}
