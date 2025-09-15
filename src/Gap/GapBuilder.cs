using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Abstract;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Gap;

/// <summary>
/// Simplified gap builder with fluent API for chaining gap rules.
/// </summary>
public sealed class GapBuilder : ICssBuilder
{
    private readonly List<GapRule> _rules = [];

    internal GapBuilder(int size, Breakpoint? breakpoint = null)
    {
        if (size >= 0)
            _rules.Add(new GapRule(size, breakpoint));
    }

    internal GapBuilder(List<GapRule> rules)
    {
        _rules.AddRange(rules);
    }

    /// <summary>
    /// Chain with a new size for the next rule.
    /// </summary>
    public GapBuilder S0 => ChainWithSize(0);

    /// <summary>
    /// Chain with a new size for the next rule.
    /// </summary>
    public GapBuilder S1 => ChainWithSize(1);

    /// <summary>
    /// Chain with a new size for the next rule.
    /// </summary>
    public GapBuilder S2 => ChainWithSize(2);

    /// <summary>
    /// Chain with a new size for the next rule.
    /// </summary>
    public GapBuilder S3 => ChainWithSize(3);

    /// <summary>
    /// Chain with a new size for the next rule.
    /// </summary>
    public GapBuilder S4 => ChainWithSize(4);

    /// <summary>
    /// Chain with a new size for the next rule.
    /// </summary>
    public GapBuilder S5 => ChainWithSize(5);

    /// <summary>
    /// Apply on phone devices (portrait phones, less than 576px).
    /// </summary>
    public GapBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);

    /// <summary>
    /// Apply on mobile devices (landscape phones, 576px and up).
    /// </summary>
    public GapBuilder OnMobile => ChainWithBreakpoint(Breakpoint.Mobile);

    /// <summary>
    /// Apply on tablet devices (tablets, 768px and up).
    /// </summary>
    public GapBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);

    /// <summary>
    /// Apply on laptop devices (laptops, 992px and up).
    /// </summary>
    public GapBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);

    /// <summary>
    /// Apply on desktop devices (desktops, 1200px and up).
    /// </summary>
    public GapBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);

    /// <summary>
    /// Apply on wide screen devices (larger desktops, 1400px and up).
    /// </summary>
    public GapBuilder OnWideScreen => ChainWithBreakpoint(Breakpoint.ExtraExtraLarge);

    private GapBuilder ChainWithSize(int size)
    {
        var newRules = new List<GapRule>(_rules) { new GapRule(size, null) };
        return new GapBuilder(newRules);
    }

    private GapBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        GapRule? lastRule = _rules.LastOrDefault();
        if (lastRule == null)
            return new GapBuilder(0, breakpoint);

        var newRules = new List<GapRule>(_rules);
        // Update the last rule with the new breakpoint
        newRules[newRules.Count - 1] = new GapRule(lastRule.Size, breakpoint);
        return new GapBuilder(newRules);
    }

    /// <summary>
    /// Gets the CSS class string for the current configuration.
    /// </summary>
    public string ToClass()
    {
        if (_rules.Count == 0)
            return string.Empty;

        var classes = new List<string>(_rules.Count);

        foreach (GapRule rule in _rules)
        {
            string sizeClass = GetSizeClass(rule.Size);
            string breakpointClass = GetBreakpointClass(rule.Breakpoint);

            if (sizeClass.HasContent())
            {
                string className = sizeClass;
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

    /// <summary>
    /// Gets the CSS style string for the current configuration.
    /// </summary>
    public string ToStyle()
    {
        if (_rules.Count == 0)
            return string.Empty;

        var styles = new List<string>(_rules.Count);

        foreach (GapRule rule in _rules)
        {
            string? sizeValue = GetSizeValue(rule.Size);
            if (sizeValue == null) continue;

            styles.Add($"gap: {sizeValue}");
        }

        return string.Join("; ", styles);
    }

    private static string GetSizeClass(int size)
    {
        return size switch
        {
            0 => "gap-0",
            1 => "gap-1",
            2 => "gap-2",
            3 => "gap-3",
            4 => "gap-4",
            5 => "gap-5",
            _ => string.Empty
        };
    }

    private static string? GetSizeValue(int size)
    {
        return size switch
        {
            0 => "0",
            1 => "0.25rem",
            2 => "0.5rem",
            3 => "1rem",
            4 => "1.5rem",
            5 => "3rem",
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
