﻿using Overlayer.Core.Tags;
using System.Runtime.ConstrainedExecution;

namespace Overlayer.Tags
{
    public static class CurHitTags
    {
        [Tag("CurHit", Category = Category.HitMargin)]
        [ReliabilityContract(Consistency.MayCorruptProcess, Cer.MayFail)]
        public static string Hit() => RDString.Get("HitMargin." + GetCurHitMargin(GCS.difficulty));
        [Tag("CurTE", RelatedPatches = "Overlayer.Patches.GetHitMarginFixer:Prefix", Category = Category.HitMargin)]
        public static double TE() => GetCurDiffCount(HitMargin.TooEarly);
        [Tag("CurVE", RelatedPatches = "Overlayer.Patches.GetHitMarginFixer:Prefix", Category = Category.HitMargin)]
        public static double VE() => GetCurDiffCount(HitMargin.VeryEarly);
        [Tag("CurEP", RelatedPatches = "Overlayer.Patches.GetHitMarginFixer:Prefix", Category = Category.HitMargin)]
        public static double EP() => GetCurDiffCount(HitMargin.EarlyPerfect);
        [Tag("CurP", RelatedPatches = "Overlayer.Patches.GetHitMarginFixer:Prefix", Category = Category.HitMargin)]
        public static double P() => GetCurDiffCount(HitMargin.Perfect);
        [Tag("CurLP", RelatedPatches = "Overlayer.Patches.GetHitMarginFixer:Prefix", Category = Category.HitMargin)]
        public static double LP() => GetCurDiffCount(HitMargin.LatePerfect);
        [Tag("CurVL", RelatedPatches = "Overlayer.Patches.GetHitMarginFixer:Prefix", Category = Category.HitMargin)]
        public static double VL() => GetCurDiffCount(HitMargin.VeryLate);
        [Tag("CurTL", RelatedPatches = "Overlayer.Patches.GetHitMarginFixer:Prefix", Category = Category.HitMargin)]
        public static double TL() => GetCurDiffCount(HitMargin.TooLate);
        [Tag("CurDifficulty", Category = Category.Play)]
        [ReliabilityContract(Consistency.MayCorruptProcess, Cer.MayFail)]
        public static string Difficulty() => RDString.Get("enum.Difficulty." + GCS.difficulty);
        [Tag("Combo", RelatedPatches = "Overlayer.Patches.GetHitMarginFixer:Prefix", Category = Category.Play)]
        public static double Combo() => Variables.Combo;
        [Tag("MissCount", Category = Category.HitMargin)]
        public static double MissCount() => scrController.instance?.mistakesManager.GetHits(HitMargin.FailMiss) ?? 0;
        [Tag("Overloads", Category = Category.HitMargin)]
        public static double Overloads() => scrController.instance?.mistakesManager.GetHits(HitMargin.FailOverload) ?? 0;
        public static int GetCurDiffCount(HitMargin hit)
        {
            switch (GCS.difficulty)
            {
                case global::Difficulty.Lenient: return Variables.LenientCounts[hit];
                case global::Difficulty.Normal: return Variables.NormalCounts[hit];
                case global::Difficulty.Strict: return Variables.StrictCounts[hit];
                default: return 0;
            }
        }
        public static HitMargin GetCurHitMargin(Difficulty diff)
        {
            switch (diff)
            {
                case global::Difficulty.Lenient: return Variables.Lenient;
                case global::Difficulty.Normal: return Variables.Normal;
                case global::Difficulty.Strict: return Variables.Strict;
                default: return 0;
            }
        }
    }
}
