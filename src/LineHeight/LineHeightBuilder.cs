using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Abstract;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.LineHeight;

public sealed class LineHeightBuilder : ICssBuilder
{
    private readonly List<LineHeightRule> _rules = [];

    internal LineHeightBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new LineHeightRule(value, breakpoint));
    }

    internal LineHeightBuilder(List<LineHeightRule> rules)
    {
        _rules.AddRange(rules);
    }

    public LineHeightBuilder L1 => Chain("1");
    public LineHeightBuilder Sm => Chain("sm");
    public LineHeightBuilder Base => Chain("base");
    public LineHeightBuilder Lg => Chain("lg");

    public LineHeightBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public LineHeightBuilder OnMobile => ChainBp(Breakpoint.Mobile);
    public LineHeightBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public LineHeightBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public LineHeightBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public LineHeightBuilder OnWideScreen => ChainBp(Breakpoint.ExtraExtraLarge);

    private LineHeightBuilder Chain(string value)
    {
        var list = new List<LineHeightRule>(_rules) { new LineHeightRule(value, null) };
        return new LineHeightBuilder(list);
    }

    private LineHeightBuilder ChainBp(Breakpoint bp)
    {
        LineHeightRule last = _rules.LastOrDefault();
        if (last.Value == null)
            return new LineHeightBuilder("base", bp);

        var list = new List<LineHeightRule>(_rules);
        list[list.Count - 1] = new LineHeightRule(last.Value, bp);
        return new LineHeightBuilder(list);
    }

    public string ToClass()
    {
        if (_rules.Count == 0) return string.Empty;
        var classes = new List<string>(_rules.Count);
        foreach (LineHeightRule rule in _rules)
        {
            string cls = rule.Value switch
            {
                "1" => "lh-1",
                "sm" => "lh-sm",
                "base" => "lh-base",
                "lg" => "lh-lg",
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
        foreach (LineHeightRule rule in _rules)
        {
            string? css = rule.Value switch
            {
                "1" => "line-height: 1",
                "sm" => "line-height: 1.25",
                "base" => "line-height: 1.5",
                "lg" => "line-height: 2",
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

 
