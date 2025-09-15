using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Abstract;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.ZIndex;

public sealed class ZIndexBuilder : ICssBuilder
{
    private readonly List<ZIndexRule> _rules = [];

    internal ZIndexBuilder(int value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new ZIndexRule(value, breakpoint));
    }

    internal ZIndexBuilder(List<ZIndexRule> rules)
    {
        _rules.AddRange(rules);
    }

    public ZIndexBuilder N1 => Chain(-1);
    public ZIndexBuilder Z0 => Chain(0);
    public ZIndexBuilder Z1 => Chain(1);
    public ZIndexBuilder Z2 => Chain(2);
    public ZIndexBuilder Z3 => Chain(3);

    public ZIndexBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public ZIndexBuilder OnMobile => ChainBp(Breakpoint.Mobile);
    public ZIndexBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public ZIndexBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public ZIndexBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public ZIndexBuilder OnWideScreen => ChainBp(Breakpoint.ExtraExtraLarge);

    private ZIndexBuilder Chain(int value)
    {
        var list = new List<ZIndexRule>(_rules) { new ZIndexRule(value, null) };
        return new ZIndexBuilder(list);
    }

    private ZIndexBuilder ChainBp(Breakpoint bp)
    {
        ZIndexRule last = _rules.LastOrDefault();
        if (last.Value == 0)
            return new ZIndexBuilder(0, bp);

        var list = new List<ZIndexRule>(_rules);
        list[list.Count - 1] = new ZIndexRule(last.Value, bp);
        return new ZIndexBuilder(list);
    }

    public string ToClass()
    {
        if (_rules.Count == 0) return string.Empty;
        var classes = new List<string>(_rules.Count);
        foreach (ZIndexRule rule in _rules)
        {
            string cls = rule.Value switch
            {
                -1 => "z-n1",
                0 => "z-0",
                1 => "z-1",
                2 => "z-2",
                3 => "z-3",
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
        foreach (ZIndexRule rule in _rules)
        {
            styles.Add($"z-index: {rule.Value}");
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

 
