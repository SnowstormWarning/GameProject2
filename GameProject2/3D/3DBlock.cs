using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic3D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject2._3D
{
    public enum BlockColors
    {
        Empty,
        Settled,
        Active,
        Shadow
    }
    public class ModelBlock
    {
        // The cube's position in the world 
        Vector3 position = Vector3.Zero;

        // The direction the cube is facing
        float facing = 0;

        /// <summary>
        /// The position of the cube in the world 
        /// </summary>
        public Vector3 Position => position;

        /// <summary>
        /// The angle the cube is facing (in radians)
        /// </summary>
        public float Facing => facing;

        public BlockColors Color;

        private Game game;

        Model model;


        public ModelBlock(Game game, Vector3 position)
        {
            this.game = game;
            model = game.Content.Load<Model>("Cube");
            this.position = position;
        }

        public void Draw(ICamera camera, Vector3 color)
        {
            Matrix world = Matrix.CreateRotationY(facing) * Matrix.CreateTranslation(position);

            Matrix view = camera.View;

            Matrix projection = camera.Projection;

            foreach (ModelMesh mesh in model.Meshes)

            {

                foreach (Effect effect in mesh.Effects)

                {

                    if (effect is BasicEffect basic)

                    {
                        basic.EnableDefaultLighting();
                        basic.World = world;
                        basic.View = view;
                        basic.Projection = projection;
                        basic.DiffuseColor = color;

                    }

                }

                mesh.Draw();

            }
            //model.Draw(world, view, projection);
        }
    }
}
