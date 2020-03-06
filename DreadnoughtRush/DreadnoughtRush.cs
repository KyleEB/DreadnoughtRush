using BEPUphysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreadnoughtRush
{
    /// <summary>
    /// General setup for the game is all done here, as it specifies the initial playing field for the player.
    /// </summary>

    public class DreadnoughtRush : Game
    {
        GraphicsDeviceManager graphics;

        Ship PlayerShip;
        MotherShip Mothership;
        Skybox skybox;

        InputController Controller = new KeyboardInputController();

        /// <summary>
        /// Setup the default viewport for the game.
        /// </summary>
        public DreadnoughtRush()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1920;  //Setting Width/Height to standard 1920p by 1080p view 
            graphics.PreferredBackBufferHeight = 1080;   
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Setup the Player, Mothership, and asteroid fields for the game.
        /// Also Sets default properties for the Game's Camera
        /// </summary>
        protected override void Initialize()
        {
            Services.AddService<Space>(new Space());

            float AsteroidFieldDensityFactor = 1f; //how packed the asteroid field should be, higher means closer asteroids, lower means more spread out
            
            PlayerShip = InitializePlayer();
            Mothership = InitializeMothership();
            Asteroid.CreateAsteroidField(this, 1000, AsteroidFieldDensityFactor);
            skybox = new Skybox(this);

            SetupCamera();

            base.Initialize();
        }

        /// <summary>
        /// Setup default player by creating a new player ship.
        /// </summary>
        /// <returns></returns>
        private Ship InitializePlayer()
        {
            Vector3 PlayerPos = new Vector3(0f, 0f, 0f); //spawn at origin
            string PlayerId = "Player";//tag for collisions
            float PlayerMass = 3f;
            Vector3 PlayerLinearMomentum = Vector3.Zero; 
            Vector3 PlayerAngularMomentum = Vector3.Zero; //no inital forces

            return new Ship(this, PlayerPos, PlayerId, PlayerMass, PlayerLinearMomentum, PlayerAngularMomentum);
        }

        /// <summary>
        /// Setup default mothership by creating a new mothership.
        /// </summary>
        /// <returns></returns>
        private MotherShip InitializeMothership()
        {
            Vector3 MothershipPos = new Vector3(100f, 0f, 50f); //set it generally in front of the player
            string MothershipId = "Mothership";//tag for collisions
            float MothershipMass = 100f;
            Vector3 MothershipLinearMomentum = new Vector3(2f, 0f, 0f); //give it a little push to start off
            Vector3 MothershipAngularMomentum = new Vector3(0f, 0f, 0f);

            return new MotherShip(this, MothershipPos, MothershipId, MothershipMass, MothershipLinearMomentum, MothershipAngularMomentum);
        }

        /// <summary>
        /// Setup default camera view that will be used to follow the player ship.
        /// </summary>
        private void SetupCamera()
        {
            float aspectRatio = GraphicsDevice.Viewport.AspectRatio;
            float fieldOfView = MathHelper.PiOver2; //90 degree fov
            float nearClipPlane = 1;
            float farClipPlane = 4000;

            Matrix Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);

            Services.AddService<Camera>(new Camera(ConversionHelper.MathConverter.Convert(Vector3.Zero), 0f, 0f, ConversionHelper.MathConverter.Convert(Projection)));
        }

        /// <summary>
        /// Make the camera be either in a first person or third person view based on the given GameObject
        /// </summary>
        /// <param name="chase">The GameObject to follow</param>
        private void UpdateChaseObject(GameObject chase)
        {
            Camera camera = Services.GetService<Camera>();

            Vector3 localCamera;
            Vector3 localLookAt;

            if (camera.isFirstPerson) {
                localCamera = new Vector3(0, 2, 0);
                localLookAt = new Vector3(0, 3, 10);
            } else
            {
                localCamera = new Vector3(0, 3, -10);
                localLookAt = new Vector3(0, 5, 10);
            }


            Vector3 worldCamera = Vector3.Transform(localCamera, chase.TranformationMatrix);
            Vector3 worldLookAt = Vector3.Transform(localLookAt, chase.TranformationMatrix);

            camera.Position = ConversionHelper.MathConverter.Convert(worldCamera);

            camera.ViewDirection = ConversionHelper.MathConverter.Convert(worldLookAt) - camera.Position;

            if (camera.isCameraRotationLocked)
            {
                camera.LockedUp = ConversionHelper.MathConverter.Convert(Vector3.Transform(Vector3.Up, chase.RotationMatrix));
            }
            else
            {
                camera.LockedUp = ConversionHelper.MathConverter.Convert(Vector3.Up);
            }

              
        }


        /// <summary>
        /// Update BEPU Physics space
        /// Update inputs from the player
        /// Update the GameObject the camera is currently chasing
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Camera camera = Services.GetService<Camera>();
            PlayerControllerEvents(gameTime, camera);
            
            Services.GetService<Space>().Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            UpdateChaseObject(PlayerShip);

            base.Update(gameTime);
        }

        

        /// <summary>
        /// Handle player inputs from the Controller
        /// </summary>
        /// <param name="gameTime">A snapshot of the current time</param>
        /// <param name="camera">The current camera that represents the player's view</param>
        private void PlayerControllerEvents(GameTime gameTime, Camera camera)
        {
            if (Controller.shouldExit())
                Exit();

            if (Controller.changePerspective())
            {
                camera.togglePerspective((float)gameTime.ElapsedGameTime.TotalSeconds);

            }

            if (Controller.lockOrientation())
            {
                camera.toggleOrientationLock((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (Controller.forwardThrust())
            {
                PlayerShip.Movement.ApplyForwardThrust((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (Controller.backwardThrust())
            {
                PlayerShip.Movement.ApplyBackwardThrust((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (Controller.PositiveYawThrust())
            {
                PlayerShip.Movement.ApplyPositiveYawThrust((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (Controller.NegativeYawThrust())
            {
                PlayerShip.Movement.ApplyNegativeYawThrust((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (Controller.PostivePitchThrust())
            {
                PlayerShip.Movement.ApplyPositivePitchThrust((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (Controller.NegativePitchThrust())
            {
                PlayerShip.Movement.ApplyNegativePitchThrust((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (Controller.PositiveRollThrust())
            {
                PlayerShip.Movement.ApplyPositiveRollThrust((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (Controller.NegativeRollThrust())
            {
                PlayerShip.Movement.ApplyNegativeRollThrust((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (Controller.FireTorpedo())
            {
                
                PlayerShip.FireATorpedo((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }

    }
}
