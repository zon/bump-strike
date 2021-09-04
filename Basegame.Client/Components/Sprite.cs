using MonoGame.Aseprite.Documents;
using Microsoft.Xna.Framework;
using Basegame;

namespace Basegame.Client {

	public struct Sprite {
		public AsepriteDocument Document;
		public AsepriteTag Tag;
		public int FrameIndex;
		public float FrameRate;
		public float Interval;

		public static Sprite Create(AsepriteDocument document, string tag) {
			return new Sprite {
				Document = document,
				Tag = document.Tags[tag],
				FrameRate = 0.08f,
			};
		}

		public Rectangle GetFrame() {
			return Document.Frames[Tag.From + FrameIndex].ToRectangle();
		}

		public void Play(string tag) {
			Tag = Document.Tags[tag];
			FrameIndex = 0;
			Interval = 0;
		}

		public void Update(float dt) {
			if (FrameRate <= 0.001f) return;
			Interval += dt / FrameRate;
			if (Interval < 1) return;
			var d = Calc.Floor(Interval);
			Interval = Interval % 1;
			var length = Tag.To - Tag.From + 1;
			FrameIndex = (FrameIndex + d) % length;
		}

	}

}
