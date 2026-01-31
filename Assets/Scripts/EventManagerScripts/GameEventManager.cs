using System;
using System.Collections.Generic;
using GameEnums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EventManagerScripts
{
    public sealed class GameEventManager
    {
        private class EventData
        {
            public object Owner;
            public Type PayloadType; // null = no payload
            public Action NoArgCallbacks;
            public Delegate TypedCallbacks;
        }

        private readonly Dictionary<GameEvent, EventData> _events = new();

        // ---------------- INIT / SCENE RESET ----------------

        private void Initialize()
        {
            SceneManager.sceneLoaded += (_, __) => ClearAll();
        }

        private void ClearAll()
        {
            _events.Clear();
        }

        // ---------------- SUBSCRIBE (NO PAYLOAD) ----------------

        public void Subscribe(GameEvent evt, Action callback)
        {
            if (callback == null)
                return;

            var data = GetOrCreate(evt);

            if (data.PayloadType != null)
            {
                Debug.LogError(
                    $"[GameEventManager] Cannot subscribe parameterless callback to payload event {evt} " +
                    $"(expects {data.PayloadType})."
                );
                return;
            }

            data.NoArgCallbacks += callback;
        }

        // ---------------- SUBSCRIBE (T PAYLOAD) ----------------

        public void Subscribe<T>(GameEvent evt, Action<T> callback)
        {
            if (callback == null)
                return;

            var data = GetOrCreate(evt);

            if (data.PayloadType == null && data.NoArgCallbacks != null)
            {
                Debug.LogError(
                    $"[GameEventManager] Cannot subscribe typed callback to no-payload event {evt}."
                );
                return;
            }

            if (data.PayloadType != null && data.PayloadType != typeof(T))
            {
                Debug.LogError(
                    $"[GameEventManager] Payload type mismatch for {evt}. " +
                    $"Expected {data.PayloadType}, got {typeof(T)}."
                );
                return;
            }

            data.PayloadType = typeof(T);
            data.TypedCallbacks = Delegate.Combine(data.TypedCallbacks, callback);
        }

        // ---------------- RAISE (NO PAYLOAD) ----------------

        public void Raise(GameEvent evt, object sender)
        {
            if (!ValidateOwnership(evt, sender))
                return;

            var data = _events[evt];

            if (data.PayloadType != null)
            {
                Debug.LogError(
                    $"[GameEventManager] Event {evt} requires payload of type {data.PayloadType}."
                );
                return;
            }

            data.NoArgCallbacks?.Invoke();
        }

        // ---------------- RAISE (T PAYLOAD) ----------------

        public void Raise<T>(GameEvent evt, object sender, T payload)
        {
            if (!ValidateOwnership(evt, sender))
                return;

            var data = _events[evt];

            if (data.PayloadType == null)
            {
                Debug.LogError(
                    $"[GameEventManager] Event {evt} does not accept payloads."
                );
                return;
            }

            if (data.PayloadType != typeof(T))
            {
                Debug.LogError(
                    $"[GameEventManager] Payload type mismatch for {evt}. " +
                    $"Expected {data.PayloadType}, got {typeof(T)}."
                );
                return;
            }

            if (data.TypedCallbacks is Action<T> typed)
                typed.Invoke(payload);
        }

        // ---------------- INTERNAL HELPERS ----------------

        private EventData GetOrCreate(GameEvent evt)
        {
            if (_events.TryGetValue(evt, out var data)) return data;
            data = new EventData();
            _events[evt] = data;

            return data;
        }

        private bool ValidateOwnership(GameEvent evt, object sender)
        {
            if (sender == null)
            {
                Debug.LogError("[GameEventManager] Sender cannot be null.");
                return false;
            }

            var data = GetOrCreate(evt);

            data.Owner ??= sender;

            if (ReferenceEquals(data.Owner, sender)) return true;
            
            Debug.LogError(
                $"[GameEventManager] Unauthorized raise attempt for {evt}. " +
                $"Owner: {data.Owner}, Sender: {sender}"
            );
            return false;

        }
    }
}