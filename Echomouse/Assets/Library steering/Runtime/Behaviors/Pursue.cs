using UnityEngine;

namespace GLU.SteeringBehaviours
{
    public class Pursue : Behavior 
    {
        readonly private GameObject m_target;     // the target object

        private Vector3 m_previousTargetPosition; // previous target position in m
        private Vector3 m_currentTargetPosition;  // previous target position in m

        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        public Pursue(GameObject target)
        {
            m_target                 = target;
            m_previousTargetPosition = target.transform.position;
            m_currentTargetPosition  = target.transform.position;
        }

        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        private Vector3 GetFuturePosition(float dt, BehaviorContext context)
        {
            // update current target position 
            m_previousTargetPosition = m_currentTargetPosition;
            m_currentTargetPosition  = m_target.transform.position;

            // calculate target velocity 
            Vector3 targetVelocity = (m_currentTargetPosition - m_previousTargetPosition) / dt;

            // return the target position in the near future
            return m_currentTargetPosition + targetVelocity * context.m_settings.m_lookAheadTime;
        }

        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        public override Vector3 CalculateSteeringForce(float dt, BehaviorContext context)
        {
            // update target position plus desired velocity, and return steering force          
            m_positionTarget  = GetFuturePosition(dt, context);
            if (ArriveEnabled(context) && WithinArriveSlowingDistance(context, m_positionTarget))
                m_velocityDesired = CalculateArriveSteeringForce(context, m_positionTarget);
            else
                m_velocityDesired = (m_positionTarget - context.m_position).normalized * context.m_settings.m_maxDesiredVelocity;
            return m_velocityDesired - context.m_velocity;
        }

        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        override public void OnDrawGizmos(BehaviorContext context)
        {
            base.OnDrawGizmos(context);
            if (ArriveEnabled(context))
                OnDrawArriveGizmos(context);
        }
    }
}
