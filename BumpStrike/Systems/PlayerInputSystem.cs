using System.Numerics;
using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework.Input;

namespace BumpStrike {

	public class PlayerInputSystem : AEntitySetSystem<float> {

		public PlayerInputSystem(World world) : base(world
			.GetEntities()
			.With<Player>()
			.With<Actor>()
			.AsSet()
		) {}

		protected override void Update(float dt, in Entity entity) {
			ref var actor = ref entity.Get<Actor>();

			var state = Keyboard.GetState();

			var move = new Vector2();

			if (state.IsKeyDown(Keys.A)) {
				move.X = -1;
			} else if (state.IsKeyDown(Keys.D)) {
				move.X = 1;
			} else {
				move.X = 0;
			}

			if (state.IsKeyDown(Keys.W)) {
				move.Y = -1;
			} else if (state.IsKeyDown(Keys.S)) {
				move.Y = 1;
			} else {
				move.Y = 0;
			}

			actor.MoveInput = move;
		}

	}

}
