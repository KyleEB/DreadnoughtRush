using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DreadnoughtRush
{
    internal class BrokenAsteroidParticle : Particle
    {

        public BrokenAsteroidParticle(Game game) : base(game)
        {
        }

        public BrokenAsteroidParticle(Game game, Vector3 pos, string id) : base(game, pos, id)
        {
        }

        public BrokenAsteroidParticle(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum) : base(game, pos, id, mass, linMomentum)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            model = Game.Content.Load<Model>("Particle");
            texture = Game.Content.Load<Texture2D>("brokenAsteroidTexture");
            LifeTime = 5;

            base.LoadContent();
        }

        public static void CreateParticleBoom(Game game, Vector3 position, float intensity, int numParticles)
        {
            Random rand = new Random();

            for (int i = 0; i < numParticles; i++)
            {
                float xDisplace = 2 * (float)rand.NextDouble() - 1 ;
                float yDisplace = 2 * (float)rand.NextDouble() - 1 ;
                float zDisplace = 2 * (float)rand.NextDouble() - 1 ;

                Vector3 randomDisplace = new Vector3(xDisplace,yDisplace,zDisplace) * intensity;
                new BrokenAsteroidParticle(game, position, "BrokenAsteroid", 1f, randomDisplace);
            }
        }
    }
}
