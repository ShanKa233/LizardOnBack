using System.Runtime.CompilerServices;

namespace LizardOnBackMod
{
    public static class ExLizardDataHandler
    {
        public static ExLizardData GetExLizardData(this Lizard lizard)
        {
            return ExLizardData.ExLizardDataCWT.GetValue(lizard, _ => new ExLizardData(lizard));
        }
    }
    public class ExLizardData
    {
        public static ConditionalWeakTable<Lizard, ExLizardData> ExLizardDataCWT = new ConditionalWeakTable<Lizard, ExLizardData>();
        public Lizard lizard;
        public Player ownerPlayer;
        public ExLizardData(Lizard lizard)
        {
            this.lizard = lizard;
        }
        public static ExLizardData GetExLizardData(Lizard lizard)
        {
            return ExLizardDataCWT.GetValue(lizard, _ => new ExLizardData(lizard));
        }
        
        
    }
}