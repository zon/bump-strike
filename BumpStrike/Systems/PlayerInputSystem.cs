using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework.Input;

namespace BumpStrike {

	public class PlayerInputSystem : AComponentSystem<float, PlayerInput> {

		public PlayerInputSystem(World world) : base(world) {}

		protected override void Update(float dt, ref PlayerInput input) {
			var state = Keyboard.GetState();

			if (state.IsKeyDown(Keys.A)) {
				input.MoveX = -1;
			} else if (state.IsKeyDown(Keys.D)) {
				input.MoveX = 1;
			} else {
				input.MoveX = 0;
			}

			if (state.IsKeyDown(Keys.W)) {
				input.MoveY = -1;
			} else if (state.IsKeyDown(Keys.S)) {
				input.MoveY = 1;
			} else {
				input.MoveY = 0;
			}
		}

	}

}
