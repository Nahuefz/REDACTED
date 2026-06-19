namespace Enemy.Core
{
    public abstract class EnemyStateBase : IEnemyState
    {
        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void Exit() { }
    }
}
