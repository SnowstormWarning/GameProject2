using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using GameProject2;

namespace Basic3D
{
    /// <summary>
    /// A class for rendering a single triangle
    /// </summary>
    public class Triangle
    {
        /// <summary>
        /// The vertices of the triangle
        /// </summary>
        VertexPositionColor[] vertices;

        /// <summary>
        /// The effect to use rendering the triangle
        /// </summary>
        BasicEffect effect;

        Game game;

        /// <summary>
        /// Constructs a triangle instance
        /// </summary>
        /// <param name="game">The game that is creating the triangle</param>
        public Triangle(Game game)
        {
            this.game = game;
            InitializeVertices();
            InitializeEffect();
        }

        /// <summary>
        /// Initializes the vertices of the triangle
        /// </summary>
        private void InitializeVertices()
        {
            vertices = new VertexPositionColor[3];
            // vertex 0
            vertices[0].Position = new Vector3(0, 1, 0);
            vertices[0].Color = Color.Red;
            // vertex 1
            vertices[1].Position = new Vector3(1, 1, 0);
            vertices[1].Color = Color.Green;
            // vertex 2 
            vertices[2].Position = new Vector3(1, 0, 0);
            vertices[2].Color = Color.Blue;
        }

        /// <summary>
        /// Initializes the BasicEffect to render our triangle
        /// </summary>
        private void InitializeEffect()
        {
            effect = new BasicEffect(game.GraphicsDevice);
            effect.World = Matrix.Identity;
            effect.View = Matrix.CreateLookAt(
                new Vector3(0, 0, 4), // The camera position
                new Vector3(0, 0, 0), // The camera target,
                Vector3.Up            // The camera up vector
            );
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,                         // The field-of-view 
                game.GraphicsDevice.Viewport.AspectRatio,   // The aspect ratio
                0.1f, // The near plane distance 
                100.0f // The far plane distance
            );
            effect.VertexColorEnabled = true;
        }

        /// <summary>
        /// Draws the triangle
        /// </summary>
        public void Draw()
        {
            // Cache old rasterizer state
            RasterizerState oldState = game.GraphicsDevice.RasterizerState;

            // Disable backface culling 
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            game.GraphicsDevice.RasterizerState = rasterizerState;

            // Apply our effect
            effect.CurrentTechnique.Passes[0].Apply();

            // Draw the triangle
            game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                PrimitiveType.TriangleList,
                vertices,       // The vertex data 
                0,              // The first vertex to use
                1               // The number of triangles to draw
            );

            // Restore the prior rasterizer state 
            game.GraphicsDevice.RasterizerState = oldState;
        }

        /// <summary>
        /// Rotates the triangle around the y-axis
        /// </summary>
        /// <param name="gameTime">The GameTime object</param>
        public void Update(GameTime gameTime)
        {
            float angle = (float)gameTime.TotalGameTime.TotalSeconds;
            effect.World = Matrix.CreateRotationY(angle);
        }
    }

    /// <summary>
    /// A class representing a quad (a rectangle composed of two triangles)
    /// </summary>
    public class Quad
    {
        /// <summary>
        /// The vertices of the quad
        /// </summary>
        VertexPositionTexture[] vertices;

        /// <summary>
        /// The vertex indices of the quad
        /// </summary>
        short[] indices;

        /// <summary>
        /// The effect to use rendering the triangle
        /// </summary>
        BasicEffect effect;

        /// <summary>
        /// The game this cube belongs to 
        /// </summary>
        Game game;

        /// <summary>
        /// Initializes the vertices of our quad
        /// </summary>
        public void InitializeVertices()
        {
            vertices = new VertexPositionTexture[4];
            // Define vertex 0 (top left)
            vertices[0].Position = new Vector3(-1, 1, 0);
            vertices[0].TextureCoordinate = new Vector2(0, -1);
            // Define vertex 1 (top right)
            vertices[1].Position = new Vector3(1, 1, 0);
            vertices[1].TextureCoordinate = new Vector2(1, -1);
            // define vertex 2 (bottom right)
            vertices[2].Position = new Vector3(1, -1, 0);
            vertices[2].TextureCoordinate = new Vector2(1, 0);
            // define vertex 3 (bottom left) 
            vertices[3].Position = new Vector3(-1, -1, 0);
            vertices[3].TextureCoordinate = new Vector2(0, 0);
        }

        /// <summary>
        /// Initialize the indices of our quad
        /// </summary>
        public void InitializeIndices()
        {
            indices = new short[6];

            // Define triangle 0 
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            // define triangle 1
            indices[3] = 2;
            indices[4] = 3;
            indices[5] = 0;
        }

        /// <summary>
        /// Initializes the basic effect used to draw the quad
        /// </summary>
        public void InitializeEffect()
        {
            effect = new BasicEffect(game.GraphicsDevice);
            effect.World = Matrix.Identity;
            effect.View = Matrix.CreateLookAt(
                new Vector3(0, 0, 4), // The camera position
                new Vector3(0, 0, 0), // The camera target,
                Vector3.Up            // The camera up vector
            );
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,                         // The field-of-view 
                game.GraphicsDevice.Viewport.AspectRatio,   // The aspect ratio
                0.1f, // The near plane distance 
                100.0f // The far plane distance
            );
            effect.TextureEnabled = true;
            effect.Texture = game.Content.Load<Texture2D>("monogame-logo");
        }

        /// <summary>
        /// Draws the quad
        /// </summary>
        public void Draw()
        {
            // Cache the old blend state 
            BlendState oldBlendState = game.GraphicsDevice.BlendState;

            // Enable alpha blending 
            game.GraphicsDevice.BlendState = BlendState.AlphaBlend;

            // Apply our effect
            effect.CurrentTechnique.Passes[0].Apply();

            // Render the quad
            game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(
                PrimitiveType.TriangleList,
                vertices,   // The vertex collection
                0,          // The starting index in the vertex array
                4,          // The number of indices in the shape
                indices,    // The index collection
                0,          // The starting index in the index array
                2           // The number of triangles to draw
            );

            // Restore the old blend state 
            game.GraphicsDevice.BlendState = oldBlendState;
        }

        /// <summary>
        /// Constructs the Quad
        /// </summary>
        /// <param name="game">The Game the Quad belongs to</param>
        public Quad(Game game)
        {
            this.game = game;
            InitializeVertices();
            InitializeIndices();
            InitializeEffect();
        }

    }

    /// <summary>
    /// A class for rendering a cube
    /// </summary>
    public class Cube
    {
        /// <summary>
        /// The vertices of the cube
        /// </summary>
        VertexBuffer vertices;

        /// <summary>
        /// The vertex indices of the cube
        /// </summary>
        IndexBuffer indices;

        /// <summary>
        /// The effect to use rendering the cube
        /// </summary>
        BasicEffect effect;

        /// <summary>
        /// The game this cube belongs to 
        /// </summary>
        Game game;

        /// <summary>
        /// Constructs a cube instance
        /// </summary>
        /// <param name="game">The game that is creating the cube</param>
        public Cube(Game1 game)
        {
            this.game = game;
            InitializeVertices();
            InitializeIndices();
            InitializeEffect();
        }


        /// <summary>
        /// Initialize the vertex buffer
        /// </summary>
        public void InitializeVertices()
        {
            var vertexData = new VertexPositionColor[] {
            new VertexPositionColor() { Position = new Vector3(-3,  3, -3), Color = Color.Blue },
            new VertexPositionColor() { Position = new Vector3( 3,  3, -3), Color = Color.Green },
            new VertexPositionColor() { Position = new Vector3(-3, -3, -3), Color = Color.Red },
            new VertexPositionColor() { Position = new Vector3( 3, -3, -3), Color = Color.Cyan },
            new VertexPositionColor() { Position = new Vector3(-3,  3,  3), Color = Color.Blue },
            new VertexPositionColor() { Position = new Vector3( 3,  3,  3), Color = Color.Red },
            new VertexPositionColor() { Position = new Vector3(-3, -3,  3), Color = Color.Green },
            new VertexPositionColor() { Position = new Vector3( 3, -3,  3), Color = Color.Cyan }
        };
            vertices = new VertexBuffer(
                game.GraphicsDevice,            // The graphics device to load the buffer on 
                typeof(VertexPositionColor),    // The type of the vertex data 
                8,                              // The count of the vertices 
                BufferUsage.None                // How the buffer will be used
            );
            vertices.SetData<VertexPositionColor>(vertexData);
        }

        /// <summary>
        /// Initializes the index buffer
        /// </summary>
        public void InitializeIndices()
        {
            var indexData = new short[]
            {
            0, 1, 2, // Side 0
            2, 1, 3,
            4, 0, 6, // Side 1
            6, 0, 2,
            7, 5, 6, // Side 2
            6, 5, 4,
            3, 1, 7, // Side 3 
            7, 1, 5,
            4, 5, 0, // Side 4 
            0, 5, 1,
            3, 7, 2, // Side 5 
            2, 7, 6
            };
            indices = new IndexBuffer(
                game.GraphicsDevice,            // The graphics device to use
                IndexElementSize.SixteenBits,   // The size of the index 
                36,                             // The count of the indices
                BufferUsage.None                // How the buffer will be used
            );
            indices.SetData<short>(indexData);
        }

        /// <summary>
        /// Initializes the BasicEffect to render our cube
        /// </summary>
        void InitializeEffect()
        {
            effect = new BasicEffect(game.GraphicsDevice);
            effect.World = Matrix.Identity;
            effect.View = Matrix.CreateLookAt(
                new Vector3(0, 0, 4), // The camera position
                new Vector3(0, 0, 0), // The camera target,
                Vector3.Up            // The camera up vector
            );
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,                         // The field-of-view 
                game.GraphicsDevice.Viewport.AspectRatio,   // The aspect ratio
                0.1f, // The near plane distance 
                100.0f // The far plane distance
            );
            effect.VertexColorEnabled = true;
        }


        /// <summary>
        /// Draws the Cube
        /// </summary>
        public void Draw()
        {
            // apply the effect 
            effect.CurrentTechnique.Passes[0].Apply();
            // set the vertex buffer
            game.GraphicsDevice.SetVertexBuffer(vertices);
            // set the index buffer
            game.GraphicsDevice.Indices = indices;
            // Draw the triangles
            game.GraphicsDevice.DrawIndexedPrimitives(
                PrimitiveType.TriangleList, // Tye type to draw
                0,                          // The first vertex to use
                0,                          // The first index to use
                12                          // the number of triangles to draw
            );
        }

        /// <summary>
        /// Updates the Cube
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            float angle = (float)gameTime.TotalGameTime.TotalSeconds;
            // Look at the cube from farther away while spinning around it
            effect.View = Matrix.CreateRotationY(angle) * Matrix.CreateLookAt(
                new Vector3(0, 5, -10),
                Vector3.Zero,
                Vector3.Up
            );
        }


    }


}