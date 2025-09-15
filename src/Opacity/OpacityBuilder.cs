using System.Collections.Generic;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Abstract;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Opacity;

public sealed class OpacityBuilder : ICssBuilder
{
    private readonly List<OpacityRule> _rules = [];

    internal OpacityBuilder(int value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new OpacityRule(value, breakpoint));
    }

    internal OpacityBuilder(List<OpacityRule> rules)
    {
        _rules.AddRange(rules);
    }

    public OpacityBuilder V0 => Chain(0);
    public OpacityBuilder V25 => Chain(25);
    public OpacityBuilder V50 => Chain(50);
    public OpacityBuilder V75 => Chain(75);
    public OpacityBuilder V100 => Chain(100);

    public OpacityBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public OpacityBuilder OnMobile => ChainBp(Breakpoint.Mobile);
    public OpacityBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public OpacityBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public OpacityBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public OpacityBuilder OnWideScreen => ChainBp(Breakpoint.ExtraExtraLarge);

    private OpacityBuilder Chain(int value)
    {
        var list = new List<OpacityRule>(_rules) { new OpacityRule(value, null) };
        return new OpacityBuilder(list);
    }

    private OpacityBuilder ChainBp(Breakpoint bp)
    {
        if (_rules.Count == 0)
            return new OpacityBuilder(100, bp);

        var list = new List<OpacityRule>(_rules);
        OpacityRule last = list[list.Count - 1];
        list[list.Count - 1] = new OpacityRule(last.Value, bp);
        return new OpacityBuilder(list);
    }

    public string ToClass()
    {
        if (_rules.Count == 0) return string.Empty;
        var classes = new List<string>(_rules.Count);
        foreach (OpacityRule rule in _rules)
        {
            string cls = GetClass(rule.Value);
            if (cls.HasContent())
            {
                string bp = GetBp(rule.Breakpoint);
                string className = cls;
                if (bp.HasContent())
                {
                    int dashIndex = className.IndexOf('-'); // "opacity-50" -> insert after "opacity"
                    if (dashIndex > 0)
                        className = $"{className.Substring(0, dashIndex)}-{bp}{className.Substring(dashIndex)}"; // opacity-sm-50
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
        foreach (OpacityRule rule in _rules)
        {
            string? css = GetStyle(rule.Value);
            if (css.HasContent()) styles.Add(css);
        }
        return string.Join("; ", styles);
    }

    private static string GetClass(int value)
    {
        return value switch
        {
            0 => "opacity-0",
            25 => "opacity-25",
            50 => "opacity-50",
            75 => "opacity-75",
            100 => "opacity-100",
            _ => string.Empty
        };
    }

    private static string? GetStyle(int value)
    {
        return value switch
        {
            0 => "opacity: 0",
            25 => "opacity: 0.25",
            50 => "opacity: 0.5",
            75 => "opacity: 0.75",
            100 => "opacity: 1",
            _ => null
        };
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

 
