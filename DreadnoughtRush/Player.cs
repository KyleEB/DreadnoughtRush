using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DreadnoughtRush
{
    class Player : Ship
    {
        Vector3 PlayerPos = new Vector3(2, 0, -5);
        string PlayerId = "Player";
        float PlayerMass = 3f;
        Vector3 PlayerLinearMomentum = new Vector3(-0.2f, 0, 0);
        Vector3 PlayerAngularMomentum = new Vector3(-0.5f, -0.6f, 0.2f);

        public Player(Game game) : base(game)
        {
            Player(game, PlayerPos, PlayerId, PlayerMass, PlayerLinearMomentum, PlayerAngularMomentum);
        }

        public Player(Game game, Vector3 pos, string id) : base(game, pos, id)
        {
        }

        public Player(Game game, Vector3 pos, string id, float mass) : base(game, pos, id, mass)
        {
        }

        public Player(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum) : base(game, pos, id, mass, linMomentum)
        {
        }

        public Player(Game game, Vector3 pos, string id, float mass, Vector3 linMomentum, Vector3 angMomentum) : base(game, pos, id, mass, linMomentum, angMomentum)
        {
        }

    }
}
