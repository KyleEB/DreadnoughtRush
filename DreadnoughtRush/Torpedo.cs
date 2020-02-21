using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreadnoughtRush
{
    internal class Torpepdo : Ship
    {
        GameObject Trigger;
        GameObject Source;


        public Torpepdo(Game game) : base(game)
        {
        }

        public Torpepdo(Game game, Vector3 pos, string id) : base(game, pos, id)
        {
        }

        public Torpepdo(Game game, Vector3 pos, string id, float mass) : base(game, pos, id, mass)
        {
        }

        public Torpepdo(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum) : base(game, pos, id, mass, linMomentum)
        {
        }

        public Torpepdo(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum, Vector3 angMomentum) : base(game, pos, id, mass, linMomentum, angMomentum)
        {
        }

        public Torpepdo(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum, Vector3 angMomentum, GameObject trigger) : this(game, pos, id, mass, linMomentum, angMomentum)
        {
            this.Trigger = trigger;
        }

        protected override void Events_InitialCollisionDetected(BEPUphysics.BroadPhaseEntries.MobileCollidables.EntityCollidable sender, BEPUphysics.BroadPhaseEntries.Collidable other, BEPUphysics.NarrowPhaseSystems.Pairs.CollidablePairHandler pair)
        {
            if (other == Trigger.entity.CollisionInformation)
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
