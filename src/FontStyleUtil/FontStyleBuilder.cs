using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Abstract;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.FontStyleUtil;

public sealed class FontStyleBuilder : ICssBuilder
{
    private readonly List<FontStyleRule> _rules = [];

    internal FontStyleBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new FontStyleRule(value, breakpoint));
    }

    internal FontStyleBuilder(List<FontStyleRule> rules)
    {
        _rules.AddRange(rules);
    }

    public FontStyleBuilder Italic => Chain("italic");
    public FontStyleBuilder Normal => Chain("normal");

    public FontStyleBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public FontStyleBuilder OnMobile => ChainBp(Breakpoint.Mobile);
    public FontStyleBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public FontStyleBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public FontStyleBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public FontStyleBuilder OnWideScreen => ChainBp(Breakpoint.ExtraExtraLarge);

    private FontStyleBuilder Chain(string value)
    {
        var list = new List<FontStyleRule>(_rules) { new FontStyleRule(value, null) };
        return new FontStyleBuilder(list);
    }

    private FontStyleBuilder ChainBp(Breakpoint bp)
    {
        FontStyleRule last = _rules.LastOrDefault();
        if (last.Value == null)
            return new FontStyleBuilder("normal", bp);

        var list = new List<FontStyleRule>(_rules);
        list[list.Count - 1] = new FontStyleRule(last.Value, bp);
        return new FontStyleBuilder(list);
    }

    public string ToClass()
    {
        if (_rules.Count == 0) return string.Empty;
        var classes = new List<string>(_rules.Count);
        foreach (FontStyleRule rule in _rules)
        {
            string cls = rule.Value switch
            {
                "italic" => "fst-italic",
                "normal" => "fst-normal",
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
        foreach (FontStyleRule rule in _rules)
        {
            string? css = $"font-style: {rule.Value}";
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

 
