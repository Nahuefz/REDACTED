using System;

namespace Enemy.Core
{
    public static class EnemyEvents
    {
        public static event Action OnIntruderDetected;
        public static event Action<float> OnShyVisibilityChanged;

        public static void RaiseIntruderDetected() => OnIntruderDetected?.Invoke();

        public static void RaiseShyVisibilityChanged(float progress) =>
            OnShyVisibilityChanged?.Invoke(progress);
    }
}
