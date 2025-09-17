using Soenneker.Quark.Enums.GlobalKeywords;

namespace Soenneker.Quark.Components.FontWeight;

public static class FontWeight
{
    public static FontWeightBuilder Light => new(Soenneker.Quark.Enums.FontWeights.FontWeight.LightValue);
    public static FontWeightBuilder Normal => new(Soenneker.Quark.Enums.FontWeights.FontWeight.NormalValue);
    public static FontWeightBuilder Medium => new(Soenneker.Quark.Enums.FontWeights.FontWeight.MediumValue);
    public static FontWeightBuilder Semibold => new(Soenneker.Quark.Enums.FontWeights.FontWeight.SemiboldValue);
    public static FontWeightBuilder Bold => new(Soenneker.Quark.Enums.FontWeights.FontWeight.BoldValue);
    public static FontWeightBuilder Bolder => new(Soenneker.Quark.Enums.FontWeights.FontWeight.BolderValue);
    public static FontWeightBuilder Inherit => new(GlobalKeyword.InheritValue);
    public static FontWeightBuilder Initial => new(GlobalKeyword.InitialValue);
    public static FontWeightBuilder Revert => new(GlobalKeyword.RevertValue);
    public static FontWeightBuilder RevertLayer => new(GlobalKeyword.RevertLayerValue);
    public static FontWeightBuilder Unset => new(GlobalKeyword.UnsetValue);
}