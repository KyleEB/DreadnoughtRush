﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace DreadnoughtRush
{
    internal class MotherShip : GameObject
    {
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

        public override void Draw(GameTime gameTime)
        {
            foreach (var mesh in model.Meshes)
            {
                DrawMeshToCamera(mesh);
            }
            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            model = Game.Content.Load<Model>("dreadnought");
            int[] indices;
            BEPUutilities.Vector3[] vertices;
            ModelDataExtractor.GetVerticesAndIndicesFromModel(model, out vertices, out indices);

            BEPUutilities.AffineTransform transform = new BEPUutilities.AffineTransform(Entity.WorldTransform.Translation);
            BEPUphysics.Entities.Prefabs.MobileMesh modelMesh = new BEPUphysics.Entities.Prefabs.MobileMesh(vertices, indices, transform, BEPUphysics.CollisionShapes.MobileMeshSolidity.DoubleSided, 100f);

            modelMesh.OrientationMatrix = physicsObject.OrientationMatrix;
            physicsObject = modelMesh;

            Game.Services.GetService<BEPUphysics.Space>().Add(physicsObject);
            base.LoadContent();
        }
    }
}
