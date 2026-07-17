

using UnityEngine;

namespace LizardOnBackMod
{
    public class LostLizardHooks
    {

        // case 0:
        //     return Translate("May drop when stunned, will drop when grabbed by creatures"); // 被击晕时可能掉落，被生物抓住时会掉落
        // case 1:
        //     return Translate("Will drop after being carried by dangerous creatures for a while"); // 被危险生物携带一段时间后会掉落
        // case 2:
        //     return Translate("Only drops when player dies"); // 仅在玩家死亡时掉落
        // case 3:
        //     return Translate("Never drops, even when player dies"); // 永不掉落，即使玩家死亡
        public static void initHook()
        {
            On.Player.Update += PlayerOnUpdate;//被危险生物抓住时放下蜥蜴
            On.Player.Stun += PlayerOnStun;//击晕放下蜥蜴
            On.Player.Die += PlayerOnDie;//死亡放下蜥蜴
        }

        private static void PlayerOnUpdate(On.Player.orig_Update orig, Player self, bool eu)
        {
            orig(self, eu);
            var lizardData = LizardOnBackHook.LizardOnBack.GetLizardOnBackData(self);
            if (lizardData.HasALizard)
            {
                //0和1的时候会丢弃
                if (self.dangerGrasp != null && !self.dangerGrasp.discontinued
                && (LizardOnBackOptions.getOptions().LizardGripStrength.Value <= 1))
                {
                    if (LizardOnBackOptions.getOptions().LizardGripStrength.Value == 0 || (self.dangerGraspTime > 40 * 5))
                    {
                        lizardData.DropLizard();
                    }
                }
            }
        }
        private static void PlayerOnStun(On.Player.orig_Stun orig, Player self, int st)
        {
            var lizardData = LizardOnBackHook.LizardOnBack.GetLizardOnBackData(self);
            if (LizardOnBackOptions.getOptions().LizardGripStrength.Value <= 1)//在2之后就不眩晕丢弃了所以小于2才执行
            {
                int origSt = st;
                if (self.room != null)
                {
                    if (self.Malnourished)
                    {
                        origSt = Mathf.RoundToInt((float)origSt * (self.exhausted ? 2f : 1.5f));
                    }
                    if (origSt > UnityEngine.Random.Range(40, 80) && lizardData.HasALizard && self.stunDamageType != Creature.DamageType.Blunt)
                    {
                        lizardData.DropLizard();
                    }
                }
            }
            orig(self, st);
        }
        private static void PlayerOnDie(On.Player.orig_Die orig, Player self)
        {
            if (LizardOnBackHook.LizardOnBack.GetLizardOnBackData(self).HasALizard
            && LizardOnBackOptions.getOptions().LizardGripStrength.Value <= 2)
            {
                LizardOnBackHook.LizardOnBack.GetLizardOnBackData(self).DropLizard();
            }
            orig(self);
        }


    }
}