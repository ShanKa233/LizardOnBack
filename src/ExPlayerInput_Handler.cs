using System.Runtime.CompilerServices;
using LizardOnBackMod;

namespace LizardOnBack
{

    public static class ExPlayerInput_Handler
    {
        public static bool lizardToBackInput(this Player self)
        {
            return LizardOnBackHook.LizardOnBack.GetLizardOnBackData(self).lizardToBackInput;
        }
    }

}