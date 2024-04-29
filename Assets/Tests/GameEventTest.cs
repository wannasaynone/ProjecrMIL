using NUnit.Framework;
using NUnit.Framework.Internal;
using ProjectMIL.GameEvent;

public class GameEventTest
{
    public class TestEvent : GameEventBase
    {
        public int testValue = 0;
    }

    [Test]
    public void publish()
    {
        TestEvent testEvent = new TestEvent();

        EventBus.Subscribe<TestEvent>(handler);
        EventBus.Publish(testEvent);
        Assert.AreEqual(1, testEvent.testValue);
        EventBus.ForceClearAll();
    }

    [Test]
    public void unsubscribe()
    {
        TestEvent testEvent = new TestEvent();

        EventBus.Subscribe<TestEvent>(handler);
        EventBus.Publish(testEvent);
        Assert.AreEqual(1, testEvent.testValue);
        EventBus.Unsubscribe<TestEvent>(handler);
        EventBus.Publish(testEvent);
        Assert.AreEqual(1, testEvent.testValue);
        EventBus.ForceClearAll();
    }

    private void handler(TestEvent e) { e.testValue++; }
}
