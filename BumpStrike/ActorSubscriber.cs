using System;
using DefaultEcs;

namespace BumpStrike {

    public class ActorSubscriber {

        [Subscribe]
        void On(in Collision collision) {
            ref var actor = ref collision.A.Get<Actor>();
            if (!actor.IsReady) return;

            // ref var body = ref collision.A.Get<Body>();
            // ref var otherActor = ref collision.B.Get<Actor>();
            // ref var otherBody = ref collision.B.Get<Body>();

            actor.Cooldown = actor.Attack.Cooldown;
        }

    }

}
