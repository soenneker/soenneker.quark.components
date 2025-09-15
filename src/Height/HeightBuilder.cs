using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Common;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Height;

/// <summary>
/// Simplified height builder with fluent API for chaining height rules.
/// </summary>
public sealed class HeightBuilder : ICssBuilder
{
    private readonly List<HeightRule> _rules = [];

    internal HeightBuilder(string size, Breakpoint? breakpoint = null)
    {
        _rules.Add(new HeightRule(size, breakpoint));
    }

    internal HeightBuilder(List<HeightRule> rules)
    {
        _rules.AddRange(rules);
    }

    /// <summary>
    /// Chain with 25% height for the next rule.
    /// </summary>
    public HeightBuilder P25 => ChainWithSize("25");

    /// <summary>
    /// Chain with 50% height for the next rule.
    /// </summary>
    public HeightBuilder P50 => ChainWithSize("50");

    /// <summary>
    /// Chain with 75% height for the next rule.
    /// </summary>
    public HeightBuilder P75 => ChainWithSize("75");

    /// <summary>
    /// Chain with 100% height for the next rule.
    /// </summary>
    public HeightBuilder P100 => ChainWithSize("100");

    /// <summary>
    /// Chain with auto height for the next rule.
    /// </summary>
    public HeightBuilder Auto => ChainWithSize("auto");

    /// <summary>
    /// Apply on phone devices (portrait phones, less than 576px).
    /// </summary>
    public HeightBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);

    /// <summary>
    /// Apply on mobile devices (landscape phones, 576px and up).
    /// </summary>
    public HeightBuilder OnMobile => ChainWithBreakpoint(Breakpoint.Mobile);

    /// <summary>
    /// Apply on tablet devices (tablets, 768px and up).
    /// </summary>
    public HeightBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);

    /// <summary>
    /// Apply on laptop devices (laptops, 992px and up).
    /// </summary>
    public HeightBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);

    /// <summary>
    /// Apply on desktop devices (desktops, 1200px and up).
    /// </summary>
    public HeightBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);

    /// <summary>
    /// Apply on wide screen devices (larger desktops, 1400px and up).
    /// </summary>
    public HeightBuilder OnWideScreen => ChainWithBreakpoint(Breakpoint.ExtraExtraLarge);

    private HeightBuilder ChainWithSize(string size)
    {
        var newRules = new List<HeightRule>(_rules) { new HeightRule(size, null) };
        return new HeightBuilder(newRules);
    }

    private HeightBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        HeightRule? lastRule = _rules.LastOrDefault();
        if (lastRule == null)
            return new HeightBuilder("100", breakpoint);

        var newRules = new List<HeightRule>(_rules);
        // Update the last rule with the new breakpoint
        newRules[newRules.Count - 1] = new HeightRule(lastRule.Size, breakpoint);
        return new HeightBuilder(newRules);
    }

    /// <summary>
    /// Gets the CSS class string for the current configuration.
    /// </summary>
    public string ToClass()
    {
        if (_rules.Count == 0)
            return string.Empty;

        var classes = new List<string>(_rules.Count);

        foreach (HeightRule rule in _rules)
        {
            string heightClass = GetHeightClass(rule.Size);
            string breakpointClass = GetBreakpointClass(rule.Breakpoint);

            if (heightClass.HasContent())
            {
                string className = heightClass;
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

        foreach (HeightRule rule in _rules)
        {
            string? heightValue = GetHeightValue(rule.Size);
            if (heightValue == null) continue;

            styles.Add($"height: {heightValue}");
        }

        return string.Join("; ", styles);
    }

    private static string GetHeightClass(string size)
    {
        return size switch
        {
            "25" => "h-25",
            "50" => "h-50",
            "75" => "h-75",
            "100" => "h-100",
            "auto" => "h-auto",
            _ => string.Empty
        };
    }

    private static string? GetHeightValue(string size)
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


