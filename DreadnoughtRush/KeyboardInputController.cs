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
    }

    
}
