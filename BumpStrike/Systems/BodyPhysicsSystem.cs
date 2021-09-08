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

			var nx = body.Position.X + body.Velocity.X * dt;
			if (Grid.IsSolid(
				nx,
				body.Position.Y,
				body.Radius
			)) {
				if (body.Velocity.X > 0) {
					body.Position.X = Calc.Floor(nx + body.Radius) - body.Radius;
				} else if (body.Velocity.X < 0) {
					body.Position.X = Calc.Floor(nx - body.Radius) + body.Radius;
				}
				body.Velocity.X = 0;
			}
			
			var ny = body.Position.Y + body.Velocity.Y * dt;
			if (Grid.IsSolid(
				body.Position.X,
				ny,
				body.Radius
			)) {
				if (body.Velocity.Y > 0) {
					body.Position.Y = Calc.Floor(ny + body.Radius) - body.Radius;
				} else if (body.Velocity.Y < 0) {
					body.Position.Y = Calc.Floor(ny - body.Radius) + body.Radius;
				}
				body.Velocity.Y = 0;
			}

			body.Position += body.Velocity * dt;

			var bounds = Bounds.Create(body.Position, body.Radius);
			if (bounds == body.Bounds) return;
			Grid.Move(entity, body.Bounds, bounds);
			body.Bounds = bounds;
		}

	}

}
