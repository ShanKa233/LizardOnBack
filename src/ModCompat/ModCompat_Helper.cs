using System.Linq;
using LizardOnBack;

namespace LizardOnBackMod.ModCompat
{
    public class ModCompat_Helper
    {
        public static bool IsImprovedInputMod_Enabled => ModManager.ActiveMods.Any(x => x.id == "improved-input-config");

        public static void Initialize()
        {

            LizardOnBackMod_RWInputMod_Compat.RWInputMod.Initialize();
            // UnityEngine.Debug.Log("ModCompat_Helper.Initialize加载成功");
        }


    }
}