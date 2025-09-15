using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Abstract;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Visibility;

public sealed class VisibilityBuilder : ICssBuilder
{
    private readonly List<VisibilityRule> _rules = [];

    internal VisibilityBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new VisibilityRule(value, breakpoint));
    }

    internal VisibilityBuilder(List<VisibilityRule> rules)
    {
        _rules.AddRange(rules);
    }

    public VisibilityBuilder Visible => Chain("visible");
    public VisibilityBuilder Invisible => Chain("invisible");

    public VisibilityBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public VisibilityBuilder OnMobile => ChainBp(Breakpoint.Mobile);
    public VisibilityBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public VisibilityBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public VisibilityBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public VisibilityBuilder OnWideScreen => ChainBp(Breakpoint.ExtraExtraLarge);

    private VisibilityBuilder Chain(string value)
    {
        var list = new List<VisibilityRule>(_rules) { new VisibilityRule(value, null) };
        return new VisibilityBuilder(list);
    }

    private VisibilityBuilder ChainBp(Breakpoint bp)
    {
        VisibilityRule last = _rules.LastOrDefault();
        if (last.Value == null)
            return new VisibilityBuilder("visible", bp);

        var list = new List<VisibilityRule>(_rules);
        list[list.Count - 1] = new VisibilityRule(last.Value, bp);
        return new VisibilityBuilder(list);
    }

    public string ToClass()
    {
        if (_rules.Count == 0) return string.Empty;
        var classes = new List<string>(_rules.Count);
        foreach (VisibilityRule rule in _rules)
        {
            string cls = rule.Value switch
            {
                "invisible" => "invisible",
                "visible" => "visible",
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
        foreach (VisibilityRule rule in _rules)
        {
            string? css = rule.Value switch
            {
                "invisible" => "visibility: hidden",
                "visible" => "visibility: visible",
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

 
