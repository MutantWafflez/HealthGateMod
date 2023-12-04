using System.Diagnostics.CodeAnalysis;
using HealthGateMod.Common.Configs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.ResourceSets;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace HealthGateMod.Common.ResourceOverlays {
    public class GatingOverlay : ModResourceOverlay {
        private bool _drawingLife;

        public override bool PreDrawResourceDisplay(PlayerStatsSnapshot snapshot, IPlayerResourcesDisplaySet displaySet, bool drawingLife, ref Color textColor, [UnscopedRef] out bool drawText) {
            _drawingLife = drawingLife;
            return base.PreDrawResourceDisplay(snapshot, displaySet, drawingLife, ref textColor, out drawText);
        }

        public override bool PreDrawResource(ResourceOverlayDrawContext context) {
            BalanceConfig balanceConfig = ModContent.GetInstance<BalanceConfig>();
            PreferenceConfig preferenceConfig = ModContent.GetInstance<PreferenceConfig>();

            if (!preferenceConfig.showGatingPercentageOnHealthBar
                || !_drawingLife
                || !(context.texture.Name.Contains("_Fill") || context.texture == TextureAssets.Heart || context.texture == TextureAssets.Heart2)
                || context.snapshot.Life < context.snapshot.LifeMax * balanceConfig.requiredPercentageForProtection) {
                return true;
            }

            switch (context.DisplaySet) {
                case HorizontalBarsPlayerResourcesDisplaySet: {
                    if (context.resourceNumber < (int)(context.snapshot.LifeMax * (1f - balanceConfig.remainingPercentageAfterGate) / context.snapshot.LifePerSegment)) {
                        return true;
                    }
                    break;
                }
                default: {
                    if (context.resourceNumber >= (int)MathHelper.Max(1f, context.snapshot.LifeMax * balanceConfig.remainingPercentageAfterGate / context.snapshot.LifePerSegment)) {
                        return true;
                    }
                    break;
                }
            }

            context.SpriteBatch.End();
            context.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.SamplerStateForCursor, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);

            GameShaders.Armor.Apply(
                GameShaders.Armor.GetShaderIdFromItemId(ItemID.HallowBossDye),
                null,
                new DrawData(context.texture.Value, context.position, context.source, context.color, context.rotation, context.origin, context.scale, context.effects)
            );
            context.Draw();

            context.SpriteBatch.End();
            context.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.SamplerStateForCursor, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
            return false;
        }
    }
}