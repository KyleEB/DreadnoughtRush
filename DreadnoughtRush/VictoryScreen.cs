using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreadnoughtRush
{
    internal class VictoryScreen : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        private SpriteFont font;
        string displayText;

        public VictoryScreen(Game game) : base(game)
        {
            game.Components.Add(this);
            displayText = "You've Reached The Mothership \n CONGRATULATIONS!";
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, displayText, new Vector2(600, 800), Color.White);
            spriteBatch.End();
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
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
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            font = Game.Content.Load<SpriteFont>("displayText");
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
    }
}
