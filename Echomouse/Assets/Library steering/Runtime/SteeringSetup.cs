using UnityEngine;

namespace GLU.SteeringBehaviours
{
    using FMType = Refactored.FlockManagerFactory.NeighborLookupType;

    public class SteeringSetup : Generic.Singleton<SteeringSetup>
    {
        [Header("Steering Setup")]
        public SteeringStepSettings m_stepSettings = new SteeringStepSettings()
        {
            m_location     = SteeringStepSettings.UpdateMethod.Update,
            m_stepInterval = 0.1f
        };
        public FMType               m_flockManagerType = FMType.Simple; // the type of FlockManager we want to use
    }
}
