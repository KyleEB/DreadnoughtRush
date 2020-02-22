using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DreadnoughtRush
{
    internal class Ship : GameObject
    {

        protected ThrustScalars thrusterScalars;
        public ThrustMovement Movement;
        protected float fireRate;
        protected float firingSideOffset; 
        protected float firingForce; 
        public int AmmoCount { get; private set; }

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
            float YawScalar = 2;
            float PitchScalar = 2;
            float RollScalar = 4;
            float ForwardThrustScalar = 4;
            float BackwardThrustScalar = 2;

            fireRate = 0.5f;
            firingSideOffset = 0;
            firingForce = 10;

            AmmoCount = 20;
            thrusterScalars = new ThrustScalars(YawScalar, PitchScalar, RollScalar, ForwardThrustScalar, BackwardThrustScalar);
            Movement = new ThrustMovement(this, thrusterScalars);
            this.physicsObject.LinearDamping = 0.3f;
            this.physicsObject.AngularDamping = 0.3f;
        }


        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            model = Game.Content.Load<Model>("spaceship");
            physicsObject.Radius = model.Meshes[0].BoundingSphere.Radius / 2;

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

        public void FireATorpedo(float dt)
        {
            if (fireRate > 0)
            {
                fireRate -= dt;
            }
            else
            {
                fireRate = 1;
                if (AmmoCount > 0)
                {
                    firingSideOffset *= -1;

                    float torpedoMass = 0.5f;
                    Vector3 torpedoLinear = ConversionHelper.MathConverter.Convert(firingForce * Entity.WorldTransform.Forward + Entity.LinearVelocity);
                    Vector3 torpedoAngular = Vector3.Zero; 
                    Vector3 firedFromPos = Vector3.Transform(new Vector3(firingSideOffset, -2f, 10), TranformationMatrix);

                    AmmoCount -= 1;
                    Torpedo fired = new Torpedo(this.Game, firedFromPos, "Torpedo", torpedoMass, torpedoLinear, torpedoAngular);

                    fired.Entity.Orientation = this.Entity.Orientation;
                    fired.Movement.ApplyForwardThrust(firingForce);
                }
            }
        }



    }
}
