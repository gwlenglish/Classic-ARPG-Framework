using System;
using GWLPXL.ARPGCore.Types.com;

namespace GWLPXL.ARPG._Scripts.Attributes.com
{
	// currently percent is float. 0.1f = 10%. maybe change it
	public enum AttributeModifierType
	{
		Flat = 100,
		PercentAdd = 200,
		PercentMult = 300,
	}
	// maybe do as class
	[Serializable]
	public struct AttributeModifier
	{
		public float Value;
		public AttributeModifierType Type;
		public AttributeModifierOrderType Order;
		public object Source;

		public AttributeModifier(float value, AttributeModifierType type, AttributeModifierOrderType order, object source)
		{
			Value = value;
			Type = type;
			Order = order;
			Source = source;
		}

		public AttributeModifier(float value, AttributeModifierType type) : this(value, type, (AttributeModifierOrderType)type, null) { }

		public AttributeModifier(float value, AttributeModifierType type, AttributeModifierOrderType order) : this(value, type, order, null) { }

		public AttributeModifier(float value, AttributeModifierType type, object source) : this(value, type, (AttributeModifierOrderType)type, source) { }
	}

	/// <summary>
	/// Value is set runtime by code, used in traits with GetLeveledValue()
	/// </summary>
	[Serializable]
	public struct AttributeModifierManaged
	{
		public AttributeModifierType Type;
		public AttributeModifierOrderType Order;
		public object Source;

		public AttributeModifier Convert(float value)
		{
			return new AttributeModifier(value, Type, Order, Source);
		}
	}
}
