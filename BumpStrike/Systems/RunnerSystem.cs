using DefaultEcs;
using DefaultEcs.System;

namespace BumpStrike {

	public class RunnerSystem : AEntitySetSystem<float> {

		public RunnerSystem(World world) : base(world
			.GetEntities()
			.With<Runner>()
            .With<Body>()
			.AsSet()
		) {}

		protected override void Update(float dt, in Entity entity) {
			ref var body = ref entity.Get<Body>();
			ref var actor = ref entity.Get<Runner>();

            body.Velocity += actor.MoveInput * actor.Accel * dt;
            body.Velocity += -body.Velocity * actor.Friction * dt;

            if (actor.MoveInput.X != 0) {
				actor.Facing = actor.MoveInput.X;
			}
		}

	}

}
