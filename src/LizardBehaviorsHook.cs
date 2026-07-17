using System;
using System.Linq;
using MonoMod.Cil;
using RWCustom;

namespace LizardOnBackMod
{
    public class LizardBehaviorsHook
    {
        public static void Hook()
        {
            On.LizardAI.DetermineBehavior += LizardAI_DetermineBehavior;//防止在背上的蜥蜴像回家

            On.Creature.SuckedIntoShortCut += SuckedIntoShortCut;//如果是蜥蜴回家就松手
        }

        private static void SuckedIntoShortCut(On.Creature.orig_SuckedIntoShortCut orig, Creature self, IntVector2 entrancePos, bool carriedByOther)
        {
            //如果蜥蜴回家就让他手上的玩家手松开
            if (self.room.shortcutData(entrancePos).shortCutType == ShortcutData.Type.CreatureHole)
            {
                if (self is Lizard lizard)
                {
                    for (int i = lizard.grabbedBy.Count - 1; i >= 0; i--)
                    {
                        var grab = lizard.grabbedBy[i];
                        if (grab?.grabber is Player player)
                        {
                            player.ReleaseGrasp(player.grasps.IndexOf(grab));
                        }
                    }
                    if(lizard.GetExLizardData().ownerPlayer!=null)
                    {
                        LizardOnBackHook.LizardOnBack.GetLizardOnBackData(lizard.GetExLizardData().ownerPlayer).DropLizard();
                    }
                }
            }
            orig.Invoke(self, entrancePos, carriedByOther);
        }
       private static LizardAI.Behavior LizardAI_DetermineBehavior(On.LizardAI.orig_DetermineBehavior orig, LizardAI self)
        {
            LizardAI.Behavior behavior = orig(self);
            if (self.lizard.GetExLizardData().ownerPlayer != null)
            {
                // 使用枚举类型判断蜥蜴的行为,如果打算在背上回家就阻止
                if (behavior == LizardAI.Behavior.ReturnPrey ||
                    behavior == LizardAI.Behavior.EscapeRain ||
                    behavior == LizardAI.Behavior.Injured)
                {
                    behavior = LizardAI.Behavior.FollowFriend;
                }
            }
            return behavior;
        }
    }
}