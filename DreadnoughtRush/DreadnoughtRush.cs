using BEPUphysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace DreadnoughtRush
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>

    public class DreadnoughtRush : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Ship PlayerShip;
        Asteroid asteroid;

        InputController Controller = new KeyboardInputController();

        private bool FirstPerson = false;

        private double FirstPersonToggleTimeout = 1;

        private bool CameraRotationLocked = false;

        public DreadnoughtRush()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1920;  //Setting Width/Height to standard 1920 by 1080p view 
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
            // TODO: Add your initialization logic here

            // Make our BEPU Physics space a service
            Services.AddService<Space>(new Space());


            // Create two asteroids.  Note that asteroids automatically add themselves to
            // as a DrawableGameComponent as well as add an object into Bepu physics
            // that represents the asteroid.


            Vector3 AsteroidPos = new Vector3(0, 0, 10);
            string AsteroidId = "Asteroid";
            float AsteroidMass = 3f;
            Vector3 AsteroidLinearMomentum = new Vector3(0f, 0f, 0f);
            Vector3 AsteroidAngularMomentum = new Vector3(0f, 0f, 0f);


            asteroid = new Asteroid(this, AsteroidPos, AsteroidId, AsteroidMass, AsteroidLinearMomentum, AsteroidAngularMomentum);


            PlayerShip = InitializePlayer();


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

        

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            SetupCamera();
        }

        private void SetupCamera()
        {
            float aspectRatio = GraphicsDevice.Viewport.AspectRatio;
            float fieldOfView = MathHelper.PiOver4;
            float nearClipPlane = 1;
            float farClipPlane = 200;

            Matrix Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);

            Services.AddService<Camera>(new Camera(ConversionHelper.MathConverter.Convert(Vector3.Zero), 0f, 0f, ConversionHelper.MathConverter.Convert(Projection)));

            UpdateChaseObject(PlayerShip);
        }

        private void UpdateChaseObject(Ship chase)
        {
            Camera camera = Services.GetService<Camera>();

            Vector3 localCamera;
            Vector3 localLookAt;

            if (FirstPerson) {
                localCamera = new Vector3(0, 2, 0);
                localLookAt = new Vector3(0, 1, 10);
            } else
            {
                localCamera = new Vector3(0, 2, -20);
                localLookAt = new Vector3(0, 1, 10);
            }


            Vector3 worldCamera = Vector3.Transform(localCamera, chase.TranformationMatrix);
            Vector3 worldLookAt = Vector3.Transform(localLookAt, chase.TranformationMatrix);

            camera.Position = ConversionHelper.MathConverter.Convert(worldCamera);

            camera.ViewDirection = ConversionHelper.MathConverter.Convert(worldLookAt) - camera.Position;

            if (CameraRotationLocked)
            {
                camera.LockedUp = ConversionHelper.MathConverter.Convert(Vector3.Transform(Vector3.Up, chase.RotationMatrix));
            }
            else
            {
                camera.LockedUp = ConversionHelper.MathConverter.Convert(Vector3.Up);
            }

              
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            if (Controller.shouldExit())
                Exit();

            if (Controller.changePerspective())
            {
                togglePerspective(gameTime);

            }

            if (Controller.lockOrientation())
            {
                toggleOrientationLock(gameTime);
            }

            if (Controller.forwardThrust())
            {
                PlayerShip.ApplyForwardThrust((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (Controller.backwardThrust())
            {
                PlayerShip.ApplyBackwardThrust((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (Controller.PositiveYawThrust())
            {
                PlayerShip.ApplyPositiveYawThrust((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (Controller.NegativeYawThrust())
            {
                PlayerShip.ApplyNegativeYawThrust((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if(Controller.PostivePitchThrust())
            {
                PlayerShip.ApplyPositivePitchThrust((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (Controller.NegativePitchThrust())
            {
                PlayerShip.ApplyNegativePitchThrust((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (Controller.PositiveRollThrust())
            {
                PlayerShip.ApplyPositiveRollThrust((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (Controller.NegativeRollThrust())
            {
                PlayerShip.ApplyNegativeRollThrust((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (Controller.AngularDampeners())
            {
                PlayerShip.ApplyAngularDampeners();
            } else
            {
                PlayerShip.ReleaseAngularDampeners();
            }




            Services.GetService<Space>().Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            UpdateChaseObject(PlayerShip);

            base.Update(gameTime);
        }

        private void toggleOrientationLock(GameTime gameTime)
        {
            if (FirstPersonToggleTimeout < 0)
            {
                CameraRotationLocked = !CameraRotationLocked;
                FirstPersonToggleTimeout = 1;
            }
            else
            {
                FirstPersonToggleTimeout -= gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        private void togglePerspective(GameTime gameTime)
        {
            if (FirstPersonToggleTimeout < 0)
            {
                FirstPerson = !FirstPerson;
                FirstPersonToggleTimeout = 1;
            }
            else
            {
                FirstPersonToggleTimeout -= gameTime.ElapsedGameTime.TotalSeconds;
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
