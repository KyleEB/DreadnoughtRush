using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DreadnoughtRush
{
    internal class Particle : GameObject
    {
        public double LifeTime = 1;

        public Particle(Game game) : base(game)
        {
            
        }

        public Particle(Game game, Vector3 pos, string id) : base(game, pos, id)
        {
            physicsObject.CollisionInformation.CollisionRules.Personal = BEPUphysics.CollisionRuleManagement.CollisionRule.NoSolver; //do nothing with collisions
        }

        public Particle(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum) : base(game, pos, id, mass, linMomentum)
        {
            physicsObject.CollisionInformation.CollisionRules.Personal = BEPUphysics.CollisionRuleManagement.CollisionRule.NoSolver; //do nothing with collisions
        }

        public override void Initialize()
        {
            base.Initialize();
            
        }


        public override void Draw(GameTime gameTime)
        {
            LifeTime -= gameTime.ElapsedGameTime.TotalSeconds;
            if (LifeTime > 0)
            {
                foreach (var mesh in model.Meshes)
                {
                    DrawMeshToCamera(mesh);
                }
                base.Draw(gameTime);
            }
            else
            {
                this.physicsObject.Space.Remove(this.physicsObject);
                this.Visible = false;
            }
        }

        protected override void DrawMeshToCamera(ModelMesh mesh)
        {
            camera = Game.Services.GetService<Camera>();
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.Alpha = 1f;
                //effect.EnableDefaultLighting();
                effect.TextureEnabled = true;
                effect.Texture = texture;

                effect.PreferPerPixelLighting = true;

                effect.World = this.TranformationMatrix;

                effect.View = ConversionHelper.MathConverter.Convert(camera.ViewMatrix);

                effect.Projection = ConversionHelper.MathConverter.Convert(camera.ProjectionMatrix);
            }
            mesh.Draw();
        }
       
    }
}
