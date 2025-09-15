using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Abstract;
using Soenneker.Quark.Enums.Breakpoints;
using Soenneker.Quark.Enums.GlobalKeywords;
using TextOverflowEnum = Soenneker.Quark.Enums.TextOverflows.TextOverflow;

namespace Soenneker.Quark.Components.TextOverflow;

/// <summary>
/// Simplified text overflow builder with fluent API for chaining text overflow rules.
/// </summary>
public sealed class TextOverflowBuilder : ICssBuilder
{
    private readonly List<TextOverflowRule> _rules = [];

    internal TextOverflowBuilder(TextOverflowEnum textOverflow, Breakpoint? breakpoint = null)
    {
        _rules.Add(new TextOverflowRule(textOverflow, breakpoint));
    }

    internal TextOverflowBuilder(List<TextOverflowRule> rules)
    {
        _rules.AddRange(rules);
    }

    /// <summary>
    /// Chain with clip text overflow for the next rule.
    /// </summary>
    public TextOverflowBuilder Clip => ChainWithTextOverflow(TextOverflowEnum.Clip);

    /// <summary>
    /// Chain with ellipsis text overflow for the next rule.
    /// </summary>
    public TextOverflowBuilder Ellipsis => ChainWithTextOverflow(TextOverflowEnum.Ellipsis);

    /// <summary>
    /// Chain with inherit text overflow for the next rule.
    /// </summary>
    public TextOverflowBuilder Inherit => ChainWithTextOverflow(GlobalKeyword.Inherit);

    /// <summary>
    /// Chain with initial text overflow for the next rule.
    /// </summary>
    public TextOverflowBuilder Initial => ChainWithTextOverflow(GlobalKeyword.Initial);

    /// <summary>
    /// Chain with revert text overflow for the next rule.
    /// </summary>
    public TextOverflowBuilder Revert => ChainWithTextOverflow(GlobalKeyword.Revert);

    /// <summary>
    /// Chain with revert-layer text overflow for the next rule.
    /// </summary>
    public TextOverflowBuilder RevertLayer => ChainWithTextOverflow(GlobalKeyword.RevertLayer);

    /// <summary>
    /// Chain with unset text overflow for the next rule.
    /// </summary>
    public TextOverflowBuilder Unset => ChainWithTextOverflow(GlobalKeyword.Unset);

    /// <summary>
    /// Apply on phone devices (portrait phones, less than 576px).
    /// </summary>
    public TextOverflowBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);

    /// <summary>
    /// Apply on mobile devices (landscape phones, 576px and up).
    /// </summary>
    public TextOverflowBuilder OnMobile => ChainWithBreakpoint(Breakpoint.Mobile);

    /// <summary>
    /// Apply on tablet devices (tablets, 768px and up).
    /// </summary>
    public TextOverflowBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);

    /// <summary>
    /// Apply on laptop devices (laptops, 992px and up).
    /// </summary>
    public TextOverflowBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);

    /// <summary>
    /// Apply on desktop devices (desktops, 1200px and up).
    /// </summary>
    public TextOverflowBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);

    /// <summary>
    /// Apply on wide screen devices (larger desktops, 1400px and up).
    /// </summary>
    public TextOverflowBuilder OnWideScreen => ChainWithBreakpoint(Breakpoint.ExtraExtraLarge);

    private TextOverflowBuilder ChainWithTextOverflow(TextOverflowEnum textOverflow)
    {
        var newRules = new List<TextOverflowRule>(_rules) { new TextOverflowRule(textOverflow, null) };
        return new TextOverflowBuilder(newRules);
    }

    private TextOverflowBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        TextOverflowRule? lastRule = _rules.LastOrDefault();
        if (lastRule == null)
            return new TextOverflowBuilder(TextOverflowEnum.Clip, breakpoint);

        var newRules = new List<TextOverflowRule>(_rules);
        // Update the last rule with the new breakpoint
        newRules[newRules.Count - 1] = new TextOverflowRule(lastRule.TextOverflow, breakpoint);
        return new TextOverflowBuilder(newRules);
    }

    /// <summary>
    /// Gets the CSS class string for the current configuration.
    /// </summary>
    public string ToClass()
    {
        if (_rules.Count == 0)
            return string.Empty;

        var classes = new List<string>(_rules.Count);

        foreach (TextOverflowRule rule in _rules)
        {
            string textOverflowClass = GetTextOverflowClass(rule.TextOverflow);
            string breakpointClass = GetBreakpointClass(rule.Breakpoint);

            if (textOverflowClass.HasContent())
            {
                string className = textOverflowClass;
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

        foreach (TextOverflowRule rule in _rules)
        {
            if (rule.TextOverflow.Value.HasContent())
            {
                styles.Add($"text-overflow: {rule.TextOverflow.Value}");
            }
        }

        return string.Join("; ", styles);
    }

    private static string GetTextOverflowClass(TextOverflowEnum textOverflow)
    {
        switch (textOverflow)
        {
            case TextOverflowEnum.ClipValue:
                return "text-truncate";
            case TextOverflowEnum.EllipsisValue:
                return "text-truncate";
            default:
                return string.Empty;
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
