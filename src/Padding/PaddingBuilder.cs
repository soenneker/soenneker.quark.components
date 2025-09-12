using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Common;
using Soenneker.Quark.Enums.Breakpoints;
using Soenneker.Quark.Enums.ElementSides;

namespace Soenneker.Quark.Components.Padding;

/// <summary>
/// Simplified padding builder with fluent API for chaining padding rules.
/// </summary>
public sealed class PaddingBuilder : ICssBuilder
{
    private readonly List<PaddingRule> _rules = [];

    internal PaddingBuilder(int size, Breakpoint? breakpoint = null)
    {
        if (size >= 0)
            _rules.Add(new PaddingRule(size, ElementSide.All, breakpoint));
        else
            _rules.Add(new PaddingRule(-1, ElementSide.All, breakpoint)); // Auto
    }

    internal PaddingBuilder(List<PaddingRule> rules)
    {
        _rules.AddRange(rules);
    }

    /// <summary>
    /// Apply to the top.
    /// </summary>
    public PaddingBuilder FromTop => AddRule(ElementSide.Top);

    /// <summary>
    /// Apply to the right.
    /// </summary>
    public PaddingBuilder FromRight => AddRule(ElementSide.Right);

    /// <summary>
    /// Apply to the bottom.
    /// </summary>
    public PaddingBuilder FromBottom => AddRule(ElementSide.Bottom);

    /// <summary>
    /// Apply to the left.
    /// </summary>
    public PaddingBuilder FromLeft => AddRule(ElementSide.Left);

    /// <summary>
    /// Apply to horizontal sides (left and right).
    /// </summary>
    public PaddingBuilder OnX => AddRule(ElementSide.Horizontal);

    /// <summary>
    /// Apply to vertical sides (top and bottom).
    /// </summary>
    public PaddingBuilder OnY => AddRule(ElementSide.Vertical);

    /// <summary>
    /// Apply to all sides.
    /// </summary>
    public PaddingBuilder OnAll => AddRule(ElementSide.All);

    /// <summary>
    /// Apply to start (left in LTR, right in RTL).
    /// </summary>
    public PaddingBuilder FromStart => AddRule(ElementSide.InlineStart);

    /// <summary>
    /// Apply to end (right in LTR, left in RTL).
    /// </summary>
    public PaddingBuilder FromEnd => AddRule(ElementSide.InlineEnd);

    /// <summary>
    /// Chain with a new size for the next rule.
    /// </summary>
    public PaddingBuilder S0 => ChainWithSize(0);

    /// <summary>
    /// Chain with a new size for the next rule.
    /// </summary>
    public PaddingBuilder S1 => ChainWithSize(1);

    /// <summary>
    /// Chain with a new size for the next rule.
    /// </summary>
    public PaddingBuilder S2 => ChainWithSize(2);

    /// <summary>
    /// Chain with a new size for the next rule.
    /// </summary>
    public PaddingBuilder S3 => ChainWithSize(3);

    /// <summary>
    /// Chain with a new size for the next rule.
    /// </summary>
    public PaddingBuilder S4 => ChainWithSize(4);

    /// <summary>
    /// Chain with a new size for the next rule.
    /// </summary>
    public PaddingBuilder S5 => ChainWithSize(5);

    /// <summary>
    /// Apply on phone devices (portrait phones, less than 576px).
    /// </summary>
    public PaddingBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);

    /// <summary>
    /// Apply on mobile devices (landscape phones, 576px and up).
    /// </summary>
    public PaddingBuilder OnMobile => ChainWithBreakpoint(Breakpoint.Mobile);

    /// <summary>
    /// Apply on tablet devices (tablets, 768px and up).
    /// </summary>
    public PaddingBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);

    /// <summary>
    /// Apply on laptop devices (laptops, 992px and up).
    /// </summary>
    public PaddingBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);

    /// <summary>
    /// Apply on desktop devices (desktops, 1200px and up).
    /// </summary>
    public PaddingBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);

    /// <summary>
    /// Apply on wide screen devices (larger desktops, 1400px and up).
    /// </summary>
    public PaddingBuilder OnWideScreen => ChainWithBreakpoint(Breakpoint.ExtraExtraLarge);

    private PaddingBuilder AddRule(ElementSide side)
    {
        PaddingRule? lastRule = _rules.LastOrDefault();
        int size = lastRule?.Size ?? 0;
        Breakpoint? breakpoint = lastRule?.Breakpoint;

        var newRules = new List<PaddingRule>(_rules);

        // If the last rule was "All" and we're specifying a side, replace it
        if (lastRule != null && EqualityComparer<ElementSide>.Default.Equals(lastRule.Side, ElementSide.All))
        {
            newRules[newRules.Count - 1] = new PaddingRule(size, side, breakpoint);
        }
        else
        {
            newRules.Add(new PaddingRule(size, side, breakpoint));
        }

        return new PaddingBuilder(newRules);
    }

    private PaddingBuilder ChainWithSize(int size)
    {
        var newRules = new List<PaddingRule>(_rules) { new PaddingRule(size, ElementSide.All, null) };
        return new PaddingBuilder(newRules);
    }

    private PaddingBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        PaddingRule? lastRule = _rules.LastOrDefault();
        if (lastRule == null)
            return new PaddingBuilder(0, breakpoint);

        var newRules = new List<PaddingRule>(_rules);
        // Update the last rule with the new breakpoint
        newRules[newRules.Count - 1] = new PaddingRule(lastRule.Size, lastRule.Side, breakpoint);
        return new PaddingBuilder(newRules);
    }

    /// <summary>
    /// Gets the CSS class string for the current configuration.
    /// </summary>
    public string ToClass()
    {
        if (_rules.Count == 0)
            return string.Empty;

        var classes = new List<string>(_rules.Count);

        foreach (PaddingRule rule in _rules)
        {
            string sizeClass = GetSizeClass(rule.Size);
            string sideClass = GetSideClass(rule.Side);
            string breakpointClass = GetBreakpointClass(rule.Breakpoint);

            if (sizeClass.HasContent())
            {
                if (sideClass.HasContent())
                {
                    // Build the class name correctly: prefix + side + "-" + size
                    string baseClass = sizeClass.Substring(0, 1); // "p"
                    string size = sizeClass.Substring(2); // "3"
                    var className = $"{baseClass}{sideClass}-{size}";

                    if (breakpointClass.HasContent())
                        className = $"{breakpointClass}-{className}";

                    classes.Add(className);
                }
                else
                {
                    string className = sizeClass;
                    if (breakpointClass.HasContent())
                        className = $"{breakpointClass}-{className}";

                    classes.Add(className);
                }
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

        foreach (PaddingRule rule in _rules)
        {
            string? sizeValue = GetSizeValue(rule.Size);
            if (sizeValue == null) continue;

            string sideValue = GetSideValue(rule.Side);

            if (sideValue == "all")
            {
                styles.Add($"padding: {sizeValue}");
            }
            else if (sideValue.Contains(";"))
            {
                // Handle horizontal/vertical cases that need multiple properties
                var parts = sideValue.Split(';');
                foreach (var part in parts)
                {
                    if (part.HasContent())
                    {
                        styles.Add($"padding-{part.Trim()}: {sizeValue}");
                    }
                }
            }
            else
            {
                styles.Add($"padding-{sideValue}: {sizeValue}");
            }
        }

        return string.Join("; ", styles);
    }

    private static string GetSizeClass(int size)
    {
        return size switch
        {
            0 => "p-0",
            1 => "p-1",
            2 => "p-2",
            3 => "p-3",
            4 => "p-4",
            5 => "p-5",
            -1 => "p-auto",
            _ => string.Empty
        };
    }

    private static string GetSideClass(ElementSide side)
    {
        return side.Value switch
        {
            "all" => string.Empty,
            "top" => "t",
            "right" => "e",
            "bottom" => "b",
            "left" => "s",
            "horizontal" or "left-right" => "x",
            "vertical" or "top-bottom" => "y",
            "inline-start" => "s",
            "inline-end" => "e",
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
            -1 => "auto",
            _ => null
        };
    }

    private static string GetSideValue(ElementSide side)
    {
        return side.Value switch
        {
            "all" => "all",
            "top" => "top",
            "right" => "right",
            "bottom" => "bottom",
            "left" => "left",
            "horizontal" or "left-right" => "left; right",
            "vertical" or "top-bottom" => "top; bottom",
            "inline-start" => "inline-start",
            "inline-end" => "inline-end",
            _ => "all"
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