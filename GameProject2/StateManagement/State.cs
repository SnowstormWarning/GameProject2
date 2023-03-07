using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject2.StateManagement
{
    public enum StateCommands
    {
        None,
        RemoveSource,
        AddTarget
    }

    public abstract class State
    {
        private static SpriteFont _font => Game1.Font;

        public abstract State GetTargetState();

        public abstract StateCommands GetStateCommand();
        public abstract void Initialize();
        public abstract void LoadContent(ContentManager content);

        public abstract void Update(GameTime gameTime,MouseState mouse, KeyboardState keys);

        public abstract void Draw(GameTime gameTime,SpriteBatch spriteBatch, SpriteFont font);
    }
}
