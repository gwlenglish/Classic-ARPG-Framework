
using GWLPXL.ARPGCore.Items.com;

namespace GWLPXL.ARPGCore.com
{

    public interface IDescribeEquipment
    {
        void SetHighlightedItem(Item highlightedItem);
        void SetMyEquipment(Item myequipment);
        void EnableComparisonPanel();
        void DisableComparisonPanel();
        void DescribeEquippedEquipment(string description);
        void DescribeHighlightedEquipment(string description);

    }
}