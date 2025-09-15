using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Abstract;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.UserSelect;

public sealed class UserSelectBuilder : ICssBuilder
{
    private readonly List<UserSelectRule> _rules = [];

    internal UserSelectBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new UserSelectRule(value, breakpoint));
    }

    internal UserSelectBuilder(List<UserSelectRule> rules)
    {
        _rules.AddRange(rules);
    }

    public UserSelectBuilder None => Chain("none");
    public UserSelectBuilder Auto => Chain("auto");
    public UserSelectBuilder All => Chain("all");

    public UserSelectBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public UserSelectBuilder OnMobile => ChainBp(Breakpoint.Mobile);
    public UserSelectBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public UserSelectBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public UserSelectBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public UserSelectBuilder OnWideScreen => ChainBp(Breakpoint.ExtraExtraLarge);

    private UserSelectBuilder Chain(string value)
    {
        var list = new List<UserSelectRule>(_rules) { new UserSelectRule(value, null) };
        return new UserSelectBuilder(list);
    }

    private UserSelectBuilder ChainBp(Breakpoint bp)
    {
        UserSelectRule last = _rules.LastOrDefault();
        if (last.Value == null)
            return new UserSelectBuilder("auto", bp);

        var list = new List<UserSelectRule>(_rules);
        list[list.Count - 1] = new UserSelectRule(last.Value, bp);
        return new UserSelectBuilder(list);
    }

    public string ToClass()
    {
        if (_rules.Count == 0) return string.Empty;
        var classes = new List<string>(_rules.Count);
        foreach (UserSelectRule rule in _rules)
        {
            string cls = rule.Value switch
            {
                "none" => "user-select-none",
                "auto" => "user-select-auto",
                "all" => "user-select-all",
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
        foreach (UserSelectRule rule in _rules)
        {
            string? css = $"user-select: {rule.Value}";
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

 
