using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Abstract;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.PositionOffset;

public sealed class PositionOffsetBuilder : ICssBuilder
{
    private readonly List<PositionOffsetRule> _rules = [];

    internal PositionOffsetBuilder(string property, string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new PositionOffsetRule(property, value, breakpoint));
    }

    internal PositionOffsetBuilder(List<PositionOffsetRule> rules)
    {
        _rules.AddRange(rules);
    }

    public PositionOffsetBuilder Top0 => Chain("top", "0");
    public PositionOffsetBuilder Top50 => Chain("top", "50");
    public PositionOffsetBuilder Top100 => Chain("top", "100");

    public PositionOffsetBuilder Bottom0 => Chain("bottom", "0");
    public PositionOffsetBuilder Bottom50 => Chain("bottom", "50");
    public PositionOffsetBuilder Bottom100 => Chain("bottom", "100");

    public PositionOffsetBuilder Start0 => Chain("start", "0");
    public PositionOffsetBuilder Start50 => Chain("start", "50");
    public PositionOffsetBuilder Start100 => Chain("start", "100");

    public PositionOffsetBuilder End0 => Chain("end", "0");
    public PositionOffsetBuilder End50 => Chain("end", "50");
    public PositionOffsetBuilder End100 => Chain("end", "100");

    public PositionOffsetBuilder TranslateMiddle => Chain("translate", "middle");
    public PositionOffsetBuilder TranslateMiddleX => Chain("translate", "middle-x");
    public PositionOffsetBuilder TranslateMiddleY => Chain("translate", "middle-y");

    public PositionOffsetBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public PositionOffsetBuilder OnMobile => ChainBp(Breakpoint.Mobile);
    public PositionOffsetBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public PositionOffsetBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public PositionOffsetBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public PositionOffsetBuilder OnWideScreen => ChainBp(Breakpoint.ExtraExtraLarge);

    private PositionOffsetBuilder Chain(string property, string value)
    {
        var list = new List<PositionOffsetRule>(_rules) { new PositionOffsetRule(property, value, null) };
        return new PositionOffsetBuilder(list);
    }

    private PositionOffsetBuilder ChainBp(Breakpoint bp)
    {
        PositionOffsetRule last = _rules.LastOrDefault();
        if (last.Property == null)
            return new PositionOffsetBuilder("top", "0", bp);

        var list = new List<PositionOffsetRule>(_rules);
        list[list.Count - 1] = new PositionOffsetRule(last.Property, last.Value, bp);
        return new PositionOffsetBuilder(list);
    }

    public string ToClass()
    {
        if (_rules.Count == 0) return string.Empty;
        var classes = new List<string>(_rules.Count);
        foreach (PositionOffsetRule rule in _rules)
        {
            string cls = GetClass(rule.Property, rule.Value);
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
        foreach (PositionOffsetRule rule in _rules)
        {
            string? css = GetStyle(rule.Property, rule.Value);
            if (css.HasContent()) styles.Add(css);
        }
        return string.Join("; ", styles);
    }

    private static string GetClass(string property, string value)
    {
        if (property == "translate")
        {
            return value switch
            {
                "middle" => "translate-middle",
                "middle-x" => "translate-middle-x",
                "middle-y" => "translate-middle-y",
                _ => string.Empty
            };
        }

        string prefix = property switch
        {
            "top" => "top",
            "bottom" => "bottom",
            "start" => "start",
            "end" => "end",
            _ => string.Empty
        };
        if (prefix.Length == 0) return string.Empty;
        return $"{prefix}-{value}";
    }

    private static string? GetStyle(string property, string value)
    {
        if (property == "translate") return null; // class-only utility
        string cssProp = property switch
        {
            "top" => "top",
            "bottom" => "bottom",
            "start" => "inset-inline-start",
            "end" => "inset-inline-end",
            _ => string.Empty
        };
        if (cssProp.Length == 0) return null;
        string cssVal = value switch
        {
            "0" => "0",
            "50" => "50%",
            "100" => "100%",
            _ => string.Empty
        };
        if (cssVal.Length == 0) return null;
        return $"{cssProp}: {cssVal}";
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

internal readonly record struct PositionOffsetRule(string Property, string Value, Breakpoint? Breakpoint);
