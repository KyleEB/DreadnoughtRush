using BEPUphysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreadnoughtRush
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>

    public class DreadnoughtRush : Game
    {
        GraphicsDeviceManager graphics;

        Ship PlayerShip;
        MotherShip Mothership;

        InputController Controller = new KeyboardInputController();

        public DreadnoughtRush()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1920;  //Setting Width/Height to standard 1920p by 1080p view 
            graphics.PreferredBackBufferHeight = 1080;   
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Make our BEPU Physics space a service
            Services.AddService<Space>(new Space());

            float AsteroidFieldDensityFactor = 2f;
            
            PlayerShip = InitializePlayer();
            Mothership = InitializeMothership(); 
            Asteroid.CreateAsteroidField(this, 500, AsteroidFieldDensityFactor);

            new Skybox(this);

            SetupCamera();

            base.Initialize();
        }


        private Ship InitializePlayer()
        {
            Vector3 PlayerPos = new Vector3(0f, -1f, -5f);
            string PlayerId = "Player";
            float PlayerMass = 3f;
            Vector3 PlayerLinearMomentum = new Vector3(0f, 0f, 2f);
            Vector3 PlayerAngularMomentum = new Vector3(0f, 0f, 0f);

            return new Ship(this, PlayerPos, PlayerId, PlayerMass, PlayerLinearMomentum, PlayerAngularMomentum);
        }

        private MotherShip InitializeMothership()
        {
            Vector3 MothershipPos = new Vector3(10f, 10f, -5f);
            string MothershipId = "Mothership";
            float MothershipMass = 3f;
            Vector3 MothershipLinearMomentum = new Vector3(2f, 0f, 0f);
            Vector3 MothershipAngularMomentum = new Vector3(0f, 0f, 0f);

            return new MotherShip(this, MothershipPos, MothershipId, MothershipMass, MothershipLinearMomentum, MothershipAngularMomentum);
        }

        private void SetupCamera()
        {
            float aspectRatio = GraphicsDevice.Viewport.AspectRatio;
            float fieldOfView = MathHelper.PiOver2; //90 degree fov
            float nearClipPlane = 1;
            float farClipPlane = 4000;

            Matrix Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);

            Services.AddService<Camera>(new Camera(ConversionHelper.MathConverter.Convert(Vector3.Zero), 0f, 0f, ConversionHelper.MathConverter.Convert(Projection)));
        }

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
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
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
