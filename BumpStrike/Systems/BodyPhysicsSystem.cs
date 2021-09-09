using DefaultEcs;
using DefaultEcs.System;
using Basegame;
using System.Numerics;

namespace BumpStrike {

	public class BodyPhysicsSystem : AEntitySetSystem<float> {
		readonly Grid Grid;

		public BodyPhysicsSystem(World world, Grid grid) : base(world
			.GetEntities()
			.With<Body>()
			.AsSet()
		) {
			Grid = grid;
		}

		protected override void Update(float dt, in Entity entity) {
			ref var body = ref entity.Get<Body>();
			
			body.Velocity += body.Impluse;
			body.Impluse = Vector2.Zero;

			var position = body.Position;
			var velocity = body.Velocity;
			var next = body.Position + body.Velocity * dt;

			if (Grid.RadiusIsSolid(
				next.X,
				position.Y,
				body.Radius
			)) {
				if (velocity.X > 0) {
					position.X = Calc.Floor(next.X + body.Radius) - body.Radius;
				} else if (velocity.X < 0) {
					position.X = Calc.Floor(next.X - body.Radius) + 1 + body.Radius;
				}
				velocity.X = 0;
			}
			
			if (Grid.RadiusIsSolid(
				position.X,
				next.Y,
				body.Radius
			)) {
				if (velocity.Y > 0) {
					position.Y = Calc.Floor(next.Y + body.Radius) - body.Radius;
				} else if (velocity.Y < 0) {
					position.Y = Calc.Floor(next.Y - body.Radius) + 1 + body.Radius;
				}
				velocity.Y = 0;
			}

			body.Position = position + velocity * dt;

			var bounds = Bounds.Create(body.Position, body.Radius);
			if (bounds == body.Bounds) return;
			Grid.Move(entity, body.Bounds, bounds);
			body.Bounds = bounds;
		}

	}

}
