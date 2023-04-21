using GameProject2.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject2.Tools;

namespace GameProject2.StateManagement
{
    public class MainMenu : State
    {
        private StaticSprite _gameTitle;
        private Button _start;
        private Button _exit;
        private StateCommands _command = StateCommands.None;
        private State _targetState;

        public override void Initialize()
        {
            _gameTitle = new StaticSprite("DownfallTitleImage",PosTool.RelativeVector(0.5f,0.25f, 2560f * 0.2f, 1440f * 0.2f),0.2f);
            _start = new Button("StartButton", PosTool.RelativeVector(0.5f,0.5f,2560f*0.05f,1440f*0.05f), 0.05f);
            _exit = new Button("ExitButton", PosTool.RelativeVector(0.5f, 0.6f, 2560f * 0.05f, 1440f * 0.05f), 0.05f);
            _start.ChangeHoverColor(Color.Blue);
            _exit.ChangeHoverColor(Color.Maroon);
            _start.ChangeClickColor(Color.LightBlue);
            _exit.ChangeClickColor(Color.Red);
        }

        public override void LoadContent(ContentManager content)
        {
            _gameTitle.LoadContent(content);
            _start.LoadContent(content);
            _exit.LoadContent(content);
        }

        public override void Update(GameTime gameTime,MouseState mouse, KeyboardState keys)
        {
            _start.Update(mouse);
            _exit.Update(mouse);
            if(_command != StateCommands.None) _command = StateCommands.None;
            if (_exit.IsClickedOn)
            {
                StateManager.CloseGame = true;
            }
            if (_start.IsClickedOn)
            {
                _targetState = new StandardGame(this);
                _command = StateCommands.AddTarget;

            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont font)
        {
            _gameTitle.Draw(gameTime, spriteBatch);
            _start.Draw(gameTime, spriteBatch);
            _exit.Draw(gameTime, spriteBatch);
            spriteBatch.DrawString(font, "With Music By: Noah Mikulin", PosTool.RelativeVector(0.45f, 0.28f), Color.White);
        }

        public override StateCommands GetStateCommand()
        {
            return _command;
        }

        public override State GetTargetState()
        {
            return _targetState;
        }
    }
}
