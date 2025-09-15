using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Abstract;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Shadow;

public sealed class ShadowBuilder : ICssBuilder
{
    private readonly List<ShadowRule> _rules = [];

    internal ShadowBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new ShadowRule(value, breakpoint));
    }

    internal ShadowBuilder(List<ShadowRule> rules)
    {
        _rules.AddRange(rules);
    }

    public ShadowBuilder None => Chain("none");
    public ShadowBuilder Base => Chain("base");
    public ShadowBuilder Sm => Chain("sm");
    public ShadowBuilder Lg => Chain("lg");

    public ShadowBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public ShadowBuilder OnMobile => ChainBp(Breakpoint.Mobile);
    public ShadowBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public ShadowBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public ShadowBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public ShadowBuilder OnWideScreen => ChainBp(Breakpoint.ExtraExtraLarge);

    private ShadowBuilder Chain(string value)
    {
        var list = new List<ShadowRule>(_rules) { new ShadowRule(value, null) };
        return new ShadowBuilder(list);
    }

    private ShadowBuilder ChainBp(Breakpoint bp)
    {
        ShadowRule last = _rules.LastOrDefault();
        if (last.Value == null)
            return new ShadowBuilder("base", bp);

        var list = new List<ShadowRule>(_rules);
        list[list.Count - 1] = new ShadowRule(last.Value, bp);
        return new ShadowBuilder(list);
    }

    public string ToClass()
    {
        if (_rules.Count == 0) return string.Empty;
        var classes = new List<string>(_rules.Count);
        foreach (ShadowRule rule in _rules)
        {
            string cls = rule.Value switch
            {
                "none" => "shadow-none",
                "base" => "shadow",
                "sm" => "shadow-sm",
                "lg" => "shadow-lg",
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
        // Shadow utilities are class-first; omit style mapping
        return string.Empty;
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

internal readonly record struct ShadowRule(string Value, Breakpoint? Breakpoint);
