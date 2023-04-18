using Basic3D;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject2._3D
{
    public class StaticCamera: ICamera
    {
        #region Fields

        // The camera's angle 
        float angle;

        // The camera's position
        Vector3 position;

        // The game this camera belongs to 
        Game game;

        // The view matrix 
        Matrix view;

        // The projection matrix 
        Matrix projection;

        #endregion

        #region Properties

        /// <summary>
        /// The camera's view matrix 
        /// </summary>
        public Matrix View => view;

        /// <summary>
        /// The camera's projection matrix 
        /// </summary>
        public Matrix Projection => projection;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructs a new camera that circles the origin
        /// </summary>
        /// <param name="game">The game this camera belongs to</param>
        /// <param name="position">The initial position of the camera</param>
        /// <param name="speed">The speed of the camera</param>
        public StaticCamera(Game game, Vector3 position)
        {
            this.game = game;
            this.position = position;
            this.projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                game.GraphicsDevice.Viewport.AspectRatio,
                1,
                1000
            );
            this.view = Matrix.CreateLookAt(
                position,
                Vector3.Zero,
                Vector3.Up
            );
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the camera's positon
        /// </summary>
        /// <param name="gameTime">The GameTime object</param>
        public void Update(GameTime gameTime)
        {

            // Calculate a new view matrix
            this.view =
                Matrix.CreateRotationY(angle) *
                Matrix.CreateLookAt(position, Vector3.Zero, Vector3.Up);
        }

        #endregion
    }
}
