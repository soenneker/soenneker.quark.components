using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Common;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Overflow;

/// <summary>
/// Simplified overflow builder with fluent API for chaining overflow rules.
/// </summary>
public sealed class OverflowBuilder : ICssBuilder
{
    private readonly List<OverflowRule> _rules = [];

    internal OverflowBuilder(string overflow, Breakpoint? breakpoint = null)
    {
        _rules.Add(new OverflowRule(overflow, breakpoint));
    }

    internal OverflowBuilder(List<OverflowRule> rules)
    {
        _rules.AddRange(rules);
    }

    /// <summary>
    /// Chain with overflow auto for the next rule.
    /// </summary>
    public OverflowBuilder Auto => ChainWithOverflow("auto");

    /// <summary>
    /// Chain with overflow hidden for the next rule.
    /// </summary>
    public OverflowBuilder Hidden => ChainWithOverflow("hidden");

    /// <summary>
    /// Chain with overflow visible for the next rule.
    /// </summary>
    public OverflowBuilder Visible => ChainWithOverflow("visible");

    /// <summary>
    /// Chain with overflow scroll for the next rule.
    /// </summary>
    public OverflowBuilder Scroll => ChainWithOverflow("scroll");

    /// <summary>
    /// Apply on phone devices (portrait phones, less than 576px).
    /// </summary>
    public OverflowBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);

    /// <summary>
    /// Apply on mobile devices (landscape phones, 576px and up).
    /// </summary>
    public OverflowBuilder OnMobile => ChainWithBreakpoint(Breakpoint.Mobile);

    /// <summary>
    /// Apply on tablet devices (tablets, 768px and up).
    /// </summary>
    public OverflowBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);

    /// <summary>
    /// Apply on laptop devices (laptops, 992px and up).
    /// </summary>
    public OverflowBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);

    /// <summary>
    /// Apply on desktop devices (desktops, 1200px and up).
    /// </summary>
    public OverflowBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);

    /// <summary>
    /// Apply on wide screen devices (larger desktops, 1400px and up).
    /// </summary>
    public OverflowBuilder OnWideScreen => ChainWithBreakpoint(Breakpoint.ExtraExtraLarge);

    private OverflowBuilder ChainWithOverflow(string overflow)
    {
        var newRules = new List<OverflowRule>(_rules) { new OverflowRule(overflow, null) };
        return new OverflowBuilder(newRules);
    }

    private OverflowBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        OverflowRule? lastRule = _rules.LastOrDefault();
        if (lastRule == null)
            return new OverflowBuilder("auto", breakpoint);

        var newRules = new List<OverflowRule>(_rules);
        // Update the last rule with the new breakpoint
        newRules[newRules.Count - 1] = new OverflowRule(lastRule.Overflow, breakpoint);
        return new OverflowBuilder(newRules);
    }

    /// <summary>
    /// Gets the CSS class string for the current configuration.
    /// </summary>
    public string ToClass()
    {
        if (_rules.Count == 0)
            return string.Empty;

        var classes = new List<string>(_rules.Count);

        foreach (OverflowRule rule in _rules)
        {
            string overflowClass = GetOverflowClass(rule.Overflow);
            string breakpointClass = GetBreakpointClass(rule.Breakpoint);

            if (overflowClass.HasContent())
            {
                string className = overflowClass;
                if (breakpointClass.HasContent())
                    className = $"{breakpointClass}-{className}";

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

        foreach (OverflowRule rule in _rules)
        {
            if (rule.Overflow.HasContent())
            {
                styles.Add($"overflow: {rule.Overflow}");
            }
        }

        return string.Join("; ", styles);
    }

    private static string GetOverflowClass(string overflow)
    {
        return overflow switch
        {
            "auto" => "overflow-auto",
            "hidden" => "overflow-hidden",
            "visible" => "overflow-visible",
            "scroll" => "overflow-scroll",
            _ => string.Empty
        };
    }

    private static string GetBreakpointClass(Breakpoint? breakpoint)
    {
        if (breakpoint == null) return string.Empty;

        return breakpoint.Value switch
        {
            "phone" or "xs" => string.Empty, // xs is default, no prefix needed
            "mobile" or "sm" => "sm",
            "tablet" or "md" => "md",
            "laptop" or "lg" => "lg",
            "desktop" or "xl" => "xl",
            "xxl" => "xxl",
            _ => string.Empty
        };
    }
}


