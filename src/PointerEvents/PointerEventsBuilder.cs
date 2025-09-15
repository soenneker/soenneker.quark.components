using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Abstract;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.PointerEvents;

public sealed class PointerEventsBuilder : ICssBuilder
{
    private readonly List<PointerEventsRule> _rules = [];

    internal PointerEventsBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new PointerEventsRule(value, breakpoint));
    }

    internal PointerEventsBuilder(List<PointerEventsRule> rules)
    {
        _rules.AddRange(rules);
    }

    public PointerEventsBuilder None => Chain("none");
    public PointerEventsBuilder Auto => Chain("auto");

    public PointerEventsBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public PointerEventsBuilder OnMobile => ChainBp(Breakpoint.Mobile);
    public PointerEventsBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public PointerEventsBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public PointerEventsBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public PointerEventsBuilder OnWideScreen => ChainBp(Breakpoint.ExtraExtraLarge);

    private PointerEventsBuilder Chain(string value)
    {
        var list = new List<PointerEventsRule>(_rules) { new PointerEventsRule(value, null) };
        return new PointerEventsBuilder(list);
    }

    private PointerEventsBuilder ChainBp(Breakpoint bp)
    {
        PointerEventsRule last = _rules.LastOrDefault();
        if (last.Value == null)
            return new PointerEventsBuilder("auto", bp);

        var list = new List<PointerEventsRule>(_rules);
        list[list.Count - 1] = new PointerEventsRule(last.Value, bp);
        return new PointerEventsBuilder(list);
    }

    public string ToClass()
    {
        if (_rules.Count == 0) return string.Empty;
        var classes = new List<string>(_rules.Count);
        foreach (PointerEventsRule rule in _rules)
        {
            string cls = rule.Value switch
            {
                "none" => "pe-none",
                "auto" => "pe-auto",
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
        foreach (PointerEventsRule rule in _rules)
        {
            string? css = rule.Value switch
            {
                "none" => "pointer-events: none",
                "auto" => "pointer-events: auto",
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

 
