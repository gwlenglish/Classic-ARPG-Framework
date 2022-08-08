using System;
using GWLPXL.ARPG._Scripts.Editor.FuzzySharp.SimilarityRatio.Strategy.Generic;

namespace GWLPXL.ARPG._Scripts.Editor.FuzzySharp.SimilarityRatio.Scorer.StrategySensitive.TokenDifference
{
    public class PartialTokenDifferenceScorer : TokenDifferenceScorerBase
    {
        protected override Func<string[], string[], int> Scorer => PartialRatioStrategy<string>.Calculate;
    }
}
