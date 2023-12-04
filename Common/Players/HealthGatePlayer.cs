using System;
using HealthGateMod.Common.Configs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace HealthGateMod.Common.Players {
    public class HealthGatePlayer : ModPlayer {
        private int _maxHealthGateDamage;

        public override void ModifyHurt(ref Player.HurtModifiers modifiers) {
            BalanceConfig balanceConfig = ModContent.GetInstance<BalanceConfig>();

            _maxHealthGateDamage = -1;
            if (Player.statLife < Player.statLifeMax2 * balanceConfig.requiredPercentageForProtection
                || !balanceConfig.gatingProtectsAgainstFallDamage && modifiers.DamageSource.SourceOtherIndex == 0
                || !balanceConfig.allowGatingInPvp && modifiers.PvP) {
                return;
            }

            modifiers.SetMaxDamage(_maxHealthGateDamage = Player.statLife - (int)Math.Max(1f, Math.Ceiling(Player.statLifeMax2 * balanceConfig.remainingPercentageAfterGate)));
        }

        public override void PostHurt(Player.HurtInfo info) {
            BalanceConfig balanceConfig = ModContent.GetInstance<BalanceConfig>();
            if (info.Damage != _maxHealthGateDamage) {
                return;
            }

            CombatText.NewText(Player.Hitbox, Color.GhostWhite, "!!!", true);
            if (balanceConfig.gatingImmunityTicks > 0) {
                Player.AddImmuneTime(info.CooldownCounter, balanceConfig.gatingImmunityTicks);
            }

            if (ModContent.GetInstance<PreferenceConfig>().playSoundOnHealthGate && Main.myPlayer == Player.whoAmI) {
                SoundEngine.PlaySound(SoundID.Thunder with { Variants = new ReadOnlySpan<int>(new[] { 0 }) });
            }
        }
    }
}