using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace GameProject2.StateManagement
{
    internal class StateManager
    {
        private List<State> states;
        public static GraphicsDevice GraphicsDevice;
        public static bool CloseGame;
        public State CurrentState;
        public static Game1 game;

        public StateManager(GraphicsDevice graphicsDevice)
        {
            states = new List<State>();
            GraphicsDevice = graphicsDevice;
        }

        public void AddState(State state, ContentManager content)
        {
            states.Add(state);
            CurrentState = states[states.Count - 1];
            CurrentState.Initialize();
            CurrentState.LoadContent(content);
        }

        public void AddUnloadedState(State state)
        {
            states.Add(state);
            CurrentState = state;

        }

        public void RemoveState(int index)
        {
            if (index >= states.Count || index < 0) return;
            if (CurrentState == states.ElementAt(index))
            {
                if (index > 0) CurrentState = states.ElementAt(index - 1); else CurrentState = states.ElementAt(states.Count - 1);
            }
            states.RemoveAt(index);
        }

        public void RemoveState(State state)
        {
            int? i = states.IndexOf(state);
            if (i != null) RemoveState((int)i); else return;
        }

        public void RemoveCurrentState()
        {
            RemoveState(CurrentState);
        }
    }
}
