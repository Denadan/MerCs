using System;
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

    public class StateMachine<TEnum, BaseState> : MonoBehaviour
        where BaseState : IState<TEnum>
    {
        private Dictionary<TEnum, BaseState> _states = new Dictionary<TEnum, BaseState>();
        private TEnum currentState;

        protected BaseState curStateHandler { get; private set; }

        public TEnum StartState;

        public TEnum State
        {
            get => currentState;
            set
            {
                if (curStateHandler != null)
                    curStateHandler.OnUnload();
                else
                {
                    currentState = value;
                    curStateHandler = _states[value];
                    curStateHandler.OnLoad();
                }
            }
        }

        protected virtual void Start()
        {
            currentState = StartState;
            curStateHandler = _states[currentState];
        }

        protected void addState(BaseState state)
        {
            _states.Add(state.State, state);
        }
    }
}