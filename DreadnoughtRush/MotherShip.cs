using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace DreadnoughtRush
{
    internal class MotherShip : GameObject
    {
        public bool hasPlayerReached = false;
        public MotherShip(Game game) : base(game)
        {
        }

        public MotherShip(Game game, Vector3 pos, string id) : base(game, pos, id)
        {
        }

        public MotherShip(Game game, Vector3 pos, string id, float mass) : base(game, pos, id, mass)
        {
        }

        public MotherShip(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum) : base(game, pos, id, mass, linMomentum)
        {
        }

        public MotherShip(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum, Vector3 angMomentum) : base(game, pos, id, mass, linMomentum, angMomentum)
        {
        }

        protected override void Events_InitialCollisionDetected(BEPUphysics.BroadPhaseEntries.MobileCollidables.EntityCollidable sender, BEPUphysics.BroadPhaseEntries.Collidable other, BEPUphysics.NarrowPhaseSystems.Pairs.CollidablePairHandler pair)
        {
            string tempTag = string.Empty;

            if (other != null && other.Tag != null)
            {
                tempTag = other.Tag.ToString();
            }

            if (tempTag.Equals("Player"))
            {
                hasPlayerReached = true;
                new VictoryScreen(this.Game);
            }
        }

        protected override void LoadContent()
        {
            model = Game.Content.Load<Model>("dreadnought");
            ((BEPUphysics.Entities.Prefabs.Sphere)entity).Radius = model.Meshes[0].BoundingSphere.Radius;
            base.LoadContent();
        }
    }
}
