using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreadnoughtRush
{
    internal class VictoryScreen : DrawableGameComponent
    {
        SpriteBatch spritebatch;
        public VictoryScreen(Game game) : base(game)
        {
            game.Components.Add(this);
        }

        public override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
    }
}
