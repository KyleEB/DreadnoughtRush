using Microsoft.Xna.Framework;
using System;

namespace DreadnoughtRush
{
    class ThrustMovement
    {

        ThrustScalars Scalars;
        GameObject Target;

        public ThrustMovement(GameObject target, ThrustScalars scalars)
        {
            Scalars = scalars;
            Target = target;
        }

        /// <summary>
        /// Apply forward thrust at the center of our ship, this means we are pushing it forward.
        /// </summary>
        /// <param name="dt">Amount of time passed in seconds, scales the power of the thrust.</param>
        public void ApplyForwardThrust(float dt)
        {
            Vector3 direction = new Vector3(0, 0, Scalars.ForwardThrustScalar);
            ApplyThrust(direction, Vector3.Zero, dt);
        }

        /// <summary>
        /// Apply backward thrust at the center of our ship, this means we are pushing it backward.
        /// </summary>
        /// <param name="dt">Amount of time passed in seconds, scales the power of the thrust.</param>
        public void ApplyBackwardThrust(float dt)
        {
            Vector3 direction = new Vector3(0, 0, -Scalars.BackwardThrustScalar);
            ApplyThrust(direction, Vector3.Zero, dt);
        }

        public void ApplyPositiveYawThrust(float dt)
        {
            Vector3 direction = new Vector3(-1, 0, 0);
            Vector3 localPosition = new Vector3(0, 0, Scalars.YawScalar);
            ApplyThrust(direction, localPosition, dt);
        }

        public void ApplyNegativeYawThrust(float dt)
        {
            Vector3 direction = new Vector3(1, 0, 0);
            Vector3 localPostion = new Vector3(0, 0, Scalars.YawScalar);
            ApplyThrust(direction, localPostion, dt);
        }

        public void ApplyPositivePitchThrust(float dt)
        {
            Vector3 direction = new Vector3(0, 1, 0);
            Vector3 localPosition = new Vector3(0, 0, Scalars.PitchScalar);
            ApplyThrust(direction, localPosition, dt);
        }

        public void ApplyNegativePitchThrust(float dt)
        {
            Vector3 direction = new Vector3(0, -1, 0);
            Vector3 localPostion = new Vector3(0, 0, Scalars.PitchScalar);
            ApplyThrust(direction, localPostion, dt);
        }

        public void ApplyPositiveRollThrust(float dt)
        {
            Vector3 direction = new Vector3(0, -1, 0);
            Vector3 localPosition = new Vector3(-Scalars.RollScalar, 0, 0);
            ApplyThrust(direction, localPosition, dt);
            ApplyThrust(-direction, -localPosition, dt);
        }

        public void ApplyNegativeRollThrust(float dt)
        {
            Vector3 direction = new Vector3(0, 1, 0);
            Vector3 localPosition = new Vector3(-Scalars.RollScalar, 0, 0);
            ApplyThrust(direction, localPosition, dt);
            ApplyThrust(-direction, -localPosition, dt);
        }

        /// <summary>
        /// Apply a general thrust to the ship in reference to a local position and direction and time.
        /// </summary>
        /// <param name="Direction">Vector in which we want to push towards</param>
        /// <param name="LocalPosition">The local position to apply the thrust, so we can thrust at offsets away from the center</param>
        /// <param name="dt">Amount of time passed in seconds</param>
        private void ApplyThrust(Vector3 Direction, Vector3 LocalPosition, float dt)
        {
            Vector3 worldPosition = Vector3.Transform(LocalPosition, Target.TranformationMatrix);
            Vector3 worldDirection = Vector3.Transform(Direction, Target.RotationMatrix);

            new FireParticle(this.Target.Game, worldPosition - worldDirection, "fireParticle", 1f, Direction );
            Target.Entity.ApplyImpulse(ConversionHelper.MathConverter.Convert(worldPosition), ConversionHelper.MathConverter.Convert(worldDirection));
        }
    }

    class ThrustScalars
    {

        public float YawScalar { get; private set; }
        public float PitchScalar { get; private set; }
        public float RollScalar { get; private set; }
        public float ForwardThrustScalar { get; private set; }
        public float BackwardThrustScalar { get; private set; }

        public ThrustScalars(float yawScalar, float pitchScalar, float rollScalar, float forwardThrustScalar,float backwardThrustScalar)
        {
            YawScalar = yawScalar;
            PitchScalar = pitchScalar;
            RollScalar = rollScalar;
            ForwardThrustScalar = forwardThrustScalar;
            BackwardThrustScalar = backwardThrustScalar;
        }
    }
}
