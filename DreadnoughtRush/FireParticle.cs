﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreadnoughtRush
{
    internal class FireParticle : Particle
    {   
       
        public FireParticle(Game game) : base(game)
        {
        }

        public FireParticle(Game game, Vector3 pos, string id) : base(game, pos, id)
        {
        }

        public FireParticle(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum) : base(game, pos, id, mass, linMomentum)
        {
        }

        public FireParticle(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum, double lifetime) : base(game, pos, id, mass, linMomentum)
        {
            LifeTime = lifetime;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            model = Game.Content.Load<Model>("Particle");
            
            texture = Game.Content.Load<Texture2D>("fireTexture");

            base.LoadContent();
        }
    }
}
