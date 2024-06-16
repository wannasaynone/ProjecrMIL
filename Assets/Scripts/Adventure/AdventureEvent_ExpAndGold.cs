using UnityEngine;

namespace ProjectMIL.Adventure
{
    public class AdventureEvent_ExpAndGold : AdventureEventBase
    {
        private readonly Utlity.ContextHandler contextHandler;
        private static int totalCount = 0; // TODO: handle save and load
        private static int jackpotCount = 0; // TODO: handle save and load
        private static float jackpotRate = 0f; // TODO: handle save and load

        public AdventureEvent_ExpAndGold(Utlity.ContextHandler contextHandler)
        {
            this.contextHandler = contextHandler;
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
        public override void Execute()
        {
            if (Random.value < 0.5f)
            {
                CreateExpEvent();
            }
            else
            {
                CreateGoldEvent();
            }
        }

        private void CreateExpEvent()
        {
            double mu = 3.79;  // mu 值
            double sigma = 1.32;  // sigma 值
            LogNormalDistribution distribution = new LogNormalDistribution(mu, sigma);
            int addExp = System.Convert.ToInt32(distribution.GenerateRandom());

            totalCount++;
            if (addExp >= 1000)
            {
                jackpotCount++;
                jackpotRate = 0f;
            }
            else if (totalCount >= 2 + jackpotCount * 20 && jackpotCount < 3 && addExp <= 100)
            {
                jackpotRate += UnityEngine.Random.value;
                if (jackpotRate >= jackpotCount + 1)
                {
                    addExp += 1000;
                    jackpotRate = 0f;
                    jackpotCount++;
                }
            }

            GameEvent.EventBus.Publish(new GameEvent.OnAdventureEventCreated_Exp()
            {
                addExp = addExp,
                title = contextHandler.GetContext(10000),
                description = contextHandler.GetContext(10001)
            });
        }

        private void CreateGoldEvent()
        {
            double mu = 3.79;  // mu 值
            double sigma = 1.32;  // sigma 值
            LogNormalDistribution distribution = new LogNormalDistribution(mu, sigma);
            int addGold = System.Convert.ToInt32(distribution.GenerateRandom());

            totalCount++;
            if (addGold >= 1000)
            {
                jackpotCount++;
                jackpotRate = 0f;
            }
            else if (totalCount >= 2 + jackpotCount * 20 && jackpotCount < 3 && addGold <= 100)
            {
                jackpotRate += UnityEngine.Random.value;
                if (jackpotRate >= jackpotCount + 1)
                {
                    addGold += 1000;
                    jackpotRate = 0f;
                    jackpotCount++;
                }
            }

            GameEvent.EventBus.Publish(new GameEvent.OnAdventureEventCreated_Gold()
            {
                addGold = addGold,
                title = contextHandler.GetContext(10002),
                description = contextHandler.GetContext(10003)
            });
        }
    }
}