using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace HealthGateMod.Common.Configs {
    public class PreferenceConfig : ModConfig {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        /// <summary>
        /// Whether or not to player a client-only sound when Health Gated.
        /// </summary>
        [DefaultValue(true)]
        public bool playSoundOnHealthGate;

        /// <summary>
        /// Whether or not to show, roughly, how much health will remain after
        /// being health gated on the health UI.
        /// </summary>
        [DefaultValue(true)]
        public bool showGatingPercentageOnHealthBar;
    }
}