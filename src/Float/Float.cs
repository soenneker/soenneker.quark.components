using Soenneker.Quark.Enums.GlobalKeywords;

namespace Soenneker.Quark.Components.Float;

public static class Float
{
	public static FloatBuilder None => new(Soenneker.Quark.Enums.Floats.Float.NoneValue);

	public static FloatBuilder Left => new(Soenneker.Quark.Enums.Floats.Float.LeftValue);

	public static FloatBuilder Right => new(Soenneker.Quark.Enums.Floats.Float.RightValue);

	public static FloatBuilder Inherit => new(GlobalKeyword.InheritValue);
	public static FloatBuilder Initial => new(GlobalKeyword.InitialValue);
	public static FloatBuilder Revert => new(GlobalKeyword.RevertValue);
	public static FloatBuilder RevertLayer => new(GlobalKeyword.RevertLayerValue);
	public static FloatBuilder Unset => new(GlobalKeyword.UnsetValue);
}