using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Common;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Width;

/// <summary>
/// Simplified width builder with fluent API for chaining width rules.
/// </summary>
public sealed class WidthBuilder : ICssBuilder
{
    private readonly List<WidthRule> _rules = [];

    internal WidthBuilder(string size, Breakpoint? breakpoint = null)
    {
        _rules.Add(new WidthRule(size, breakpoint));
    }

    internal WidthBuilder(List<WidthRule> rules)
    {
        _rules.AddRange(rules);
    }

    /// <summary>
    /// Chain with 25% width for the next rule.
    /// </summary>
    public WidthBuilder P25 => ChainWithSize("25");

    /// <summary>
    /// Chain with 50% width for the next rule.
    /// </summary>
    public WidthBuilder P50 => ChainWithSize("50");

    /// <summary>
    /// Chain with 75% width for the next rule.
    /// </summary>
    public WidthBuilder P75 => ChainWithSize("75");

    /// <summary>
    /// Chain with 100% width for the next rule.
    /// </summary>
    public WidthBuilder P100 => ChainWithSize("100");

    /// <summary>
    /// Chain with auto width for the next rule.
    /// </summary>
    public WidthBuilder Auto => ChainWithSize("auto");

    /// <summary>
    /// Apply on phone devices (portrait phones, less than 576px).
    /// </summary>
    public WidthBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);

    /// <summary>
    /// Apply on mobile devices (landscape phones, 576px and up).
    /// </summary>
    public WidthBuilder OnMobile => ChainWithBreakpoint(Breakpoint.Mobile);

    /// <summary>
    /// Apply on tablet devices (tablets, 768px and up).
    /// </summary>
    public WidthBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);

    /// <summary>
    /// Apply on laptop devices (laptops, 992px and up).
    /// </summary>
    public WidthBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);

    /// <summary>
    /// Apply on desktop devices (desktops, 1200px and up).
    /// </summary>
    public WidthBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);

    /// <summary>
    /// Apply on wide screen devices (larger desktops, 1400px and up).
    /// </summary>
    public WidthBuilder OnWideScreen => ChainWithBreakpoint(Breakpoint.ExtraExtraLarge);

    private WidthBuilder ChainWithSize(string size)
    {
        var newRules = new List<WidthRule>(_rules) { new WidthRule(size, null) };
        return new WidthBuilder(newRules);
    }

    private WidthBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        WidthRule? lastRule = _rules.LastOrDefault();
        if (lastRule == null)
            return new WidthBuilder("100", breakpoint);

        var newRules = new List<WidthRule>(_rules);
        // Update the last rule with the new breakpoint
        newRules[newRules.Count - 1] = new WidthRule(lastRule.Size, breakpoint);
        return new WidthBuilder(newRules);
    }

    /// <summary>
    /// Gets the CSS class string for the current configuration.
    /// </summary>
    public string ToClass()
    {
        if (_rules.Count == 0)
            return string.Empty;

        var classes = new List<string>(_rules.Count);

        foreach (WidthRule rule in _rules)
        {
            string widthClass = GetWidthClass(rule.Size);
            string breakpointClass = GetBreakpointClass(rule.Breakpoint);

            if (widthClass.HasContent())
            {
                string className = widthClass;
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

        foreach (WidthRule rule in _rules)
        {
            string? widthValue = GetWidthValue(rule.Size);
            if (widthValue == null) continue;

            styles.Add($"width: {widthValue}");
        }

        return string.Join("; ", styles);
    }

    private static string GetWidthClass(string size)
    {
        return size switch
        {
            "25" => "w-25",
            "50" => "w-50",
            "75" => "w-75",
            "100" => "w-100",
            "auto" => "w-auto",
            _ => string.Empty
        };
    }

    private static string? GetWidthValue(string size)
    {
        return size switch
        {
            "25" => "25%",
            "50" => "50%",
            "75" => "75%",
            "100" => "100%",
            "auto" => "auto",
            _ => null
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


