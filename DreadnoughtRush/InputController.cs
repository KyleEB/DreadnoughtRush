using Microsoft.Xna.Framework.Input;
namespace DreadnoughtRush
{
    abstract class InputController
    {
        public abstract bool shouldExit();

        public abstract bool changePerspective();

        public abstract bool lockOrientation();
        
    }

    class Bindings
    {
        //todo create pathway for bindings
    }
}