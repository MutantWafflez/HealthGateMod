using System.ComponentModel;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Config;

namespace HealthGateMod.Common.Configs {
    public class BalanceConfig : ModConfig {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        /// <summary>
        /// What percentage of health is required for the player to be protected
        /// by Health Gating.
        /// </summary>
        [Range(0.01f, 1f)]
        [DefaultValue(0.5f)]
        public float requiredPercentageForProtection;

        /// <summary>
        /// What percentage of health the player will have left after being Health
        /// Gated.
        /// </summary>
        [Range(0.01f, 1f)]
        [DefaultValue(0.05f)]
        public float remainingPercentageAfterGate;

        /// <summary>
        /// How many additional ticks of immunity are granted to the player after
        /// they are Health Gated.
        /// </summary>
        [Range(0, 6000)]
        [DefaultValue(120)]
        public int gatingImmunityTicks;

        /// <summary>
        /// Whether or not Health Gating protects against fall damage.
        /// </summary>
        [DefaultValue(false)]
        public bool gatingProtectsAgainstFallDamage;

        /// <summary>
        /// Whether or not players will be protected by Health Gating in PVP.
        /// </summary>
        [DefaultValue(false)]
        public bool allowGatingInPvp;

        [OnDeserialized]
        public void ClampValues(StreamingContext context) {
            requiredPercentageForProtection = MathHelper.Clamp(requiredPercentageForProtection, 0.01f, 1f);
            remainingPercentageAfterGate = MathHelper.Clamp(remainingPercentageAfterGate, 0.01f, 1f);
        }
    }
}