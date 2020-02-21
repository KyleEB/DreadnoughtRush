﻿using BEPUphysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

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
        Asteroid [] AsteroidField;

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
            // TODO: Add your initialization logic here

            // Make our BEPU Physics space a service
            Services.AddService<Space>(new Space());


            PlayerShip = InitializePlayer();
            AsteroidField = InitializeAsteroidField(PlayerShip);

            base.Initialize();
        }

        private Asteroid[] InitializeAsteroidField(GameObject trigger)
        {
            Vector3 AsteroidPos = new Vector3(-250, -250, -250);
            string AsteroidId = "Asteroid";
            float AsteroidMass = 3f;
            Vector3 AsteroidLinearMomentum = new Vector3(0f, 0f, 0f);
            Vector3 AsteroidAngularMomentum = new Vector3(0f, 0f, 0f);

            Random random = new Random();

            int asteroidCount = 1000;
            Asteroid[] TempAsteroidField = new Asteroid[asteroidCount];
            for (int i = 0; i < asteroidCount; i++)
            {
                float xPos = asteroidCount * (float)random.NextDouble();
                float yPos = asteroidCount * (float)random.NextDouble();
                float zPos = asteroidCount * (float)random.NextDouble();

                float xRotate = 5 * (float)random.NextDouble() - 5;
                float yRotate = 5 * (float)random.NextDouble() - 5;
                float zRotate = 5 * (float)random.NextDouble() - 5;

                float xLinear = 10 * (float)random.NextDouble() - 10;
                float yLinear = 10 * (float)random.NextDouble() - 10;
                float zLinear = 10 * (float)random.NextDouble() - 10;

                Vector3 randomPosition = new Vector3(xPos, yPos, zPos);
                Vector3 randomRotation = new Vector3(xRotate, yRotate, zRotate);
                Vector3 randomLinear = new Vector3(xLinear, yLinear, zLinear);

                 
                TempAsteroidField[i] = new Asteroid(this, AsteroidPos + randomPosition, AsteroidId + i, AsteroidMass, AsteroidLinearMomentum + randomLinear, AsteroidAngularMomentum + randomRotation, trigger);
            }
            return TempAsteroidField;
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
            float fieldOfView = MathHelper.PiOver2; //90 degree fov
            float nearClipPlane = 1;
            float farClipPlane = 2000;

            Matrix Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);

            Services.AddService<Camera>(new Camera(ConversionHelper.MathConverter.Convert(Vector3.Zero), 0f, 0f, ConversionHelper.MathConverter.Convert(Projection)));

            UpdateChaseObject(PlayerShip);
        }

        private void UpdateChaseObject(Ship chase)
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
                PlayerShip.FireATorpedo();
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
