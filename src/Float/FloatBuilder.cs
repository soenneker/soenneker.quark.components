using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Abstract;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Float;

/// <summary>
/// Simplified float builder with fluent API for chaining float rules.
/// </summary>
public sealed class FloatBuilder : ICssBuilder
{
    private readonly List<FloatRule> _rules = [];

    internal FloatBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new FloatRule(value, breakpoint));
    }

    internal FloatBuilder(List<FloatRule> rules)
    {
        _rules.AddRange(rules);
    }

    public FloatBuilder None => ChainWithValue("none");
    public FloatBuilder Start => ChainWithValue("start");
    public FloatBuilder End => ChainWithValue("end");

    public FloatBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);
    public FloatBuilder OnMobile => ChainWithBreakpoint(Breakpoint.Mobile);
    public FloatBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);
    public FloatBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);
    public FloatBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);
    public FloatBuilder OnWideScreen => ChainWithBreakpoint(Breakpoint.ExtraExtraLarge);

    private FloatBuilder ChainWithValue(string value)
    {
        var newRules = new List<FloatRule>(_rules) { new FloatRule(value, null) };
        return new FloatBuilder(newRules);
    }

    private FloatBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        FloatRule? lastRule = _rules.LastOrDefault();
        if (lastRule == null)
            return new FloatBuilder("none", breakpoint);

        var newRules = new List<FloatRule>(_rules);
        newRules[newRules.Count - 1] = new FloatRule(lastRule.Value, breakpoint);
        return new FloatBuilder(newRules);
    }

    public string ToClass()
    {
        if (_rules.Count == 0)
            return string.Empty;

        var classes = new List<string>(_rules.Count);

        foreach (FloatRule rule in _rules)
        {
            string floatClass = GetFloatClass(rule.Value);
            string breakpointClass = GetBreakpointClass(rule.Breakpoint);

            if (floatClass.HasContent())
            {
                string className = floatClass;
                if (breakpointClass.HasContent())
                {
                    int dashIndex = className.IndexOf('-');
                    if (dashIndex > 0)
                        className = $"{className.Substring(0, dashIndex)}-{breakpointClass}{className.Substring(dashIndex)}";
                    else
                        className = $"{breakpointClass}-{className}";
                }

                classes.Add(className);
            }
        }

        return string.Join(" ", classes);
    }

    public string ToStyle()
    {
        if (_rules.Count == 0)
            return string.Empty;

        var styles = new List<string>(_rules.Count);

        foreach (FloatRule rule in _rules)
        {
            string? css = GetFloatStyle(rule.Value);
            if (css.HasContent())
                styles.Add(css);
        }

        return string.Join("; ", styles);
    }

    private static string GetFloatClass(string value)
    {
        return value switch
        {
            "start" => "float-start",
            "end" => "float-end",
            "none" => "float-none",
            _ => string.Empty
        };
    }

    private static string? GetFloatStyle(string value)
    {
        return value switch
        {
            "start" => "float: inline-start",
            "end" => "float: inline-end",
            "none" => "float: none",
            _ => null
        };
    }

    private static string GetBreakpointClass(Breakpoint? breakpoint)
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
