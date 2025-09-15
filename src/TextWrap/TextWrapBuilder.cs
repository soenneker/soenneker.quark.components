using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Abstract;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.TextWrap;

public sealed class TextWrapBuilder : ICssBuilder
{
    private readonly List<TextWrapRule> _rules = [];

    internal TextWrapBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new TextWrapRule(value, breakpoint));
    }

    internal TextWrapBuilder(List<TextWrapRule> rules)
    {
        _rules.AddRange(rules);
    }

    public TextWrapBuilder Wrap => Chain("wrap");
    public TextWrapBuilder NoWrap => Chain("nowrap");

    public TextWrapBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public TextWrapBuilder OnMobile => ChainBp(Breakpoint.Mobile);
    public TextWrapBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public TextWrapBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public TextWrapBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public TextWrapBuilder OnWideScreen => ChainBp(Breakpoint.ExtraExtraLarge);

    private TextWrapBuilder Chain(string value)
    {
        var list = new List<TextWrapRule>(_rules) { new TextWrapRule(value, null) };
        return new TextWrapBuilder(list);
    }

    private TextWrapBuilder ChainBp(Breakpoint bp)
    {
        TextWrapRule last = _rules.LastOrDefault();
        if (last.Value == null)
            return new TextWrapBuilder("wrap", bp);

        var list = new List<TextWrapRule>(_rules);
        list[list.Count - 1] = new TextWrapRule(last.Value, bp);
        return new TextWrapBuilder(list);
    }

    public string ToClass()
    {
        if (_rules.Count == 0) return string.Empty;
        var classes = new List<string>(_rules.Count);
        foreach (TextWrapRule rule in _rules)
        {
            string cls = rule.Value switch
            {
                "wrap" => "text-wrap",
                "nowrap" => "text-nowrap",
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
        foreach (TextWrapRule rule in _rules)
        {
            string? css = rule.Value switch
            {
                "wrap" => "text-wrap: wrap",
                "nowrap" => "text-wrap: nowrap",
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

 
