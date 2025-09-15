using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Abstract;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.TextBreak;

public sealed class TextBreakBuilder : ICssBuilder
{
    private readonly List<TextBreakRule> _rules = [];

    internal TextBreakBuilder(bool enabled, Breakpoint? breakpoint = null)
    {
        _rules.Add(new TextBreakRule(enabled, breakpoint));
    }

    internal TextBreakBuilder(List<TextBreakRule> rules)
    {
        _rules.AddRange(rules);
    }

    public TextBreakBuilder Enable => Chain(true);
    public TextBreakBuilder Disable => Chain(false);

    public TextBreakBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public TextBreakBuilder OnMobile => ChainBp(Breakpoint.Mobile);
    public TextBreakBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public TextBreakBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public TextBreakBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public TextBreakBuilder OnWideScreen => ChainBp(Breakpoint.ExtraExtraLarge);

    private TextBreakBuilder Chain(bool enabled)
    {
        var list = new List<TextBreakRule>(_rules) { new TextBreakRule(enabled, null) };
        return new TextBreakBuilder(list);
    }

    private TextBreakBuilder ChainBp(Breakpoint bp)
    {
        TextBreakRule last = _rules.LastOrDefault();
        if (!last.Enabled)
            return new TextBreakBuilder(true, bp);

        var list = new List<TextBreakRule>(_rules);
        list[list.Count - 1] = new TextBreakRule(last.Enabled, bp);
        return new TextBreakBuilder(list);
    }

    public string ToClass()
    {
        if (_rules.Count == 0) return string.Empty;
        var classes = new List<string>(_rules.Count);
        foreach (TextBreakRule rule in _rules)
        {
            string cls = rule.Enabled ? "text-break" : string.Empty;
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
        foreach (TextBreakRule rule in _rules)
        {
            if (rule.Enabled) styles.Add("word-wrap: break-word");
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

 
