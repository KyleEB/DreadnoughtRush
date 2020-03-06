using BEPUphysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreadnoughtRush
{
     abstract class GameObject : DrawableGameComponent
     {
        /// <summary>
        /// Collection of meshes and textures that make up the model, currently only using fbx files. 
        /// </summary>
        
        protected Model model;

        /// <summary>
        /// If not using an fbx with a texture the texture will need to be imported.
        /// </summary> 
        
        protected Texture2D texture;

        /// <summary>
        /// BePu Physics entity for the model that will be added to the static space of DreadnoughtRush.
        /// This couples BePu and monogame together.
        /// </summary>

        public BEPUphysics.Entities.Entity entity;

        /// <summary>
        /// Camera that represents the Camera that resides behind the PlayerShip. 
        /// </summary>
        
        protected static Camera camera;

        /// <summary>
        /// Getter that converts the Bepu Vector 3 position of the BePu Entity returned as a XNA Vector 3
        /// </summary>
        public Vector3 CurrentPosition => ConversionHelper.MathConverter.Convert(entity.BufferedStates.InterpolatedStates.Position);

        /// <summary>
        /// Getter that converts the Bepu Matrix3x3 orientation of the BePu Entity returned as a XNA Matrix
        /// </summary>
        public Matrix RotationMatrix => ConversionHelper.MathConverter.Convert(entity.BufferedStates.InterpolatedStates.OrientationMatrix);

        public Matrix TranformationMatrix => ConversionHelper.MathConverter.Convert(entity.BufferedStates.InterpolatedStates.WorldTransform);



        public GameObject(Game game) : base(game)
        {
            game.Components.Add(this);
        }

        public GameObject(Game game, Vector3 pos, string id) : this(game)
        {
            entity = new BEPUphysics.Entities.Prefabs.Sphere(ConversionHelper.MathConverter.Convert(pos), 1);
            entity.AngularDamping = 0f;
            entity.LinearDamping = 0f;
            entity.CollisionInformation.Events.InitialCollisionDetected += Events_InitialCollisionDetected;
            entity.CollisionInformation.Tag = id;
            entity.Tag = id;
            Game.Services.GetService<Space>().Add(entity);
        }

        protected virtual void Events_InitialCollisionDetected(BEPUphysics.BroadPhaseEntries.MobileCollidables.EntityCollidable sender, BEPUphysics.BroadPhaseEntries.Collidable other, BEPUphysics.NarrowPhaseSystems.Pairs.CollidablePairHandler pair)
        {
        }

        public GameObject(Game game, Vector3 pos, string id, float mass) : this(game, pos, id)
        {
            entity.Mass = mass;
        }

        public GameObject(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum) : this(game, pos, id, mass)
        {
            entity.LinearMomentum = ConversionHelper.MathConverter.Convert(linMomentum);
        }

        public GameObject(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum, Vector3 angMomentum) : this(game, pos, id, mass, linMomentum)
        {
            entity.AngularMomentum = ConversionHelper.MathConverter.Convert(angMomentum);
        }

        protected virtual void DrawMeshToCamera(ModelMesh mesh)
        {
            camera = Game.Services.GetService<Camera>();
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.Alpha = 1f;
                               
                effect.EnableDefaultLighting();

                effect.PreferPerPixelLighting = true;
                
                effect.World = this.TranformationMatrix;

                effect.View = ConversionHelper.MathConverter.Convert(camera.ViewMatrix);

                effect.Projection = ConversionHelper.MathConverter.Convert(camera.ProjectionMatrix);
            }
            mesh.Draw();
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var mesh in model.Meshes)
            {
                DrawMeshToCamera(mesh);
            }
            base.Draw(gameTime);
        }
    }
}
