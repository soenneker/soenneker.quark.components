using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Abstract;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.TextTransform;

public sealed class TextTransformBuilder : ICssBuilder
{
    private readonly List<TextTransformRule> _rules = [];

    internal TextTransformBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new TextTransformRule(value, breakpoint));
    }

    internal TextTransformBuilder(List<TextTransformRule> rules)
    {
        _rules.AddRange(rules);
    }

    public TextTransformBuilder Lowercase => Chain("lowercase");
    public TextTransformBuilder Uppercase => Chain("uppercase");
    public TextTransformBuilder Capitalize => Chain("capitalize");

    public TextTransformBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public TextTransformBuilder OnMobile => ChainBp(Breakpoint.Mobile);
    public TextTransformBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public TextTransformBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public TextTransformBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public TextTransformBuilder OnWideScreen => ChainBp(Breakpoint.ExtraExtraLarge);

    private TextTransformBuilder Chain(string value)
    {
        var list = new List<TextTransformRule>(_rules) { new TextTransformRule(value, null) };
        return new TextTransformBuilder(list);
    }

    private TextTransformBuilder ChainBp(Breakpoint bp)
    {
        TextTransformRule last = _rules.LastOrDefault();
        if (last.Value == null)
            return new TextTransformBuilder("lowercase", bp);

        var list = new List<TextTransformRule>(_rules);
        list[list.Count - 1] = new TextTransformRule(last.Value, bp);
        return new TextTransformBuilder(list);
    }

    public string ToClass()
    {
        if (_rules.Count == 0) return string.Empty;
        var classes = new List<string>(_rules.Count);
        foreach (TextTransformRule rule in _rules)
        {
            string cls = rule.Value switch
            {
                "lowercase" => "text-lowercase",
                "uppercase" => "text-uppercase",
                "capitalize" => "text-capitalize",
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
        foreach (TextTransformRule rule in _rules)
        {
            string? css = $"text-transform: {rule.Value}";
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

 
