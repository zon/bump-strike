using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using Basegame.Client;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace BumpStrike {

	public class ActorRenderSystem : AEntitySetSystem<float> {
		readonly CameraView Camera;

		public ActorRenderSystem(World world, CameraView camera) : base(world
			.GetEntities()
			.With<ActorView>()
			.With<Sprite>()
			.With<Actor>()
			.With<Body>()
			.AsSet()
		) {
			Camera = camera;
		}

		protected override void Update(float dt, in Entity entity) {
			ref var view = ref entity.Get<ActorView>();
			ref var sprite = ref entity.Get<Sprite>();
			ref var actor = ref entity.Get<Actor>();
			ref var body = ref entity.Get<Body>();

			sprite.Update(dt);

			var tag = "stand";
			if (actor.MoveInput.X != 0 || actor.MoveInput.Y != 0) {
				tag = "walk";
			}
			if (sprite.Tag.Name != tag) {
				sprite.Play(tag);
			}
			
			var effects = SpriteEffects.None;
			if (actor.Facing < 0) {
				effects = SpriteEffects.FlipHorizontally;
			}

			var frame = sprite.GetFrame();
			var size = Camera.WorldToTarget(body.Radius, body.Radius) * 2;
			var position = Camera.WorldToTarget(body.Position.ToXNA()) + new Vector2(
				-frame.Width / 2,
				size.Y / 2 - frame.Height
			);

			if (sprite.Tag.Name == "walk") {
				var speed = (position - view.Previous).Length() / dt;
				sprite.FrameRate = speed / 2;
			}
			view.Previous = position;

			// Camera.Batch.DrawCircle(
			// 	Camera.WorldToTarget(body.Position.ToXNA()),
			// 	Camera.WorldToTarget(body.Radius, body.Radius).X,
			// 	16,
			// 	Color.Blue
			// );

			Camera.Batch.Draw(
				texture: sprite.Document.Texture, 
				position: position,
				sourceRectangle: frame,
				color: Color.White,
				rotation: 0,
				origin: Vector2.Zero,
				scale: 1,
				effects: effects,
				layerDepth: 0
			);

			// var min = Camera.WorldToTarget(body.Bounds.Min.X, body.Bounds.Min.Y);
			// var max = Camera.WorldToTarget(body.Bounds.Max.X + 1, body.Bounds.Max.Y + 1);
			// Camera.Batch.DrawRectangle(
			// 	min,
			// 	max - min,
			// 	Color.Yellow
			// );
		}

	}

}
