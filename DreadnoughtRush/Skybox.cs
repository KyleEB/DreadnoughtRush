using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreadnoughtRush
{
    public class Skybox : DrawableGameComponent
    {
        /// <summary>
        /// The skybox model, which will just be a cube
        /// </summary>
        private Model model;

        /// <summary>
        /// The size of the cube, used so that we can resize the box
        /// for different sized environments.
        /// </summary>
        private float size = 10f;

        /// <summary>
        /// Creates a new skybox
        /// </summary>
        /// <param name="skyboxTexture">the name of the skybox texture to use</param>
        public Skybox(Game game) : base(game)
        {
            game.Components.Add(this);
            model = Game.Content.Load<Model>("skybox");
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var mesh in model.Meshes)
            {
                DrawMeshToCamera(mesh);
            }
            base.Draw(gameTime);
        }


        protected virtual void DrawMeshToCamera(ModelMesh mesh)
        {
            Camera camera = Game.Services.GetService<Camera>();

            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.Alpha = 0.5f;

                //effect.EnableDefaultLighting();

                //effect.PreferPerPixelLighting = true;

                effect.World = Matrix.CreateScale(size) * Matrix.CreateTranslation(ConversionHelper.MathConverter.Convert(camera.Position));

                effect.View = ConversionHelper.MathConverter.Convert(camera.ViewMatrix);

                effect.Projection = ConversionHelper.MathConverter.Convert(camera.ProjectionMatrix);
            }
            mesh.Draw();
        }
    }
}
