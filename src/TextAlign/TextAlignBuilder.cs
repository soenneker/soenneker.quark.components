using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Abstract;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.TextAlign;

public sealed class TextAlignBuilder : ICssBuilder
{
    private readonly List<TextAlignRule> _rules = [];

    internal TextAlignBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new TextAlignRule(value, breakpoint));
    }

    internal TextAlignBuilder(List<TextAlignRule> rules)
    {
        _rules.AddRange(rules);
    }

    public TextAlignBuilder Start => Chain("start");
    public TextAlignBuilder Center => Chain("center");
    public TextAlignBuilder End => Chain("end");

    public TextAlignBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public TextAlignBuilder OnMobile => ChainBp(Breakpoint.Mobile);
    public TextAlignBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public TextAlignBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public TextAlignBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public TextAlignBuilder OnWideScreen => ChainBp(Breakpoint.ExtraExtraLarge);

    private TextAlignBuilder Chain(string value)
    {
        var list = new List<TextAlignRule>(_rules) { new TextAlignRule(value, null) };
        return new TextAlignBuilder(list);
    }

    private TextAlignBuilder ChainBp(Breakpoint bp)
    {
        TextAlignRule last = _rules.LastOrDefault();
        if (last.Value == null)
            return new TextAlignBuilder("start", bp);

        var list = new List<TextAlignRule>(_rules);
        list[list.Count - 1] = new TextAlignRule(last.Value, bp);
        return new TextAlignBuilder(list);
    }

    public string ToClass()
    {
        if (_rules.Count == 0) return string.Empty;
        var classes = new List<string>(_rules.Count);
        foreach (TextAlignRule rule in _rules)
        {
            string cls = rule.Value switch
            {
                "start" => "text-start",
                "center" => "text-center",
                "end" => "text-end",
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
        foreach (TextAlignRule rule in _rules)
        {
            string? css = rule.Value switch
            {
                "start" => "text-align: start",
                "center" => "text-align: center",
                "end" => "text-align: end",
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

 
