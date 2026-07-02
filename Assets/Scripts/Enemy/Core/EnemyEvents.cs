using System;

namespace Enemy.Core
{
    public static class EnemyEvents
    {
        public static event Action OnIntruderDetected;
        public static event Action<float> OnScaredVisibilityChanged;
        public static event Action<float> OnTriggerVisibilityChanged;

        public static void RaiseIntruderDetected()
        {
            if (OnIntruderDetected != null)
            {
                OnIntruderDetected.Invoke();
            }
        }

        public static void RaiseShyVisibilityChanged(float progress)
        {
            if (OnScaredVisibilityChanged != null)
            {
                OnScaredVisibilityChanged.Invoke(progress);
            }
        }

        public static void RaiseTriggerVisibilityChanged(float progress)
        {
            if (OnTriggerVisibilityChanged != null)
            {
                OnTriggerVisibilityChanged.Invoke(progress);
            }
        }
    }
}
