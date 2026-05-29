namespace Enemy.ShyEnemy
{
    public class FleeState :  IEnemyState
    {
        private ShyEnemyBehaviour _enemy;

        public FleeState(ShyEnemyBehaviour enemy)
        {
            _enemy = enemy;
        }
        public void EnterState()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateState()
        {
            throw new System.NotImplementedException();
        }

        public void ExitState()
        {
            throw new System.NotImplementedException();
        }
    }
}
