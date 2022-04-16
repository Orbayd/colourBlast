using System;
using System.Collections.Generic;
using System.Linq;

namespace ColourBlast.Helpers
{
    public static class MessageBus
    {
        //Convert to Dictionary
        public static List<object> _subscriptions = new List<object>();

        public static List<object> _danglingPublishers = new List<object>();

        public static void Subscribe<T>(Action<T> handler)
        {
            _subscriptions.Add(handler);

            var danglingsofType = _danglingPublishers.OfType<T>().ToList();
            if (danglingsofType != null || danglingsofType.Any())
            {
                foreach (var publisher in danglingsofType)
                {
                    Publish(publisher);
                }
                //_danglingPublishers.Remove(danglingsofType.Cast<object>());

                for (int i = 0; i < danglingsofType.Count; i++)
                {
                    var publisherObject = danglingsofType.ElementAtOrDefault(0);
                    var dangling = _danglingPublishers.FirstOrDefault((x) => x is T p && p.Equals(publisherObject));
                    _danglingPublishers.Remove(dangling);

                }
            }
        }

        public static void Publish<T>(T publisher)
        {
            var subscriptionsOfType = _subscriptions.OfType<Action<T>>().ToList();

            if (subscriptionsOfType == null || !subscriptionsOfType.Any())
            {
                if (!_danglingPublishers.Contains(publisher))
                {
                    _danglingPublishers.Add(publisher);
                }
            }

            foreach (var subscription in subscriptionsOfType)
            {
                subscription.Invoke(publisher);
            }
        }

        public static void UnSubscribe<T>(Action<T> handler)
        {
            _subscriptions.Remove(handler);
        }
        public static void UnSubscribe<T>()
        {

            var subscriptionsOfType = _subscriptions.OfType<Action<T>>().ToList();
            foreach (var subscription in subscriptionsOfType)
            {
                _subscriptions.Remove(subscription);
            }
        }

    }
}
