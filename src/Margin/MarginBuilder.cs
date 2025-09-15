using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Common;
using Soenneker.Quark.Enums.Breakpoints;
using Soenneker.Quark.Enums.ElementSides;

namespace Soenneker.Quark.Components.Margin;

/// <summary>
/// Simplified margin builder with fluent API for chaining margin rules.
/// </summary>
public sealed class MarginBuilder : ICssBuilder
{
    private readonly List<MarginRule> _rules = [];

    internal MarginBuilder(int size, Breakpoint? breakpoint = null)
    {
        if (size >= 0)
            _rules.Add(new MarginRule(size, ElementSide.All, breakpoint));
        else
            _rules.Add(new MarginRule(-1, ElementSide.All, breakpoint)); // Auto
    }

    internal MarginBuilder(List<MarginRule> rules)
    {
        _rules.AddRange(rules);
    }

    /// <summary>
    /// Apply to the top.
    /// </summary>
    public MarginBuilder FromTop => AddRule(ElementSide.Top);

    /// <summary>
    /// Apply to the right.
    /// </summary>
    public MarginBuilder FromRight => AddRule(ElementSide.Right);

    /// <summary>
    /// Apply to the bottom.
    /// </summary>
    public MarginBuilder FromBottom => AddRule(ElementSide.Bottom);

    /// <summary>
    /// Apply to the left.
    /// </summary>
    public MarginBuilder FromLeft => AddRule(ElementSide.Left);

    /// <summary>
    /// Apply to horizontal sides (left and right).
    /// </summary>
    public MarginBuilder OnX => AddRule(ElementSide.Horizontal);

    /// <summary>
    /// Apply to vertical sides (top and bottom).
    /// </summary>
    public MarginBuilder OnY => AddRule(ElementSide.Vertical);

    /// <summary>
    /// Apply to all sides.
    /// </summary>
    public MarginBuilder OnAll => AddRule(ElementSide.All);

    /// <summary>
    /// Apply to start (left in LTR, right in RTL).
    /// </summary>
    public MarginBuilder FromStart => AddRule(ElementSide.InlineStart);

    /// <summary>
    /// Apply to end (right in LTR, left in RTL).
    /// </summary>
    public MarginBuilder FromEnd => AddRule(ElementSide.InlineEnd);

    /// <summary>
    /// Chain with a new size for the next rule.
    /// </summary>
    public MarginBuilder S0 => ChainWithSize(0);

    /// <summary>
    /// Chain with a new size for the next rule.
    /// </summary>
    public MarginBuilder S1 => ChainWithSize(1);

    /// <summary>
    /// Chain with a new size for the next rule.
    /// </summary>
    public MarginBuilder S2 => ChainWithSize(2);

    /// <summary>
    /// Chain with a new size for the next rule.
    /// </summary>
    public MarginBuilder S3 => ChainWithSize(3);

    /// <summary>
    /// Chain with a new size for the next rule.
    /// </summary>
    public MarginBuilder S4 => ChainWithSize(4);

    /// <summary>
    /// Chain with a new size for the next rule.
    /// </summary>
    public MarginBuilder S5 => ChainWithSize(5);

    /// <summary>
    /// Chain with auto size for the next margin rule.
    /// </summary>
    public MarginBuilder Auto => ChainWithSize(-1);

    /// <summary>
    /// Apply on phone devices (portrait phones, less than 576px).
    /// </summary>
    public MarginBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);

    /// <summary>
    /// Apply on mobile devices (landscape phones, 576px and up).
    /// </summary>
    public MarginBuilder OnMobile => ChainWithBreakpoint(Breakpoint.Mobile);

    /// <summary>
    /// Apply on tablet devices (tablets, 768px and up).
    /// </summary>
    public MarginBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);

    /// <summary>
    /// Apply on laptop devices (laptops, 992px and up).
    /// </summary>
    public MarginBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);

    /// <summary>
    /// Apply on desktop devices (desktops, 1200px and up).
    /// </summary>
    public MarginBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);

    /// <summary>
    /// Apply on wide screen devices (larger desktops, 1400px and up).
    /// </summary>
    public MarginBuilder OnWideScreen => ChainWithBreakpoint(Breakpoint.ExtraExtraLarge);

    private MarginBuilder AddRule(ElementSide side)
    {
        MarginRule? lastRule = _rules.LastOrDefault();
        int size = lastRule?.Size ?? 0;
        Breakpoint? breakpoint = lastRule?.Breakpoint;

        var newRules = new List<MarginRule>(_rules);

        // If the last rule was "All" and we're specifying a side, replace it
        if (lastRule != null && EqualityComparer<ElementSide>.Default.Equals(lastRule.Side, ElementSide.All))
        {
            newRules[newRules.Count - 1] = new MarginRule(size, side, breakpoint);
        }
        else
        {
            newRules.Add(new MarginRule(size, side, breakpoint));
        }

        return new MarginBuilder(newRules);
    }

    private MarginBuilder ChainWithSize(int size)
    {
        var newRules = new List<MarginRule>(_rules) { new MarginRule(size, ElementSide.All, null) };
        return new MarginBuilder(newRules);
    }

    private MarginBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        MarginRule? lastRule = _rules.LastOrDefault();
        if (lastRule == null)
            return new MarginBuilder(0, breakpoint);

        var newRules = new List<MarginRule>(_rules);
        // Update the last rule with the new breakpoint
        newRules[newRules.Count - 1] = new MarginRule(lastRule.Size, lastRule.Side, breakpoint);
        return new MarginBuilder(newRules);
    }

    /// <summary>
    /// Gets the CSS class string for the current configuration.
    /// </summary>
    public string ToClass()
    {
        if (_rules.Count == 0)
            return string.Empty;

        var classes = new List<string>(_rules.Count);

        foreach (MarginRule rule in _rules)
        {
            string sizeClass = GetSizeClass(rule.Size);
            string sideClass = GetSideClass(rule.Side);
            string breakpointClass = GetBreakpointClass(rule.Breakpoint);

            if (sizeClass.HasContent())
            {
                if (sideClass.HasContent())
                {
                    // Build the class name correctly: prefix + side + "-" + size
                    string baseClass = sizeClass.Substring(0, 1); // "m"
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

        foreach (MarginRule rule in _rules)
        {
            string? sizeValue = GetSizeValue(rule.Size);
            if (sizeValue == null) continue;

            string sideValue = GetSideValue(rule.Side);

            if (sideValue == "all")
            {
                styles.Add($"margin: {sizeValue}");
            }
            else if (sideValue.Contains(";"))
            {
                // Handle horizontal/vertical cases that need multiple properties
                string[] parts = sideValue.Split(';');
                foreach (string part in parts)
                {
                    if (part.HasContent())
                    {
                        styles.Add($"margin-{part.Trim()}: {sizeValue}");
                    }
                }
            }
            else
            {
                styles.Add($"margin-{sideValue}: {sizeValue}");
            }
        }

        return string.Join("; ", styles);
    }

    private static string GetSizeClass(int size)
    {
        return size switch
        {
            0 => "m-0",
            1 => "m-1",
            2 => "m-2",
            3 => "m-3",
            4 => "m-4",
            5 => "m-5",
            -1 => "m-auto",
            _ => string.Empty
        };
    }

    private static string GetSideClass(ElementSide side)
    {
        switch (side)
        {
            case ElementSide.AllValue:
                return string.Empty;
            case ElementSide.TopValue:
                return "t";
            case ElementSide.RightValue:
                return "e";
            case ElementSide.BottomValue:
                return "b";
            case ElementSide.LeftValue:
                return "s";
            case ElementSide.HorizontalValue:
            case ElementSide.LeftRightValue:
                return "x";
            case ElementSide.VerticalValue:
            case ElementSide.TopBottomValue:
                return "y";
            case ElementSide.InlineStartValue:
                return "s";
            case ElementSide.InlineEndValue:
                return "e";
            default:
                return string.Empty;
        }
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
        switch (side)
        {
            case ElementSide.AllValue:
                return "all";
            case ElementSide.TopValue:
                return "top";
            case ElementSide.RightValue:
                return "right";
            case ElementSide.BottomValue:
                return "bottom";
            case ElementSide.LeftValue:
                return "left";
            case ElementSide.HorizontalValue:
            case ElementSide.LeftRightValue:
                return "left; right";
            case ElementSide.VerticalValue:
            case ElementSide.TopBottomValue:
                return "top; bottom";
            case ElementSide.InlineStartValue:
                return "inline-start";
            case ElementSide.InlineEndValue:
                return "inline-end";
            default:
                return "all";
        }
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