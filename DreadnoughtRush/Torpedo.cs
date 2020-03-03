﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DreadnoughtRush
{
    internal class Torpedo : GameObject
    {

        protected ThrustScalars thrusterScalars;
        public ThrustMovement Movement;

        private double lifeTime = 20;

        public Torpedo(Game game) : base(game)
        {
        }

        public Torpedo(Game game, Vector3 pos, string id) : base(game, pos, id)
        {
        }

        public Torpedo(Game game, Vector3 pos, string id, float mass) : base(game, pos, id, mass)
        {
        }

        public Torpedo(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum) : base(game, pos, id, mass, linMomentum)
        {
        }

        public Torpedo(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum, Vector3 angMomentum) : base(game, pos, id, mass, linMomentum, angMomentum)
        {
            float YawScalar = 0;
            float PitchScalar = 0;
            float RollScalar = 0;
            float ForwardThrustScalar = 100;
            float BackwardThrustScalar = 0;
            thrusterScalars = new ThrustScalars(YawScalar, PitchScalar, RollScalar, ForwardThrustScalar, BackwardThrustScalar);
            Movement = new ThrustMovement(this, thrusterScalars);
            ((BEPUphysics.Entities.Prefabs.Sphere)physicsObject).Radius = model.Meshes[0].BoundingSphere.Radius; //had to move the radius set here as it was throwing null errors in load content.
        }

        protected override void Events_InitialCollisionDetected(BEPUphysics.BroadPhaseEntries.MobileCollidables.EntityCollidable sender, BEPUphysics.BroadPhaseEntries.Collidable other, BEPUphysics.NarrowPhaseSystems.Pairs.CollidablePairHandler pair)
        {
            if (other != null && other.Tag.ToString().StartsWith("Asteroid"))
            {
                physicsObject.Space.Remove(physicsObject);
                Visible = false;
            }
        }


        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            model = Game.Content.Load<Model>("torpedo");
 
            base.LoadContent();
        }


        public override void Draw(GameTime gameTime)
        {
            Random rand = new Random();
            lifeTime -= gameTime.ElapsedGameTime.TotalSeconds;
            if (lifeTime > 0)
            {
                new FireParticle(this.Game, Vector3.Transform(new Vector3(2 * (float)rand.NextDouble() - 1, 2 * (float)rand.NextDouble() - 1, -.5f), TranformationMatrix), "fireParticle", 1f, ConversionHelper.MathConverter.Convert(this.Entity.LinearMomentum / 10));
                foreach (var mesh in model.Meshes)
                {
                    DrawMeshToCamera(mesh);
                }
                base.Draw(gameTime);
            }
            else
            {
                physicsObject.Space.Remove(physicsObject);
                Visible = false;
            }
        }


    }
}
