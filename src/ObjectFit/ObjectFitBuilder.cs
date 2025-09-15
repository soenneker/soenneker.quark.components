using System.Collections.Generic;
using System.Linq;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Common;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.ObjectFit;

/// <summary>
/// Simplified object-fit builder with fluent API for chaining object-fit rules.
/// </summary>
public sealed class ObjectFitBuilder : ICssBuilder
{
    private readonly List<ObjectFitRule> _rules = [];

    internal ObjectFitBuilder(string fit, Breakpoint? breakpoint = null)
    {
        _rules.Add(new ObjectFitRule(fit, breakpoint));
    }

    internal ObjectFitBuilder(List<ObjectFitRule> rules)
    {
        _rules.AddRange(rules);
    }

    /// <summary>
    /// Chain with contain for the next rule.
    /// </summary>
    public ObjectFitBuilder Contain => ChainWithFit("contain");

    /// <summary>
    /// Chain with cover for the next rule.
    /// </summary>
    public ObjectFitBuilder Cover => ChainWithFit("cover");

    /// <summary>
    /// Chain with fill for the next rule.
    /// </summary>
    public ObjectFitBuilder Fill => ChainWithFit("fill");

    /// <summary>
    /// Chain with scale-down for the next rule.
    /// </summary>
    public ObjectFitBuilder ScaleDown => ChainWithFit("scale-down");

    /// <summary>
    /// Chain with none for the next rule.
    /// </summary>
    public ObjectFitBuilder None => ChainWithFit("none");

    /// <summary>
    /// Apply on phone devices (portrait phones, less than 576px).
    /// </summary>
    public ObjectFitBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);

    /// <summary>
    /// Apply on mobile devices (landscape phones, 576px and up).
    /// </summary>
    public ObjectFitBuilder OnMobile => ChainWithBreakpoint(Breakpoint.Mobile);

    /// <summary>
    /// Apply on tablet devices (tablets, 768px and up).
    /// </summary>
    public ObjectFitBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);

    /// <summary>
    /// Apply on laptop devices (laptops, 992px and up).
    /// </summary>
    public ObjectFitBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);

    /// <summary>
    /// Apply on desktop devices (desktops, 1200px and up).
    /// </summary>
    public ObjectFitBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);

    /// <summary>
    /// Apply on wide screen devices (larger desktops, 1400px and up).
    /// </summary>
    public ObjectFitBuilder OnWideScreen => ChainWithBreakpoint(Breakpoint.ExtraExtraLarge);

    private ObjectFitBuilder ChainWithFit(string fit)
    {
        var newRules = new List<ObjectFitRule>(_rules) { new ObjectFitRule(fit, null) };
        return new ObjectFitBuilder(newRules);
    }

    private ObjectFitBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        ObjectFitRule? lastRule = _rules.LastOrDefault();
        if (lastRule == null)
            return new ObjectFitBuilder("contain", breakpoint);

        var newRules = new List<ObjectFitRule>(_rules);
        // Update the last rule with the new breakpoint
        newRules[newRules.Count - 1] = new ObjectFitRule(lastRule.Fit, breakpoint);
        return new ObjectFitBuilder(newRules);
    }

    /// <summary>
    /// Gets the CSS class string for the current configuration.
    /// </summary>
    public string ToClass()
    {
        if (_rules.Count == 0)
            return string.Empty;

        var classes = new List<string>(_rules.Count);

        foreach (ObjectFitRule rule in _rules)
        {
            string fitClass = GetFitClass(rule.Fit);
            string breakpointClass = GetBreakpointClass(rule.Breakpoint);

            if (fitClass.HasContent())
            {
                string className = fitClass;
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

        foreach (ObjectFitRule rule in _rules)
        {
            if (rule.Fit.HasContent())
            {
                styles.Add($"object-fit: {rule.Fit}");
            }
        }

        return string.Join("; ", styles);
    }

    private static string GetFitClass(string fit)
    {
        return fit switch
        {
            "contain" => "object-fit-contain",
            "cover" => "object-fit-cover",
            "fill" => "object-fit-fill",
            "scale-down" => "object-fit-scale",
            "none" => "object-fit-none",
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


