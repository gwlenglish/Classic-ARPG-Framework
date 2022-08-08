using System;
using GWLPXL.ARPG._Scripts.Editor.FuzzySharp.SimilarityRatio.Strategy;

namespace GWLPXL.ARPG._Scripts.Editor.FuzzySharp.SimilarityRatio.Scorer.StrategySensitive.TokenSort
{
    public class TokenSortScorer : TokenSortScorerBase
    {
        protected override Func<string, string, int> Scorer => DefaultRatioStrategy.Calculate;
    }
}
