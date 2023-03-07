using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject2.Sprites
{
    public class StaticSprite
    {
        /// <summary>
        /// Sprite's texture
        /// </summary>
        private Texture2D _texture;
        /// <summary>
        /// Position of Sprite
        /// </summary>
        public Vector2 Position;
        /// <summary>
        /// Scale of the Sprite's Rendering
        /// </summary>
        public float Scale = 1;
        /// <summary>
        /// The string for loading the static sprite's texture.
        /// </summary>
        public string TextureName;

        public StaticSprite(string Texture)
        {
            TextureName = Texture;
        }

        public StaticSprite(string Texture,Vector2 position, float scale)
        {
            TextureName=Texture;
            Position = position;
            Scale = scale*Game1.GlobalScalingFactor.X;
        }

        /// <summary>
        /// Loads the Sprite's sprite texture
        /// </summary>
        /// <param name="content">The content manager</param>
        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>(TextureName);

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Draw(gameTime, spriteBatch, Color.White);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(_texture, Position, null, color, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }
    }
}
