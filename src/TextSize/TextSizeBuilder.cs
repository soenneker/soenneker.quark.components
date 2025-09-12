using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Common;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.TextSize;

/// <summary>
/// Simplified text size builder with fluent API for chaining text size rules.
/// </summary>
public sealed class TextSizeBuilder : ICssBuilder
{
    private readonly List<TextSizeRule> _rules = [];

    internal TextSizeBuilder(string size, Breakpoint? breakpoint = null)
    {
        _rules.Add(new TextSizeRule(size, breakpoint));
    }

    internal TextSizeBuilder(List<TextSizeRule> rules)
    {
        _rules.AddRange(rules);
    }

    /// <summary>
    /// Chain with extra small text size for the next rule.
    /// </summary>
    public TextSizeBuilder Xs => ChainWithSize("xs");

    /// <summary>
    /// Chain with small text size for the next rule.
    /// </summary>
    public TextSizeBuilder Sm => ChainWithSize("sm");

    /// <summary>
    /// Chain with base text size for the next rule.
    /// </summary>
    public TextSizeBuilder Base => ChainWithSize("base");

    /// <summary>
    /// Chain with large text size for the next rule.
    /// </summary>
    public TextSizeBuilder Lg => ChainWithSize("lg");

    /// <summary>
    /// Chain with extra large text size for the next rule.
    /// </summary>
    public TextSizeBuilder Xl => ChainWithSize("xl");

    /// <summary>
    /// Chain with 2X large text size for the next rule.
    /// </summary>
    public TextSizeBuilder Xl2 => ChainWithSize("2xl");

    /// <summary>
    /// Chain with 3X large text size for the next rule.
    /// </summary>
    public TextSizeBuilder Xl3 => ChainWithSize("3xl");

    /// <summary>
    /// Chain with 4X large text size for the next rule.
    /// </summary>
    public TextSizeBuilder Xl4 => ChainWithSize("4xl");

    /// <summary>
    /// Chain with 5X large text size for the next rule.
    /// </summary>
    public TextSizeBuilder Xl5 => ChainWithSize("5xl");

    /// <summary>
    /// Chain with 6X large text size for the next rule.
    /// </summary>
    public TextSizeBuilder Xl6 => ChainWithSize("6xl");

    /// <summary>
    /// Chain with 7X large text size for the next rule.
    /// </summary>
    public TextSizeBuilder Xl7 => ChainWithSize("7xl");

    /// <summary>
    /// Chain with 8X large text size for the next rule.
    /// </summary>
    public TextSizeBuilder Xl8 => ChainWithSize("8xl");

    /// <summary>
    /// Chain with 9X large text size for the next rule.
    /// </summary>
    public TextSizeBuilder Xl9 => ChainWithSize("9xl");

    /// <summary>
    /// Apply on phone devices (portrait phones, less than 576px).
    /// </summary>
    public TextSizeBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);

    /// <summary>
    /// Apply on mobile devices (landscape phones, 576px and up).
    /// </summary>
    public TextSizeBuilder OnMobile => ChainWithBreakpoint(Breakpoint.Mobile);

    /// <summary>
    /// Apply on tablet devices (tablets, 768px and up).
    /// </summary>
    public TextSizeBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);

    /// <summary>
    /// Apply on laptop devices (laptops, 992px and up).
    /// </summary>
    public TextSizeBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);

    /// <summary>
    /// Apply on desktop devices (desktops, 1200px and up).
    /// </summary>
    public TextSizeBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);

    /// <summary>
    /// Apply on wide screen devices (larger desktops, 1400px and up).
    /// </summary>
    public TextSizeBuilder OnWideScreen => ChainWithBreakpoint(Breakpoint.ExtraExtraLarge);

    private TextSizeBuilder ChainWithSize(string size)
    {
        var newRules = new List<TextSizeRule>(_rules) { new TextSizeRule(size, null) };
        return new TextSizeBuilder(newRules);
    }

    private TextSizeBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        TextSizeRule? lastRule = _rules.LastOrDefault();
        if (lastRule == null)
            return new TextSizeBuilder("base", breakpoint);

        var newRules = new List<TextSizeRule>(_rules);
        // Update the last rule with the new breakpoint
        newRules[newRules.Count - 1] = new TextSizeRule(lastRule.Size, breakpoint);
        return new TextSizeBuilder(newRules);
    }

    /// <summary>
    /// Gets the CSS class string for the current configuration.
    /// </summary>
    public string ToClass()
    {
        if (_rules.Count == 0)
            return string.Empty;

        var classes = new List<string>(_rules.Count);

        foreach (TextSizeRule rule in _rules)
        {
            string sizeClass = GetSizeClass(rule.Size);
            string breakpointClass = GetBreakpointClass(rule.Breakpoint);

            if (sizeClass.HasContent())
            {
                string className = sizeClass;
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

        foreach (TextSizeRule rule in _rules)
        {
            string? sizeValue = GetSizeValue(rule.Size);
            if (sizeValue == null) continue;

            styles.Add($"font-size: {sizeValue}");
        }

        return string.Join("; ", styles);
    }

    private static string GetSizeClass(string size)
    {
        return size switch
        {
            "xs" => "fs-6",
            "sm" => "fs-5",
            "base" => "fs-4",
            "lg" => "fs-3",
            "xl" => "fs-2",
            "2xl" => "fs-1",
            "3xl" => "display-6",
            "4xl" => "display-5",
            "5xl" => "display-4",
            "6xl" => "display-3",
            "7xl" => "display-2",
            "8xl" => "display-1",
            "9xl" => "display-1", // Bootstrap doesn't have display-0, so use display-1
            _ => string.Empty
        };
    }

    private static string? GetSizeValue(string size)
    {
        return size switch
        {
            "xs" => "0.75rem",
            "sm" => "0.875rem",
            "base" => "1rem",
            "lg" => "1.125rem",
            "xl" => "1.25rem",
            "2xl" => "1.5rem",
            "3xl" => "1.75rem",
            "4xl" => "2rem",
            "5xl" => "2.25rem",
            "6xl" => "2.5rem",
            "7xl" => "3rem",
            "8xl" => "3.5rem",
            "9xl" => "4rem",
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
