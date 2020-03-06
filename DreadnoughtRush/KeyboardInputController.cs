using Microsoft.Xna.Framework.Input;

namespace DreadnoughtRush
{
    class KeyboardInputController : InputController
    {
        private bool isKeyDown(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key);
        }

        public override bool shouldExit()
        {
            return isKeyDown(Keys.Escape);
        }

        public override bool changePerspective()
        {
            return isKeyDown(Keys.P);
        }

        public override bool lockOrientation()
        {
            return isKeyDown(Keys.O);
        }

        public override bool forwardThrust()
        {
            return isKeyDown(Keys.W);
        }

        public override bool backwardThrust()
        {
            return isKeyDown(Keys.S);
        }

        public override bool PositiveYawThrust()
        {
            return isKeyDown(Keys.L);
        }

        public override bool NegativeYawThrust()
        {
            return isKeyDown(Keys.J);
        }

        public override bool PostivePitchThrust()
        {
            return isKeyDown(Keys.I);
        }

        public override bool NegativePitchThrust()
        {
            return isKeyDown(Keys.K);
        }

        public override bool PositiveRollThrust()
        {
            return isKeyDown(Keys.D);
        }

        public override bool NegativeRollThrust()
        {
            return isKeyDown(Keys.A);
        }

        public override bool FireTorpedo()
        {
            return isKeyDown(Keys.Space);
        }

    }
}
