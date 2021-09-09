using DefaultEcs;
using DefaultEcs.System;
using Basegame;
using System.Numerics;

namespace BumpStrike {

	public class ActorPhysicsSystem : AEntitySetSystem<float> {

		public ActorPhysicsSystem(World world) : base(world
			.GetEntities()
			.With<Actor>()
            .With<Body>()
			.AsSet()
		) {}

		protected override void Update(float dt, in Entity entity) {
			ref var body = ref entity.Get<Body>();
			ref var actor = ref entity.Get<Actor>();

            body.Velocity += actor.MoveInput * actor.Accel * dt;
            body.Velocity += -body.Velocity * actor.Friction * dt;

            if (actor.MoveInput.X != 0) {
				actor.Facing = actor.MoveInput.X;
			}
		}

	}

}
