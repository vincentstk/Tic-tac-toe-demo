using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hiraishin.ObserverPattern
{
    public class EventDispatcher : MonoBehaviour
    {
        private static EventDispatcher instance;
        private Dictionary<EventID, Action<object>> Listeners = new Dictionary<EventID, Action<object>>();
        public static EventDispatcher Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject DispatcherObject = new GameObject();
                    instance = DispatcherObject.AddComponent<EventDispatcher>();
                    DispatcherObject.name = "Event Dispatcher";
                }
                return instance;
            }
            private set { }
        }

        private void Awake()
        {
            if (instance != null && instance.GetInstanceID() != this.GetInstanceID())
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                instance = this as EventDispatcher;
            }
        }
        private void OnDestroy()
        {
            if (instance == this)
            {
                ClearAllListener();
            }
        }
        public void RegisterListener(EventID _event, Action<object> callback)
        {
            if (Listeners.ContainsKey(_event))
            {
                Listeners[_event] += callback;
            }
            else
            {
                Listeners.Add(_event, null);
                Listeners[_event] += callback;
            }
        }
        public void PostEvent(EventID _event, object param = null)
        {
            if (!Listeners.ContainsKey(_event))
            {
                Debug.Log("No Event in Listener");
                return;
            }
            Action<object> callback = Listeners[_event];
            if (callback != null)
            {
                callback(param);
            }
            else
            {
                Debug.Log("No Listener");
                Listeners.Remove(_event);
            }
        }
        public void RemoveListener(EventID _event, Action<object> callback)
        {
            if (Listeners.ContainsKey(_event))
            {
                Listeners[_event] -= callback;
            }
        }
        public void ClearAllListener()
        {
            Listeners.Clear();
        }
    }

    public static class EventDispatcherExtension
    {
        #region MonoBehaviour Extension
        public static void RegisterListener(this MonoBehaviour listener, EventID _event, Action<object> callback)
        {
            EventDispatcher.Instance.RegisterListener(_event, callback);
        }
        public static void PostEvent(this MonoBehaviour listener, EventID _event)
        {
            EventDispatcher.Instance.PostEvent(_event);
        }
        public static void PostEvent(this MonoBehaviour listener, EventID _event, object param)
        {
            EventDispatcher.Instance.PostEvent(_event, param);
        }
        public static void RemoveListener(this MonoBehaviour listener, EventID _event, Action<object> callback)
        {
            EventDispatcher.Instance.RemoveListener(_event, callback);
        }
        #endregion
        #region Non-MonoBehaviour Extension
        public static void RegisterListener(EventID _event, Action<object> callback)
        {
            EventDispatcher.Instance.RegisterListener(_event, callback);
        }
        public static void PostEvent(EventID _event)
        {
            EventDispatcher.Instance.PostEvent(_event);
        }
        public static void PostEvent(EventID _event, object param)
        {
            EventDispatcher.Instance.PostEvent(_event, param);
        }
        public static void RemoveListener(EventID _event, Action<object> callback)
        {
            EventDispatcher.Instance.RemoveListener(_event, callback);
        }
        #endregion
    }
}

