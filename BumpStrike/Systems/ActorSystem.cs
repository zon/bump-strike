using System;
using DefaultEcs;
using DefaultEcs.System;

namespace BumpStrike {

	public class ActorSystem : AEntitySetSystem<float> {

		public ActorSystem(World world) : base(world
			.GetEntities()
			.With<Actor>()
			.AsSet()
		) {}

		protected override void Update(float dt, in Entity entity) {
			ref var actor = ref entity.Get<Actor>();

            actor.Cooldown = Math.Max(actor.Cooldown - dt, 0);
		}

	}

}
