using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Abstract;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.TextDecoration;

public sealed class TextDecorationBuilder : ICssBuilder
{
    private readonly List<TextDecorationRule> _rules = [];

    internal TextDecorationBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new TextDecorationRule(value, breakpoint));
    }

    internal TextDecorationBuilder(List<TextDecorationRule> rules)
    {
        _rules.AddRange(rules);
    }

    public TextDecorationBuilder None => Chain("none");
    public TextDecorationBuilder Underline => Chain("underline");
    public TextDecorationBuilder LineThrough => Chain("line-through");

    public TextDecorationBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public TextDecorationBuilder OnMobile => ChainBp(Breakpoint.Mobile);
    public TextDecorationBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public TextDecorationBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public TextDecorationBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public TextDecorationBuilder OnWideScreen => ChainBp(Breakpoint.ExtraExtraLarge);

    private TextDecorationBuilder Chain(string value)
    {
        var list = new List<TextDecorationRule>(_rules) { new TextDecorationRule(value, null) };
        return new TextDecorationBuilder(list);
    }

    private TextDecorationBuilder ChainBp(Breakpoint bp)
    {
        TextDecorationRule last = _rules.LastOrDefault();
        if (last.Value == null)
            return new TextDecorationBuilder("none", bp);

        var list = new List<TextDecorationRule>(_rules);
        list[list.Count - 1] = new TextDecorationRule(last.Value, bp);
        return new TextDecorationBuilder(list);
    }

    public string ToClass()
    {
        if (_rules.Count == 0) return string.Empty;
        var classes = new List<string>(_rules.Count);
        foreach (TextDecorationRule rule in _rules)
        {
            string cls = rule.Value switch
            {
                "none" => "text-decoration-none",
                "underline" => "text-decoration-underline",
                "line-through" => "text-decoration-line-through",
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
        foreach (TextDecorationRule rule in _rules)
        {
            string? css = rule.Value switch
            {
                "none" => "text-decoration-line: none",
                "underline" => "text-decoration-line: underline",
                "line-through" => "text-decoration-line: line-through",
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

 
