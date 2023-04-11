using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.WIC;
using GameProject2.Tools;

namespace GameProject2.Sprites
{
    public class BlockClearParticleSystem : ParticleSystem
    {
        public BlockClearParticleSystem(Game game, int maxParticles) : base(game, maxParticles * 25) { }

        protected override void InitializeConstants()
        {
            textureFilename = "Particle";
            minNumParticles = 50;
            maxNumParticles = 100;

            blendState = BlendState.Additive;
            DrawOrder = AdditiveBlendDrawOrder;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            var velocity = RandomHelper.NextHorizontalDirection() * RandomHelper.NextFloat(200, 700);
            var lifetime = RandomHelper.NextFloat(0.5f, 1.0f);
            var acceleration = -velocity / lifetime;
            var rotation = RandomHelper.NextFloat(0, MathHelper.TwoPi);
            var angularVelocity = RandomHelper.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4);

            p.Initialize(where, velocity, acceleration, lifetime: lifetime, rotation: rotation, angularVelocity: angularVelocity);
        }

        protected override void UpdateParticle(ref Particle particle, float dt)
        {
            base.UpdateParticle(ref particle, dt);
            float normalizedLifetime = particle.TimeSinceStart / particle.Lifetime;
            float alpha = 12 * normalizedLifetime * (1 - normalizedLifetime);

            Random random = new Random();
            if (random.Next(1, 3) == 1)
            {
                particle.Color = Color.Blue;
            }
            else
            {
                particle.Color = Color.Violet;
            }


            particle.Scale = 1.50f + .70f * normalizedLifetime;
        }

        public void PlaceExplosion(Vector2 where) => AddParticles(where);
    }
}
