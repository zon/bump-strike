using DefaultEcs;

namespace BumpStrike {

    public class ActorSubscriber {

        [Subscribe]
        void On(in Collision collision) {
            ref var actor = ref collision.A.Get<Actor>();
            ref var body = ref collision.A.Get<Body>();
            ref var otherActor = ref collision.A.Get<Actor>();
            ref var otherBody = ref collision.A.Get<Body>();
        }

    }

}
