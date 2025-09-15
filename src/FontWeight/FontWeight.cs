namespace Soenneker.Quark.Components.FontWeight;

public static class FontWeight
{
    public static FontWeightBuilder Light => new("light");
    public static FontWeightBuilder Normal => new("normal");
    public static FontWeightBuilder Medium => new("medium");
    public static FontWeightBuilder Semibold => new("semibold");
    public static FontWeightBuilder Bold => new("bold");
    public static FontWeightBuilder Bolder => new("bolder");
}