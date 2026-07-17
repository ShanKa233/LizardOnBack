using System;
using System.Collections.Generic;
using ImprovedInput;
using LizardOnBackMod;
using RWCustom;
using UnityEngine;

namespace LizardOnBackMod_RWInputMod_Compat
{
    public static class RWInputMod
    {
        public static void Initialize()
        {
            Initialize_Custom_Keybindings();
            On.Player.checkInput -= Player_CheckInput;
            On.Player.checkInput += Player_CheckInput;
        }

        private static void Player_CheckInput(On.Player.orig_checkInput orig, Player self)
        {
            orig(self);
            //超过的玩家不测
            int player_number = self.playerState.playerNumber;
            if (player_number < 0) return;
            if (player_number >= maximum_number_of_players) return;

            //用这个方法来覆盖正常的那次hook
            LizardOnBackHook.LizardOnBack.GetLizardOnBackData(self).lizardToBackInput = Get_Input(self);
        }


        public static readonly int maximum_number_of_players = RainWorld.PlayerObjectBodyColors.Length;
        public static PlayerKeybind lizardtoback_keybinding = null!;

        public static void Initialize_Custom_Keybindings()
        {
            if (lizardtoback_keybinding != null) return;

            lizardtoback_keybinding = PlayerKeybind.Register(LizardOnBackMod.LizardOnBackPlugin.modID + ":lizardtoback", LizardOnBackMod.LizardOnBackPlugin.modeName
            , Custom.rainWorld.inGameTranslator.Translate("Carry Lizard"), KeyCode.None, KeyCode.None);
            lizardtoback_keybinding.HideConflict = other_keybinding => lizardtoback_keybinding.Can_Hide_Conflict_With(other_keybinding);
            lizardtoback_keybinding.Description = Custom.rainWorld.inGameTranslator.Translate("Button for carrying and dropping lizard, long press to use");
        }
        public static bool Can_Hide_Conflict_With(this PlayerKeybind keybinding, PlayerKeybind other_keybinding)
        {
            for (int player_index_a = 0; player_index_a < maximum_number_of_players; ++player_index_a)
            {
                for (int player_index_b = player_index_a; player_index_b < maximum_number_of_players; ++player_index_b)
                {
                    if (!keybinding.ConflictsWith(player_index_a, other_keybinding, player_index_b)) continue;
                    if (player_index_a != player_index_b) return false;

                    if (other_keybinding == PlayerKeybind.Map) continue;

                    if (other_keybinding == lizardtoback_keybinding) continue;
                    return false;
                }
            }
            return true;
        }

        public static bool Get_Input(Player player)
        {
            int player_number = player.playerState.playerNumber;

            if (lizardtoback_keybinding.Unbound(player_number))
            {
                return player.input[0].pckp;
            }
            else
            {
                return lizardtoback_keybinding.CheckRawPressed(player_number);
            }
        }
    }
}