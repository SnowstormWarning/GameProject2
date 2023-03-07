using GameProject2.StateManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameProject2.Tools
{
    public static class PosTool
    {
        public static Vector2 RelativeVector(float xFactor, float yFactor)
        {
            return new Vector2(StateManager.GraphicsDevice.Viewport.Width*xFactor,StateManager.GraphicsDevice.Viewport.Height*yFactor);
        }

        public static Vector2 RelativeVector(float xFactor, float yFactor,float width, float height)
        {
            return new Vector2(StateManager.GraphicsDevice.Viewport.Width * xFactor-width/2f, StateManager.GraphicsDevice.Viewport.Height * yFactor-width/2f);
        }
    }
}
