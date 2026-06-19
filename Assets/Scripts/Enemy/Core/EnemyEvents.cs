using System;

namespace Enemy.Core
{
    public static class EnemyEvents
    {
        public static event Action OnIntruderDetected;
        public static event Action<float> OnShyVisibilityChanged;

        public static void RaiseIntruderDetected()
        {
            if (OnIntruderDetected != null)
            {
                OnIntruderDetected.Invoke();
            }
        }

        public static void RaiseShyVisibilityChanged(float progress)
        {
            if (OnShyVisibilityChanged != null)
            {
                OnShyVisibilityChanged.Invoke(progress);
            }
        }
    }
}
