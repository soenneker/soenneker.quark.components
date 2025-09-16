using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Utils.PooledStringBuilders;
using Soenneker.Quark.Components.Abstract;
using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Shadow;

public sealed class ShadowBuilder : ICssBuilder
{
    private readonly List<ShadowRule> _rules = new(4);

    // ----- Class name constants -----
    private const string _classNone = "shadow-none";
    private const string _classBase = "shadow";
    private const string _classSm = "shadow-sm";
    private const string _classLg = "shadow-lg";

    internal ShadowBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new ShadowRule(value, breakpoint));
    }

    internal ShadowBuilder(List<ShadowRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    public ShadowBuilder None => Chain("none");
    public ShadowBuilder Base => Chain("base");
    public ShadowBuilder Sm => Chain("sm");
    public ShadowBuilder Lg => Chain("lg");

    public ShadowBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public ShadowBuilder OnMobile => ChainBp(Breakpoint.Mobile);
    public ShadowBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public ShadowBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public ShadowBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public ShadowBuilder OnWideScreen => ChainBp(Breakpoint.ExtraExtraLarge);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ShadowBuilder Chain(string value)
    {
        _rules.Add(new ShadowRule(value, null));
        return this;
    }

    /// <summary>Apply a breakpoint to the most recent rule (or bootstrap with "base" if empty).</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ShadowBuilder ChainBp(Breakpoint bp)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new ShadowRule("base", bp));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        ShadowRule last = _rules[lastIdx];
        _rules[lastIdx] = new ShadowRule(last.Value, bp);
        return this;
    }

    public string ToClass()
    {
        if (_rules.Count == 0)
            return string.Empty;

        using var sb = new PooledStringBuilder();
        var first = true;

        for (var i = 0; i < _rules.Count; i++)
        {
            ShadowRule rule = _rules[i];

            string baseClass = rule.Value switch
            {
                "none" => _classNone,
                "base" => _classBase,
                "sm" => _classSm,
                "lg" => _classLg,
                _ => string.Empty
            };

            if (baseClass.Length == 0)
                continue;

            string bp = GetBp(rule.Breakpoint);
            if (bp.Length != 0)
                baseClass = InsertBreakpoint(baseClass, bp);

            if (!first)
                sb.Append(' ');
            else
                first = false;

            sb.Append(baseClass);
        }

        return sb.ToString();
    }

    public string ToStyle()
    {
        // Shadow utilities are class-first; no inline style mapping
        return string.Empty;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetBp(Breakpoint? breakpoint)
    {
        if (breakpoint is null)
            return string.Empty;

        // Classic switch statement for Intellenum *Value cases
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

    /// <summary>
    /// Insert breakpoint token as: "shadow-lg" + "md" → "shadow-md-lg".
    /// Falls back to "bp-{class}" if no dash exists.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string InsertBreakpoint(string className, string bp)
    {
        int dashIndex = className.IndexOf('-');
        if (dashIndex > 0)
        {
            // length = prefix + "-" + bp + remainder
            int len = dashIndex + 1 + bp.Length + (className.Length - dashIndex);
            return string.Create(len, (className, dashIndex, bp), static (dst, s) =>
            {
                // prefix
                s.className.AsSpan(0, s.dashIndex).CopyTo(dst);
                int idx = s.dashIndex;

                // "-" + bp
                dst[idx++] = '-';
                s.bp.AsSpan().CopyTo(dst[idx..]);
                idx += s.bp.Length;

                // remainder (starts with '-')
                s.className.AsSpan(s.dashIndex).CopyTo(dst[idx..]);
            });
        }

        // Fallback: "bp-{className}"
        return string.Create(bp.Length + 1 + className.Length, (className, bp), static (dst, s) =>
        {
            s.bp.AsSpan().CopyTo(dst);
            int idx = s.bp.Length;
            dst[idx++] = '-';
            s.className.AsSpan().CopyTo(dst[idx..]);
        });
    }
}
