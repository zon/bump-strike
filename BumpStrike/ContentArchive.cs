using Microsoft.Xna.Framework.Content;
using MonoGame.Aseprite.Documents;

namespace BumpStrike {

    public class ContentArchive {
		public readonly AsepriteDocument Player;
		public readonly AsepriteDocument Attacks;

        public ContentArchive(ContentManager content) {
            Player = content.Load<AsepriteDocument>("big-jumper");
            Attacks = content.Load<AsepriteDocument>("attacks");
        }

    }

}
