using BEPUphysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DreadnoughtRush
{
    internal class Asteroid : GameObject
    {

        public Asteroid(Game game) : base(game)
        {
        }

        public Asteroid(Game game, Vector3 pos, string id) : base(game, pos, id)
        {
        }

        public Asteroid(Game game, Vector3 pos, string id, float mass) : base(game, pos, id, mass)
        {
        }

        public Asteroid(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum) : base(game, pos, id, mass, linMomentum)
        {
        }

        public Asteroid(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum, Vector3 angMomentum) : base(game, pos, id, mass, linMomentum, angMomentum)
        {
        }

        public Asteroid(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum, Vector3 angMomentum, GameObject trigger) : this(game, pos, id, mass, linMomentum, angMomentum)
        {
        }

        protected override void Events_InitialCollisionDetected(BEPUphysics.BroadPhaseEntries.MobileCollidables.EntityCollidable sender, BEPUphysics.BroadPhaseEntries.Collidable other, BEPUphysics.NarrowPhaseSystems.Pairs.CollidablePairHandler pair)
        {
            string tempTag = string.Empty;

            if(other != null && other.Tag != null)
            {
                tempTag = other.Tag.ToString();
            }

            if(tempTag.Equals("Player") || tempTag.Equals("Torpedo") || tempTag.StartsWith("Asteroid"))
            {
                if (physicsObject.Space != null)
                {
                    physicsObject.Space.Remove(physicsObject);
                    Visible = false;
                }
                if (tempTag.Equals("Torpedo"))
                {
                    BrokenAsteroidParticle.CreateParticleBoom(this.Game, CurrentPosition, 20, 40);
                }
            }
        }


        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            model = Game.Content.Load<Model>("mine");
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

        public static Asteroid[] CreateAsteroidField(Game game, int asteroidCount, float densityFactor)
        {
            Vector3 AsteroidPos = new Vector3(-250, -250, -250);
            string AsteroidId = "Asteroid";
            float AsteroidMass = 3f;
            Vector3 AsteroidLinearMomentum = new Vector3(0f, 0f, 0f);
            Vector3 AsteroidAngularMomentum = new Vector3(0f, 0f, 0f);

            Random random = new Random();

            Asteroid[] TempAsteroidField = new Asteroid[asteroidCount];
            for (int i = 0; i < asteroidCount; i++)
            {
                float xPos =  asteroidCount * (float)random.NextDouble() / densityFactor;
                float yPos =  asteroidCount * (float)random.NextDouble() / densityFactor;
                float zPos =  asteroidCount * (float)random.NextDouble() / densityFactor;

                float xRotate = 1 * (float)random.NextDouble() - 1;
                float yRotate = 1 * (float)random.NextDouble() - 1;
                float zRotate = 1 * (float)random.NextDouble() - 1;

                float xLinear = 10 * (float)random.NextDouble() - 10;
                float yLinear = 10 * (float)random.NextDouble() - 10;
                float zLinear = 10 * (float)random.NextDouble() - 10;

                Vector3 randomPosition = new Vector3(xPos, yPos, zPos);
                Vector3 randomRotation = new Vector3(xRotate, yRotate, zRotate);
                Vector3 randomLinear = new Vector3(xLinear, yLinear, zLinear);


                TempAsteroidField[i] = new Asteroid(game, AsteroidPos + randomPosition, AsteroidId + i, AsteroidMass, AsteroidLinearMomentum + randomLinear, AsteroidAngularMomentum + randomRotation);
            }
            return TempAsteroidField;
        }
    }

}