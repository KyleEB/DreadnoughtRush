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
        /// If not using an fbx the texture will need to be imported.
        /// </summary> 
        
        protected Texture2D texture;

        /// <summary>
        /// BePu Physics entity for the model that will be added to the static space of DreadnoughtRush.
        /// This couples BePu and monogame together.
        /// </summary>
        
        public BEPUphysics.Entities.Prefabs.Sphere physicsObject;

        public BEPUphysics.Entities.Entity Entity => physicsObject;

        /// <summary>
        /// Camera that represents the Camera that resides behind the PlayerShip. 
        /// </summary>
        
        protected static Camera camera;

        /// <summary>
        /// Getter that converts the Bepu Vector 3 position of the BePu Entity returned as a XNA Vector 3
        /// </summary>
        public Vector3 CurrentPosition => ConversionHelper.MathConverter.Convert(physicsObject.BufferedStates.InterpolatedStates.Position);

        /// <summary>
        /// Getter that converts the Bepu Matrix3x3 orientation of the BePu Entity returned as a XNA Matrix
        /// </summary>
        public Matrix RotationMatrix => ConversionHelper.MathConverter.Convert(physicsObject.BufferedStates.InterpolatedStates.OrientationMatrix);

        public Matrix TranformationMatrix => ConversionHelper.MathConverter.Convert(physicsObject.BufferedStates.InterpolatedStates.WorldTransform);



        public GameObject(Game game) : base(game)
        {
            game.Components.Add(this);
        }

        public GameObject(Game game, Vector3 pos, string id) : this(game)
        {
            physicsObject = new BEPUphysics.Entities.Prefabs.Sphere(ConversionHelper.MathConverter.Convert(pos), 1);
            physicsObject.AngularDamping = 0f;
            physicsObject.LinearDamping = 0f;
            physicsObject.CollisionInformation.Events.InitialCollisionDetected += Events_InitialCollisionDetected;
            physicsObject.Tag = id;
            Game.Services.GetService<Space>().Add(physicsObject);
        }

        protected virtual void Events_InitialCollisionDetected(BEPUphysics.BroadPhaseEntries.MobileCollidables.EntityCollidable sender, BEPUphysics.BroadPhaseEntries.Collidable other, BEPUphysics.NarrowPhaseSystems.Pairs.CollidablePairHandler pair)
        {
            int i = 0;
        }

        public GameObject(Game game, Vector3 pos, string id, float mass) : this(game, pos, id)
        {
            physicsObject.Mass = mass;
        }

        public GameObject(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum) : this(game, pos, id, mass)
        {
            physicsObject.LinearMomentum = ConversionHelper.MathConverter.Convert(linMomentum);
        }

        public GameObject(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum, Vector3 angMomentum) : this(game, pos, id, mass, linMomentum)
        {
            physicsObject.AngularMomentum = ConversionHelper.MathConverter.Convert(angMomentum);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected void DrawMeshToCamera(ModelMesh mesh)
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

    }
}
