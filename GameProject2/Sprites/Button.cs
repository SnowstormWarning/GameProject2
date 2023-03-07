using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;

namespace GameProject2.Sprites
{
    public class Button
    {
        private Color _hoverColor;
        private Color _clickColor;
        private bool _isHoveredOn;
        private bool _isClickedOn;
        public bool IsClickedOn => _isClickedOn;
        private Texture2D _texture;
        public Vector2 Position;
        public float Scale = 1;
        public string TextureName;
        public float Width => 2560*Scale;
        public float Height => 1440*Scale;

        public Button(string TextureName)
        {
            this.TextureName = (TextureName);
        }

        public Button(string textureName,Vector2 position, float scale)
        {
            this.TextureName = (textureName);
            this.Position = position;
            this.Scale = scale* Game1.GlobalScalingFactor.X;
        }

        public void ChangeHoverColor(Color color)
        {
            _hoverColor = color;
        }

        public void ChangeClickColor(Color color)
        {
            _clickColor = color;
        }

        /// <summary>
        /// Loads the Button's sprite texture
        /// </summary>
        /// <param name="content">The content manager</param>
        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>(TextureName);

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_isClickedOn)
            {
                spriteBatch.Draw(_texture, Position, null, _clickColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
            }
            else if(_isHoveredOn)
            {
                spriteBatch.Draw(_texture, Position, null, _hoverColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.Draw(_texture, Position,null, Color.White,0f,Vector2.Zero,Scale,SpriteEffects.None,0);
            }
        }

        public void Update(MouseState mouse)
        {
            if(_isClickedOn) _isClickedOn = false;
            _isHoveredOn = (mouse.Position.X >= Position.X && mouse.Position.X <= Position.X + Width &&
                mouse.Position.Y >= Position.Y && mouse.Position.Y <= Position.Y + Height);

            if(_isHoveredOn && mouse.LeftButton == ButtonState.Pressed)
            {
                _isClickedOn = true;
            }
        }
    }
}
