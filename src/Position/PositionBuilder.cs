using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Common;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Position;

/// <summary>
/// Simplified position builder with fluent API for chaining position rules.
/// </summary>
public sealed class PositionBuilder : ICssBuilder
{
    private readonly List<PositionRule> _rules = [];

    internal PositionBuilder(string position, Breakpoint? breakpoint = null)
    {
        _rules.Add(new PositionRule(position, breakpoint));
    }

    internal PositionBuilder(List<PositionRule> rules)
    {
        _rules.AddRange(rules);
    }

    /// <summary>
    /// Chain with static positioning for the next rule.
    /// </summary>
    public PositionBuilder Static => ChainWithPosition("static");

    /// <summary>
    /// Chain with relative positioning for the next rule.
    /// </summary>
    public PositionBuilder Relative => ChainWithPosition("relative");

    /// <summary>
    /// Chain with absolute positioning for the next rule.
    /// </summary>
    public PositionBuilder Absolute => ChainWithPosition("absolute");

    /// <summary>
    /// Chain with fixed positioning for the next rule.
    /// </summary>
    public PositionBuilder Fixed => ChainWithPosition("fixed");

    /// <summary>
    /// Chain with sticky positioning for the next rule.
    /// </summary>
    public PositionBuilder Sticky => ChainWithPosition("sticky");

    /// <summary>
    /// Apply on phone devices (portrait phones, less than 576px).
    /// </summary>
    public PositionBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);

    /// <summary>
    /// Apply on mobile devices (landscape phones, 576px and up).
    /// </summary>
    public PositionBuilder OnMobile => ChainWithBreakpoint(Breakpoint.Mobile);

    /// <summary>
    /// Apply on tablet devices (tablets, 768px and up).
    /// </summary>
    public PositionBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);

    /// <summary>
    /// Apply on laptop devices (laptops, 992px and up).
    /// </summary>
    public PositionBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);

    /// <summary>
    /// Apply on desktop devices (desktops, 1200px and up).
    /// </summary>
    public PositionBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);

    /// <summary>
    /// Apply on wide screen devices (larger desktops, 1400px and up).
    /// </summary>
    public PositionBuilder OnWideScreen => ChainWithBreakpoint(Breakpoint.ExtraExtraLarge);

    private PositionBuilder ChainWithPosition(string position)
    {
        var newRules = new List<PositionRule>(_rules) { new PositionRule(position, null) };
        return new PositionBuilder(newRules);
    }

    private PositionBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        PositionRule? lastRule = _rules.LastOrDefault();
        if (lastRule == null)
            return new PositionBuilder("static", breakpoint);

        var newRules = new List<PositionRule>(_rules);
        // Update the last rule with the new breakpoint
        newRules[newRules.Count - 1] = new PositionRule(lastRule.Position, breakpoint);
        return new PositionBuilder(newRules);
    }

    /// <summary>
    /// Gets the CSS class string for the current configuration.
    /// </summary>
    public string ToClass()
    {
        if (_rules.Count == 0)
            return string.Empty;

        var classes = new List<string>(_rules.Count);

        foreach (PositionRule rule in _rules)
        {
            string positionClass = GetPositionClass(rule.Position);
            string breakpointClass = GetBreakpointClass(rule.Breakpoint);

            if (positionClass.HasContent())
            {
                string className = positionClass;
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

        foreach (PositionRule rule in _rules)
        {
            if (rule.Position.HasContent())
            {
                styles.Add($"position: {rule.Position}");
            }
        }

        return string.Join("; ", styles);
    }

    private static string GetPositionClass(string position)
    {
        return position switch
        {
            "static" => "position-static",
            "relative" => "position-relative",
            "absolute" => "position-absolute",
            "fixed" => "position-fixed",
            "sticky" => "position-sticky",
            _ => string.Empty
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
