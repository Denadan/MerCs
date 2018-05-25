using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public interface IState<TEnum>
    {
        TEnum State { get; }

        void OnLoad();
        void OnUnload();
    }

    public abstract class StateMachine<TEnum, BaseState> : MonoBehaviour
        where BaseState : IState<TEnum>
        where TEnum : IComparable
    {
        private Dictionary<TEnum, BaseState> _states = new Dictionary<TEnum, BaseState>();
        private TEnum currentState;

        protected BaseState curStateHandler { get; private set; }
        public TEnum StartState;

        public TEnum LastState { get; private set; }

        public TEnum State
        {
            get => currentState;
            set
            {
#if UNITY_EDITOR
                Debug.Log($"Change State {currentState} => {value}");
#endif
                LastState = currentState;
                curStateHandler?.OnUnload();
                currentState = value;
                curStateHandler = _states[value];
                curStateHandler?.OnLoad();
            }
        }

        protected void addState(BaseState state)
        {
            _states.Add(state.State, state);
        }

        protected virtual void Start()
        {
            StartCoroutine(WaitForReady());
        }

        private IEnumerator WaitForReady()
        {
            while (!Ready)
            {
                yield return new WaitForFixedUpdate();
            }

            State = StartState;
        }

        protected abstract bool Ready { get; }
    }
}