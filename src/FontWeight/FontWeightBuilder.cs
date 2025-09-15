using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Abstract;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.FontWeight;

public sealed class FontWeightBuilder : ICssBuilder
{
    private readonly List<FontWeightRule> _rules = [];

    internal FontWeightBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new FontWeightRule(value, breakpoint));
    }

    internal FontWeightBuilder(List<FontWeightRule> rules)
    {
        _rules.AddRange(rules);
    }

    public FontWeightBuilder Light => Chain("light");
    public FontWeightBuilder Normal => Chain("normal");
    public FontWeightBuilder Medium => Chain("medium");
    public FontWeightBuilder Semibold => Chain("semibold");
    public FontWeightBuilder Bold => Chain("bold");
    public FontWeightBuilder Bolder => Chain("bolder");

    public FontWeightBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public FontWeightBuilder OnMobile => ChainBp(Breakpoint.Mobile);
    public FontWeightBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public FontWeightBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public FontWeightBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public FontWeightBuilder OnWideScreen => ChainBp(Breakpoint.ExtraExtraLarge);

    private FontWeightBuilder Chain(string value)
    {
        var list = new List<FontWeightRule>(_rules) { new FontWeightRule(value, null) };
        return new FontWeightBuilder(list);
    }

    private FontWeightBuilder ChainBp(Breakpoint bp)
    {
        FontWeightRule last = _rules.LastOrDefault();
        if (last.Value == null)
            return new FontWeightBuilder("normal", bp);

        var list = new List<FontWeightRule>(_rules);
        list[list.Count - 1] = new FontWeightRule(last.Value, bp);
        return new FontWeightBuilder(list);
    }

    public string ToClass()
    {
        if (_rules.Count == 0) return string.Empty;
        var classes = new List<string>(_rules.Count);
        foreach (FontWeightRule rule in _rules)
        {
            string cls = rule.Value switch
            {
                "light" => "fw-light",
                "normal" => "fw-normal",
                "medium" => "fw-medium",
                "semibold" => "fw-semibold",
                "bold" => "fw-bold",
                "bolder" => "fw-bolder",
                _ => string.Empty
            };
            if (cls.HasContent())
            {
                string bp = GetBp(rule.Breakpoint);
                string className = cls;
                if (bp.HasContent())
                {
                    int dashIndex = className.IndexOf('-');
                    if (dashIndex > 0)
                        className = $"{className.Substring(0, dashIndex)}-{bp}{className.Substring(dashIndex)}";
                    else
                        className = $"{bp}-{className}";
                }

                classes.Add(className);
            }
        }

        return string.Join(" ", classes);
    }

    public string ToStyle()
    {
        if (_rules.Count == 0) return string.Empty;
        var styles = new List<string>(_rules.Count);
        foreach (FontWeightRule rule in _rules)
        {
            string? css = rule.Value switch
            {
                "light" => "font-weight: 300",
                "normal" => "font-weight: 400",
                "medium" => "font-weight: 500",
                "semibold" => "font-weight: 600",
                "bold" => "font-weight: 700",
                "bolder" => "font-weight: bolder",
                _ => null
            };
            if (css.HasContent()) styles.Add(css);
        }

        return string.Join("; ", styles);
    }

    private static string GetBp(Breakpoint? breakpoint)
    {
        if (breakpoint == null) return string.Empty;
        switch (breakpoint)
        {
            case Breakpoint.PhoneValue:
            case Breakpoint.ExtraSmallValue:
                return string.Empty;
            case Breakpoint.MobileValue:
            case Breakpoint.SmallValue:
                return "sm";
            case Breakpoint.TabletValue:
            case Breakpoint.MediumValue:
                return "md";
            case Breakpoint.LaptopValue:
            case Breakpoint.LargeValue:
                return "lg";
            case Breakpoint.DesktopValue:
            case Breakpoint.ExtraLargeValue:
                return "xl";
            case Breakpoint.ExtraExtraLargeValue:
                return "xxl";
            default:
                return string.Empty;
        }
    }
}