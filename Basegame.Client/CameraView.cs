using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Basegame.Client {

	public class CameraView : IDisposable {
		public readonly float WorldScale;
		public readonly SpriteBatch Batch;
		public readonly RenderTarget2D RenderTarget;

		readonly GameWindow Window;
		readonly GraphicsDevice GraphicsDevice;

		Vector2 _Position;
		float _Zoom;

		public Matrix Matrix { get; private set; }
		public Matrix InverseMatrix { get; private set; }
		public Matrix ScreenMatrix { get; private set; }
		public Matrix InverseScreenMatrix { get; private set; }

		public Vector2 Position {
			get => _Position;
			set {
				_Position = value;
				UpdateMatrix();
			}
		}

		public float Zoom {
			get => _Zoom;
			set {
				_Zoom = value;
				UpdateScreenMatrix();
			}
		}

		public CameraView(
			GameWindow window,
			GraphicsDevice graphicsDevice,
			Point targetSize,
			float worldScale,
			float zoom
		) {
			Window = window;
			GraphicsDevice = graphicsDevice;
			WorldScale = worldScale;
			_Position = Vector2.Zero;
			_Zoom = zoom;

			Batch = new SpriteBatch(graphicsDevice);
			RenderTarget = new RenderTarget2D(
				graphicsDevice: graphicsDevice,
				width: targetSize.X,
				height: targetSize.Y,
				mipMap: false,
				preferredFormat: graphicsDevice.PresentationParameters.BackBufferFormat,
				preferredDepthFormat: DepthFormat.Depth24
			);

			UpdateMatrix();
		}

		public void Dispose() {
			Batch.Dispose();
			RenderTarget.Dispose();
		}

		public void SetWindow(GraphicsDeviceManager graphics) {
			graphics.PreferredBackBufferWidth = Calc.Floor(RenderTarget.Width * Zoom);
			graphics.PreferredBackBufferHeight = Calc.Floor(RenderTarget.Height * Zoom);
			graphics.ApplyChanges();
		}

		public Vector2 WorldToTarget(Vector2 worldPosition) {
			return Vector2.Transform(worldPosition, Matrix);
		}

		public Vector2 TargetToWorld(Vector2 targetPosition) {
			return Vector2.Transform(targetPosition, InverseMatrix);
		}

		public Vector2 WorldToScreen(Vector2 worldPosition) {
			return Vector2.Transform(worldPosition, ScreenMatrix);
		}

		public Vector2 ScreenToWorld(Vector2 screenPosition) {
			return Vector2.Transform(screenPosition, InverseScreenMatrix);
		}

		public Vector2 WorldToTarget(float x, float y) => WorldToTarget(new Vector2(x, y));
		public Vector2 TargetToWorld(float x, float y) => TargetToWorld(new Vector2(x, y));
		public Vector2 WorldToScreen(float x, float y) => WorldToScreen(new Vector2(x, y));
		public Vector2 ScreenToWorld(float x, float y) => ScreenToWorld(new Vector2(x, y));

		void UpdateMatrix() {
			Matrix = Matrix.CreateScale(WorldScale, WorldScale, 1);
			InverseMatrix = Matrix.Invert(Matrix);
			UpdateScreenMatrix();
		}

		void UpdateScreenMatrix() {
			ScreenMatrix =
				Matrix.CreateTranslation(new Vector3(Position, 0)) *
				Matrix *
				Matrix.CreateScale(Zoom, Zoom, 1);
			InverseScreenMatrix = Matrix.Invert(ScreenMatrix);
		}

	}

}
