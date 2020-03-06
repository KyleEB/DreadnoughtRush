﻿namespace DreadnoughtRush
{
    abstract class InputController
    {
        public abstract bool shouldExit();
        public abstract bool changePerspective();
        public abstract bool lockOrientation();
        public abstract bool forwardThrust();
        public abstract bool backwardThrust();
        public abstract bool PositiveYawThrust();
        public abstract bool NegativeYawThrust();
        public abstract bool PostivePitchThrust();
        public abstract bool NegativePitchThrust();
        public abstract bool PositiveRollThrust();
        public abstract bool NegativeRollThrust();
        public abstract bool FireTorpedo();

    }



    class Bindings
    {
        //todo create pathway for bindings
    }
}