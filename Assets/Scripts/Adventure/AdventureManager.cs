namespace ProjectMIL.Adventure
{
    public class AdventureManager
    {
        public AdventureManager()
        {
            GameEvent.EventBus.Subscribe<GameEvent.OnAdventureButtonPressed>(OnAdventureButtonPressed);
        }

        private class LogNormalDistribution
        {
            private System.Random _random;
            private double _mu;
            private double _sigma;

            public LogNormalDistribution(double mu, double sigma)
            {
                _mu = mu;
                _sigma = sigma;
                _random = new System.Random();  // 隨機數生成器
            }

            // 生成對數正態分布隨機數
            public double GenerateRandom()
            {
                // 生成兩個 (0,1) 範圍內的獨立隨機數
                double u1 = _random.NextDouble();
                double u2 = _random.NextDouble();

                // 生成標準正態分布隨機數 (Box-Muller transform)
                double stdNormal = System.Math.Sqrt(-2.0 * System.Math.Log(u1)) * System.Math.Sin(2.0 * System.Math.PI * u2);

                // 轉換為對數正態分布
                return System.Math.Exp(_mu + _sigma * stdNormal);
            }
        }

        private void OnAdventureButtonPressed(GameEvent.OnAdventureButtonPressed eventToPublish)
        {
            double mu = 3.79;  // mu 值
            double sigma = 1.32;  // sigma 值
            LogNormalDistribution distribution = new LogNormalDistribution(mu, sigma);
            GameEvent.EventBus.Publish(new GameEvent.OnAdventureEventCreated() { addExp = System.Convert.ToInt32(distribution.GenerateRandom()) });
        }
    }
}