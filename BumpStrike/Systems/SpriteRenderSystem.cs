using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using Basegame.Client;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;

namespace BumpStrike {

	public class SpriteRenderSystem : AEntitySetSystem<float> {
		readonly CameraView Camera;

		public SpriteRenderSystem(World world, CameraView camera) : base(world
			.GetEntities()
			.With<Actor>()
			.With<Body>()
			.With<Sprite>()
			.AsSet()
		) {
			Camera = camera;
		}

		protected override void Update(float dt, in Entity entity) {
			ref var actor = ref entity.Get<Actor>();
			ref var body = ref entity.Get<Body>();
			ref var sprite = ref entity.Get<Sprite>();

			sprite.Update(dt);

			var tag = "stand";
			if (actor.MoveInput.X != 0 || actor.MoveInput.Y != 0) {
				tag = "walk";
				sprite.FrameRate = Math.Max(Math.Abs(body.Velocity.X), Math.Abs(body.Velocity.Y)) * 0.02f;
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
			Camera.Batch.DrawCircle(
				Camera.WorldToTarget(body.Position.ToXNA()),
				Camera.WorldToTarget(body.Radius, body.Radius).X,
				16,
				Color.Blue
			);
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
		}

	}

}
