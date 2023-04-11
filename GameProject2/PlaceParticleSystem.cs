using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.WIC;
using GameProject2.Tools;

namespace GameProject2
{
    public class PlaceParticleSystem: ParticleSystem
    {
        public PlaceParticleSystem(Game game, int maxParticles) : base(game, maxParticles * 25) { }

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
            var velocity = RandomHelper.NextHorizontalDirection() * RandomHelper.NextFloat(10, 35);
            var lifetime = RandomHelper.NextFloat(0.5f, 0.7f);
            var acceleration = -velocity / lifetime;
            var rotation = RandomHelper.NextFloat(0, MathHelper.TwoPi);
            var angularVelocity = RandomHelper.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4);

            p.Initialize(where, velocity, acceleration, lifetime: lifetime, rotation: rotation, angularVelocity: angularVelocity);
        }

        protected override void UpdateParticle(ref Particle particle, float dt)
        {
            base.UpdateParticle(ref particle, dt);
            float normalizedLifetime = particle.TimeSinceStart / particle.Lifetime;
            float alpha = 2 * normalizedLifetime * (1 - normalizedLifetime);

            Random random = new Random();
            particle.Color = Color.DarkGray *alpha;


            particle.Scale = 1.50f + 1f * normalizedLifetime;
        }

        public void PlaceExplosion(Vector2 where) => AddParticles(where);
    }
}
