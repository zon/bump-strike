using DefaultEcs;
using DefaultEcs.System;
using Basegame;
using System;

namespace BumpStrike {

	public class ActorPhysicsSystem : AEntitySetSystem<float> {
		readonly Grid Grid;

		public ActorPhysicsSystem(World world, Grid grid) : base(world
			.GetEntities()
			.With<Actor>()
			.AsSet()
		) {
			Grid = grid;
		}

		protected override void Update(float dt, in Entity entity) {
			ref var input = ref entity.Get<PlayerInput>();
			ref var actor = ref entity.Get<Actor>();

			if (input.MoveX != 0) {
				actor.Facing = input.MoveX;
			}

			var ax = input.MoveX * Actor.Accel;
			actor.DX += ax * dt;
			actor.DX += -actor.DX * Actor.Friction * dt;

			var ay = input.MoveY * Actor.Accel;
			actor.DY += ay * dt;
			actor.DY += -actor.DY * Actor.Friction * dt;

			var nx = actor.X + actor.DX * dt;
			if (Grid.IsSolid(
				nx,
				actor.Y,
				actor.Width,
				actor.Height
			)) {
				if (actor.DX > 0) {
					actor.X = Calc.Floor(nx + actor.Width) - actor.Width;
				} else if (actor.DX < 0) {
					actor.X = Calc.Floor(nx) + 1;
				}
				actor.DX = 0;
			}
			
			var dy = actor.DY * dt;
			var ny = actor.Y + actor.DY * dt;
			if (Grid.IsSolid(
				actor.X,
				Math.Min(actor.Y, actor.Y + dy),
				actor.Width,
				Math.Max(actor.Height, actor.Height +dy)
			)) {
				if (actor.DY > 0) {
					actor.Y = Calc.Floor(ny + actor.Height) - actor.Height;
				} else if (actor.DY < 0) {
					actor.Y = Calc.Floor(ny) + 1;
				}
				actor.DY = 0;
			}
			
			actor.X += actor.DX * dt;
			actor.Y += actor.DY * dt;
		}

	}

}
