using System;
using System.Collections.Generic;
namespace GWLPXL.ARPGCore.States.com
{

    public class IStateMachine
    {
        public IState GetCurrentlyRunnnig()
        {
            return currentlyRunningState;
        }
        private IState currentlyRunningState;
        private IState previousState;

        Dictionary<Type, List<Transition>> transitions = new Dictionary<Type, List<Transition>>();
        List<Transition> currentTransitions = new List<Transition>();
        List<Transition> anytransitions = new List<Transition>();//don't have a from state, they always exist

        static List<Transition> EmptyTransitions = new List<Transition>();

        public void Tick()
        {
            //check if state locked
            Transition transition = GetTransition();
            if (transition != null)
                SetState(transition.To);

            currentlyRunningState?.Tick();
        }
        public void AddTransition(IState from, IState to, Func<bool> predicate)
        {
            if (transitions.TryGetValue(from.GetType(), out List<Transition> value) == false)
            {
                value = new List<Transition>();
                transitions[from.GetType()] = value;
            }
            value.Add(new Transition(to, predicate));
        }

        public void AddAnyTransition(IState state, Func<bool> predicate)
        {
            anytransitions.Add(new Transition(state, predicate));
        }

        public void SetState(IState state)
        {
            if (state == currentlyRunningState)
                return;

            currentlyRunningState?.Exit();
            currentlyRunningState = state;

            transitions.TryGetValue(currentlyRunningState.GetType(), out currentTransitions);
            if (currentTransitions == null)
                currentTransitions = EmptyTransitions;

            currentlyRunningState.Enter();

        }
        private class Transition
        {
            public Func<bool> Condition { get; }
            public IState To { get; }
            public Transition(IState to, Func<bool> condition)
            {
                To = to;
                Condition = condition;
            }
        }
        Transition GetTransition()
        {
            foreach (var transition in anytransitions)
            {
                if (transition.Condition())
                    return transition;
            }

            foreach (var transition in currentTransitions)
            {
                if (transition.Condition())
                    return transition;
            }

            return null;
        }


    }
}