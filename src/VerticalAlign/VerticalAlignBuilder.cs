using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Abstract;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.VerticalAlign;

public sealed class VerticalAlignBuilder : ICssBuilder
{
    private readonly List<VerticalAlignRule> _rules = [];

    internal VerticalAlignBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new VerticalAlignRule(value, breakpoint));
    }

    internal VerticalAlignBuilder(List<VerticalAlignRule> rules)
    {
        _rules.AddRange(rules);
    }

    public VerticalAlignBuilder Baseline => Chain("baseline");
    public VerticalAlignBuilder Top => Chain("top");
    public VerticalAlignBuilder Middle => Chain("middle");
    public VerticalAlignBuilder Bottom => Chain("bottom");
    public VerticalAlignBuilder TextTop => Chain("text-top");
    public VerticalAlignBuilder TextBottom => Chain("text-bottom");

    public VerticalAlignBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public VerticalAlignBuilder OnMobile => ChainBp(Breakpoint.Mobile);
    public VerticalAlignBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public VerticalAlignBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public VerticalAlignBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public VerticalAlignBuilder OnWideScreen => ChainBp(Breakpoint.ExtraExtraLarge);

    private VerticalAlignBuilder Chain(string value)
    {
        var list = new List<VerticalAlignRule>(_rules) { new VerticalAlignRule(value, null) };
        return new VerticalAlignBuilder(list);
    }

    private VerticalAlignBuilder ChainBp(Breakpoint bp)
    {
        VerticalAlignRule last = _rules.LastOrDefault();
        if (last.Value == null)
            return new VerticalAlignBuilder("baseline", bp);

        var list = new List<VerticalAlignRule>(_rules);
        list[list.Count - 1] = new VerticalAlignRule(last.Value, bp);
        return new VerticalAlignBuilder(list);
    }

    public string ToClass()
    {
        if (_rules.Count == 0) return string.Empty;
        var classes = new List<string>(_rules.Count);
        foreach (VerticalAlignRule rule in _rules)
        {
            string cls = rule.Value switch
            {
                "baseline" => "align-baseline",
                "top" => "align-top",
                "middle" => "align-middle",
                "bottom" => "align-bottom",
                "text-top" => "align-text-top",
                "text-bottom" => "align-text-bottom",
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
        foreach (VerticalAlignRule rule in _rules)
        {
            string? css = $"vertical-align: {rule.Value}";
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

 
