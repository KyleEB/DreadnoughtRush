using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreadnoughtRush
{
    internal class Torpedo : GameObject
    {

        protected ThrustScalars thrusterScalars;
        public ThrustMovement Movement;

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
        }


        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            model = Game.Content.Load<Model>("torpedo");
            //physicsObject.Radius = model.Meshes[0].BoundingSphere.Radius;

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
