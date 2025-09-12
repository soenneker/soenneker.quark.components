using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Common;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Flex;

/// <summary>
/// Simplified flex builder with fluent API for chaining flex rules.
/// </summary>
public sealed class FlexBuilder : ICssBuilder
{
    private readonly List<FlexRule> _rules = [];

    internal FlexBuilder(string property, string? value = null, Breakpoint? breakpoint = null)
    {
        _rules.Add(new FlexRule(property, value ?? string.Empty, breakpoint));
    }

    internal FlexBuilder(List<FlexRule> rules)
    {
        _rules.AddRange(rules);
    }

    // Display properties
    /// <summary>
    /// Chain with display flex for the next rule.
    /// </summary>
    public FlexBuilder Display => ChainWithRule("display", string.Empty);

    // Direction properties
    /// <summary>
    /// Chain with flex direction row for the next rule.
    /// </summary>
    public FlexBuilder Row => ChainWithRule("direction", "row");

    /// <summary>
    /// Chain with flex direction column for the next rule.
    /// </summary>
    public FlexBuilder Column => ChainWithRule("direction", "column");

    // Wrap properties
    /// <summary>
    /// Chain with flex wrap for the next rule.
    /// </summary>
    public FlexBuilder Wrap => ChainWithRule("wrap", "wrap");

    /// <summary>
    /// Chain with flex nowrap for the next rule.
    /// </summary>
    public FlexBuilder NoWrap => ChainWithRule("wrap", "nowrap");

    // Justify content properties
    /// <summary>
    /// Chain with justify content start for the next rule.
    /// </summary>
    public FlexBuilder JustifyStart => ChainWithRule("justify", "start");

    /// <summary>
    /// Chain with justify content end for the next rule.
    /// </summary>
    public FlexBuilder JustifyEnd => ChainWithRule("justify", "end");

    /// <summary>
    /// Chain with justify content center for the next rule.
    /// </summary>
    public FlexBuilder JustifyCenter => ChainWithRule("justify", "center");

    /// <summary>
    /// Chain with justify content between for the next rule.
    /// </summary>
    public FlexBuilder JustifyBetween => ChainWithRule("justify", "between");

    /// <summary>
    /// Chain with justify content around for the next rule.
    /// </summary>
    public FlexBuilder JustifyAround => ChainWithRule("justify", "around");

    /// <summary>
    /// Chain with justify content evenly for the next rule.
    /// </summary>
    public FlexBuilder JustifyEvenly => ChainWithRule("justify", "evenly");

    // Align items properties
    /// <summary>
    /// Chain with align items start for the next rule.
    /// </summary>
    public FlexBuilder AlignStart => ChainWithRule("align", "start");

    /// <summary>
    /// Chain with align items end for the next rule.
    /// </summary>
    public FlexBuilder AlignEnd => ChainWithRule("align", "end");

    /// <summary>
    /// Chain with align items center for the next rule.
    /// </summary>
    public FlexBuilder AlignCenter => ChainWithRule("align", "center");

    /// <summary>
    /// Chain with align items baseline for the next rule.
    /// </summary>
    public FlexBuilder AlignBaseline => ChainWithRule("align", "baseline");

    /// <summary>
    /// Chain with align items stretch for the next rule.
    /// </summary>
    public FlexBuilder AlignStretch => ChainWithRule("align", "stretch");

    /// <summary>
    /// Apply on phone devices (portrait phones, less than 576px).
    /// </summary>
    public FlexBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);

    /// <summary>
    /// Apply on mobile devices (landscape phones, 576px and up).
    /// </summary>
    public FlexBuilder OnMobile => ChainWithBreakpoint(Breakpoint.Mobile);

    /// <summary>
    /// Apply on tablet devices (tablets, 768px and up).
    /// </summary>
    public FlexBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);

    /// <summary>
    /// Apply on laptop devices (laptops, 992px and up).
    /// </summary>
    public FlexBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);

    /// <summary>
    /// Apply on desktop devices (desktops, 1200px and up).
    /// </summary>
    public FlexBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);

    /// <summary>
    /// Apply on wide screen devices (larger desktops, 1400px and up).
    /// </summary>
    public FlexBuilder OnWideScreen => ChainWithBreakpoint(Breakpoint.ExtraExtraLarge);

    private FlexBuilder ChainWithRule(string property, string value)
    {
        var newRules = new List<FlexRule>(_rules) { new FlexRule(property, value, null) };
        return new FlexBuilder(newRules);
    }

    private FlexBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        FlexRule? lastRule = _rules.LastOrDefault();
        if (lastRule == null)
            return new FlexBuilder("display", string.Empty, breakpoint);

        var newRules = new List<FlexRule>(_rules);
        // Update the last rule with the new breakpoint
        newRules[newRules.Count - 1] = new FlexRule(lastRule.Property, lastRule.Value, breakpoint);
        return new FlexBuilder(newRules);
    }

    /// <summary>
    /// Gets the CSS class string for the current configuration.
    /// </summary>
    public string ToClass()
    {
        if (_rules.Count == 0)
            return string.Empty;

        var classes = new List<string>(_rules.Count);

        foreach (FlexRule rule in _rules)
        {
            string flexClass = GetFlexClass(rule.Property, rule.Value);
            string breakpointClass = GetBreakpointClass(rule.Breakpoint);

            if (flexClass.HasContent())
            {
                string className = flexClass;
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

        foreach (FlexRule rule in _rules)
        {
            string? cssProperty = GetCssProperty(rule.Property, rule.Value);
            if (cssProperty.HasContent())
            {
                styles.Add(cssProperty);
            }
        }

        return string.Join("; ", styles);
    }

    private static string GetFlexClass(string property, string value)
    {
        return property switch
        {
            "display" => "d-flex",
            "direction" => value switch
            {
                "row" => "flex-row",
                "column" => "flex-column",
                _ => string.Empty
            },
            "wrap" => value switch
            {
                "wrap" => "flex-wrap",
                "nowrap" => "flex-nowrap",
                _ => string.Empty
            },
            "justify" => value switch
            {
                "start" => "justify-content-start",
                "end" => "justify-content-end",
                "center" => "justify-content-center",
                "between" => "justify-content-between",
                "around" => "justify-content-around",
                "evenly" => "justify-content-evenly",
                _ => string.Empty
            },
            "align" => value switch
            {
                "start" => "align-items-start",
                "end" => "align-items-end",
                "center" => "align-items-center",
                "baseline" => "align-items-baseline",
                "stretch" => "align-items-stretch",
                _ => string.Empty
            },
            _ => string.Empty
        };
    }

    private static string? GetCssProperty(string property, string value)
    {
        return property switch
        {
            "display" => "display: flex",
            "direction" => value switch
            {
                "row" => "flex-direction: row",
                "column" => "flex-direction: column",
                _ => null
            },
            "wrap" => value switch
            {
                "wrap" => "flex-wrap: wrap",
                "nowrap" => "flex-wrap: nowrap",
                _ => null
            },
            "justify" => value switch
            {
                "start" => "justify-content: flex-start",
                "end" => "justify-content: flex-end",
                "center" => "justify-content: center",
                "between" => "justify-content: space-between",
                "around" => "justify-content: space-around",
                "evenly" => "justify-content: space-evenly",
                _ => null
            },
            "align" => value switch
            {
                "start" => "align-items: flex-start",
                "end" => "align-items: flex-end",
                "center" => "align-items: center",
                "baseline" => "align-items: baseline",
                "stretch" => "align-items: stretch",
                _ => null
            },
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
