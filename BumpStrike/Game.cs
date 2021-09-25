using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Documents;
using DefaultEcs;
using DefaultEcs.System;
using Basegame;
using Basegame.Client;
using System;

namespace BumpStrike {

	public class Game : Microsoft.Xna.Framework.Game {
		World World;
		GraphicsDeviceManager Graphics;
		ContentArchive Archive;
		LevelResources LevelResources;
		Grid Grid;
		ISystem<float> Logic;
		ISystem<float> BackgroundRendering;
		ISystem<float> ForegroundRendering;
		ActorSubscriber ActorSubscriber;
		IDisposable ActorSubscription;
		CameraView Camera;
		SpriteBatch Result;

		public Game() {
			Graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize() {
			World = new World();

			ActorSubscriber = new ActorSubscriber();
			ActorSubscription = World.Subscribe(ActorSubscriber);

			base.Initialize();
		}

		protected override void LoadContent() {
			Archive = new ContentArchive(Content);

			LevelResources = LevelResources.Load(Content, "test");
			Grid = new Grid(LevelResources.World);

			Logic = new SequentialSystem<float>(
				new PlayerInputSystem(World),
				new ActorSystem(World),
				new RunnerSystem(World),
				new BodyCollisionSystem(World, Grid),
				new BodyPhysicsSystem(World, Grid)
			);

			var width = 256;
			var height = 256;
			Camera = new CameraView(Window, GraphicsDevice, new Point(width, height), 16, 4);
			BackgroundRendering = new LdtkDrawSystem(LevelResources, Camera);
			ForegroundRendering  = new SequentialSystem<float>(
				new ActorRenderSystem(World, Archive, Camera)
			);

			Camera.SetWindow(Graphics);
			
			Result = new SpriteBatch(GraphicsDevice);

			var player = World.CreateEntity();
			player.Set(Body.Create(8, 8));
			player.Set(Runner.Create());
			player.Set(new Actor {
				Attack = new Attack {
					Cooldown = 1
				}
			});
			player.Set(new Player());
			player.Set(new ActorView());
			player.Set(Sprite.Create(Archive.Player, "stand"));

			var other = World.CreateEntity();
			other.Set(Body.Create(6, 8));
			other.Set(Runner.Create());
			other.Set(new Actor {
				Attack = new Attack {
					Cooldown = 1
				}
			});
			other.Set(new ActorView());
			other.Set(Sprite.Create(Archive.Player, "stand"));
		}

		protected override void Update(GameTime gameTime) {
			var dt = (float) gameTime.ElapsedGameTime.TotalSeconds;
			Logic.Update(dt);
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime) {
			var dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

			GraphicsDevice.Clear(Color.Black);

			GraphicsDevice.SetRenderTarget(Camera.RenderTarget);
			GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			GraphicsDevice.Clear(Color.Black);

			BackgroundRendering.Update(dt);

			Camera.Batch.Begin(
				transformMatrix: Matrix.Identity,
				samplerState: SamplerState.PointClamp,
				sortMode: SpriteSortMode.FrontToBack
			);
			ForegroundRendering.Update(dt);
			Camera.Batch.End();
			
			GraphicsDevice.SetRenderTarget(null);

			Result.Begin(samplerState: SamplerState.PointClamp);
			Result.Draw(
				texture: Camera.RenderTarget,
				position: Vector2.Zero,
				sourceRectangle: null,
				color: Color.White,
				rotation: 0,
				origin: Vector2.Zero,
				scale: Vector2.One * Camera.Zoom,
				effects: SpriteEffects.None,
				layerDepth: 0
			);
			Result.End();

			base.Draw(gameTime);
		}
	}
}
