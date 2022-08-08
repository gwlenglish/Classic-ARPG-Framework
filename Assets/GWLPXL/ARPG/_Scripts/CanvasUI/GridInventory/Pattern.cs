using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.CanvasUI.com
{
    public interface IPattern
    {
        List<Vector2Int> GetCurrentPattern();
        List<Vector2Int> GetRotatedPattern(PatternAlignment rotation);
        PatternAlignment CurrrentRotation { get; set; }
    }

    /// <summary>
    /// defines an item's grid pattern on the board
    /// </summary>
    [System.Serializable]
    public class Pattern : IPattern
    {
        public PatternAlignment CurrrentRotation { get => currentRotation; set => currentRotation = value; }
        public string Name = default;
        public string Description = default;
        public List<Vector2Int> GridPattern = new List<Vector2Int>();

        protected PatternAlignment currentRotation = PatternAlignment.Vertical;
        public GameObject PatternPrefab = default;



        public Pattern(GameObject prefab, List<Vector2Int> pattern, string name)
        {
            Name = name;
            PatternPrefab = prefab;
            GridPattern = pattern;
        }

        public virtual List<Vector2Int> GetCurrentPattern()
        {
            if (GridPattern.Count == 0)
            {
                GridPattern.Add(new Vector2Int(0, 0));//if not pattern, always default to single cell
            }
            return GetRotatedPattern(currentRotation);
        }

        public virtual List<Vector2Int> GetRotatedPattern(PatternAlignment rotation)
        {
            List<Vector2Int> Pattern = new List<Vector2Int>(GridPattern.Count);
            switch (rotation)
            {
                case PatternAlignment.Vertical:
                    AddRotated(Pattern, 0);
                    break;
                case PatternAlignment.Horizontal:
                    AddRotated(Pattern, 90);
                    break;
            }

            return Pattern;
        }



        protected virtual void AddRotated(List<Vector2Int> Pattern, float delta)
        {
            for (int i = 0; i < GridPattern.Count; i++)
            {
                Vector3 rotated = Rotate(GridPattern[i], delta * Mathf.Deg2Rad);
                Vector2Int inted = new Vector2Int((int)rotated.x, (int)rotated.y);
                Pattern.Add(inted);
            }
        }

        protected virtual Vector2 Rotate(Vector2 v, float delta)
        {
            return new Vector2(
                v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
                v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
            );
        }
    }
}