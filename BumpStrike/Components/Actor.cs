namespace BumpStrike {

    public struct Actor {
        public Attack Attack;
        public float Cooldown;

        public bool IsReady => Cooldown <= 0;

    }

}
