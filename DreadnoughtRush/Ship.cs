using BEPUphysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DreadnoughtRush
{
    internal class Ship : GameObject
    {
        public Ship(Game game) : base(game)
        {
        }

        public Ship(Game game, Vector3 pos, string id) : base(game, pos, id)
        {
        }

        public Ship(Game game, Vector3 pos, string id, float mass) : base(game, pos, id, mass)
        {
        }

        public Ship(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum) : base(game, pos, id, mass, linMomentum)
        {
        }

        public Ship(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum, Vector3 angMomentum) : base(game, pos, id, mass, linMomentum, angMomentum)
        {
            physicsObject.AngularDamping = 0.5f;
            physicsObject.LinearDamping = 0.5f;
        }

        /// <summary>
        /// Apply forward thrust at the center of our ship, this means we are pushing it forward.
        /// </summary>
        /// <param name="dt">Amount of time passed in seconds, scales the power of the thrust.</param>
        public void ApplyForwardThrust(float dt)
        {
            Vector3 direction = new Vector3(0, 0, 1);
            ApplyThrust(direction, Vector3.Zero, dt); 
        }

        /// <summary>
        /// Apply backward thrust at the center of our ship, this means we are pushing it backward.
        /// </summary>
        /// <param name="dt">Amount of time passed in seconds, scales the power of the thrust.</param>
        public void ApplyBackwardThrust(float dt)
        {
            Vector3 direction = new Vector3(0, 0, -1);
            ApplyThrust(direction, Vector3.Zero, dt);
        }

        public void ApplyPositiveYawThrust(float dt)
        {
            Vector3 direction = new Vector3(-1, 0, 0);
            Vector3 localPosition = new Vector3(1, 0, 0);
            ApplyThrust(direction, localPosition, dt);
        }

        public void ApplyNegativeYawThrust(float dt)
        {
            Vector3 direction = new Vector3(1, 0, 0);
            Vector3 localPostion = new Vector3(1, 0, 0);
            ApplyThrust(direction, localPostion, dt);
        }

        public void ApplyPositivePitchThrust(float dt)
        {
            Vector3 direction = new Vector3(0, 1, 0);
            Vector3 localPosition = new Vector3(0, 0, 1);
            ApplyThrust(direction, localPosition, dt);
        }

        public void ApplyNegativePitchThrust(float dt)
        {
            Vector3 direction = new Vector3(0, -1, 0);
            Vector3 localPostion = new Vector3(0, 0, 1);
            ApplyThrust(direction, localPostion, dt);
        }

        public void ApplyPositiveRollThrust(float dt)
        {
            Vector3 direction =     new Vector3(0, -1, 0);
            Vector3 localPosition = new Vector3(-10, 0, 0);
            ApplyThrust(direction, localPosition, dt);
            ApplyThrust(-direction, -localPosition, dt);
        }

        public void ApplyNegativeRollThrust(float dt)
        {
            Vector3 direction =    new Vector3(0, -1, 0);
            Vector3 localPosition = new Vector3(10, 0, 0);
            ApplyThrust(direction, localPosition, dt);
            ApplyThrust(-direction, -localPosition, dt);
        }

        public void ApplyAngularDampeners()
        {
            physicsObject.AngularDamping = 1.5f;
        }

        public void ReleaseAngularDampeners()
        {
            physicsObject.AngularDamping = 0.5f;
        }

        /// <summary>
        /// Apply a general thrust to the ship in reference to a local position and direction and time.
        /// </summary>
        /// <param name="Direction">Vector in which we want to push towards</param>
        /// <param name="LocalPosition">The local position to apply the thrust, so we can thrust at offsets away from the center</param>
        /// <param name="dt">Amount of time passed in seconds</param>
        private void ApplyThrust(Vector3 Direction, Vector3 LocalPosition, float dt)
        {
            Vector3 worldPosition = Vector3.Transform(LocalPosition, TranformationMatrix);
            Vector3 worldDirection = Vector3.Transform(Direction, RotationMatrix);

            physicsObject.ApplyImpulse(ConversionHelper.MathConverter.Convert(worldPosition), ConversionHelper.MathConverter.Convert(worldDirection));
        
        }



        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            model = Game.Content.Load<Model>("spaceship");
            physicsObject.Radius = model.Meshes[0].BoundingSphere.Radius;

            base.LoadContent();
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
