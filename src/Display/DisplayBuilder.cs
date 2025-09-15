using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Common;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Display;

/// <summary>
/// Simplified display builder with fluent API for chaining display rules.
/// </summary>
public sealed class DisplayBuilder : ICssBuilder
{
    private readonly List<DisplayRule> _rules = [];

    internal DisplayBuilder(string display, Breakpoint? breakpoint = null)
    {
        _rules.Add(new DisplayRule(display, breakpoint));
    }

    internal DisplayBuilder(List<DisplayRule> rules)
    {
        _rules.AddRange(rules);
    }

    /// <summary>
    /// Chain with display none for the next rule.
    /// </summary>
    public DisplayBuilder None => ChainWithDisplay("none");

    /// <summary>
    /// Chain with display inline for the next rule.
    /// </summary>
    public DisplayBuilder Inline => ChainWithDisplay("inline");

    /// <summary>
    /// Chain with display inline-block for the next rule.
    /// </summary>
    public DisplayBuilder InlineBlock => ChainWithDisplay("inline-block");

    /// <summary>
    /// Chain with display block for the next rule.
    /// </summary>
    public DisplayBuilder Block => ChainWithDisplay("block");

    /// <summary>
    /// Chain with display flex for the next rule.
    /// </summary>
    public DisplayBuilder Flex => ChainWithDisplay("flex");

    /// <summary>
    /// Chain with display inline-flex for the next rule.
    /// </summary>
    public DisplayBuilder InlineFlex => ChainWithDisplay("inline-flex");

    /// <summary>
    /// Chain with display grid for the next rule.
    /// </summary>
    public DisplayBuilder Grid => ChainWithDisplay("grid");

    /// <summary>
    /// Chain with display inline-grid for the next rule.
    /// </summary>
    public DisplayBuilder InlineGrid => ChainWithDisplay("inline-grid");

    /// <summary>
    /// Chain with display table for the next rule.
    /// </summary>
    public DisplayBuilder Table => ChainWithDisplay("table");

    /// <summary>
    /// Chain with display table-cell for the next rule.
    /// </summary>
    public DisplayBuilder TableCell => ChainWithDisplay("table-cell");

    /// <summary>
    /// Chain with display table-row for the next rule.
    /// </summary>
    public DisplayBuilder TableRow => ChainWithDisplay("table-row");

    /// <summary>
    /// Apply on phone devices (portrait phones, less than 576px).
    /// </summary>
    public DisplayBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);

    /// <summary>
    /// Apply on mobile devices (landscape phones, 576px and up).
    /// </summary>
    public DisplayBuilder OnMobile => ChainWithBreakpoint(Breakpoint.Mobile);

    /// <summary>
    /// Apply on tablet devices (tablets, 768px and up).
    /// </summary>
    public DisplayBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);

    /// <summary>
    /// Apply on laptop devices (laptops, 992px and up).
    /// </summary>
    public DisplayBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);

    /// <summary>
    /// Apply on desktop devices (desktops, 1200px and up).
    /// </summary>
    public DisplayBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);

    /// <summary>
    /// Apply on wide screen devices (larger desktops, 1400px and up).
    /// </summary>
    public DisplayBuilder OnWideScreen => ChainWithBreakpoint(Breakpoint.ExtraExtraLarge);

    private DisplayBuilder ChainWithDisplay(string display)
    {
        var newRules = new List<DisplayRule>(_rules) { new DisplayRule(display, null) };
        return new DisplayBuilder(newRules);
    }

    private DisplayBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        DisplayRule? lastRule = _rules.LastOrDefault();
        if (lastRule == null)
            return new DisplayBuilder("block", breakpoint);

        var newRules = new List<DisplayRule>(_rules);
        // Update the last rule with the new breakpoint
        newRules[newRules.Count - 1] = new DisplayRule(lastRule.Display, breakpoint);
        return new DisplayBuilder(newRules);
    }

    /// <summary>
    /// Gets the CSS class string for the current configuration.
    /// </summary>
    public string ToClass()
    {
        if (_rules.Count == 0)
            return string.Empty;

        var classes = new List<string>(_rules.Count);

        foreach (DisplayRule rule in _rules)
        {
            string displayClass = GetDisplayClass(rule.Display);
            string breakpointClass = GetBreakpointClass(rule.Breakpoint);

            if (displayClass.HasContent())
            {
                string className = displayClass;
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

        foreach (DisplayRule rule in _rules)
        {
            if (rule.Display.HasContent())
            {
                styles.Add($"display: {rule.Display}");
            }
        }

        return string.Join("; ", styles);
    }

    private static string GetDisplayClass(string display)
    {
        return display switch
        {
            "none" => "d-none",
            "inline" => "d-inline",
            "inline-block" => "d-inline-block",
            "block" => "d-block",
            "flex" => "d-flex",
            "inline-flex" => "d-inline-flex",
            "grid" => "d-grid",
            "inline-grid" => "d-inline-grid",
            "table" => "d-table",
            "table-cell" => "d-table-cell",
            "table-row" => "d-table-row",
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
