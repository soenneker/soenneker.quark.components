using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Utils.PooledStringBuilders;
using Soenneker.Quark.Components.Abstract;
using Soenneker.Quark.Enums.Breakpoints;
using Soenneker.Quark.Enums.ElementSides;

namespace Soenneker.Quark.Components.Border;

/// <summary>
/// Simplified border builder with fluent API for chaining border rules.
/// </summary>
public sealed class BorderBuilder : ICssBuilder
{
    private readonly List<BorderRule> _rules = new(4);

    // ----- Class tokens -----
    private const string _baseToken = "b";

    // ----- Size tokens -----
    private const string _token0 = "0";
    private const string _token1 = "1";
    private const string _token2 = "2";
    private const string _token3 = "3";
    private const string _token4 = "4";
    private const string _token5 = "5";

    // ----- Side tokens -----
    private const string _sideT = "t";
    private const string _sideE = "e";
    private const string _sideB = "b";
    private const string _sideS = "s";
    private const string _sideX = "x";
    private const string _sideY = "y";

    internal BorderBuilder(int size, Breakpoint? breakpoint = null)
    {
        if (size >= 0)
            _rules.Add(new BorderRule(size, ElementSide.All, breakpoint));
    }

    internal BorderBuilder(List<BorderRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    // ----- Side chaining -----
    public BorderBuilder FromTop => AddRule(ElementSide.Top);
    public BorderBuilder FromRight => AddRule(ElementSide.Right);
    public BorderBuilder FromBottom => AddRule(ElementSide.Bottom);
    public BorderBuilder FromLeft => AddRule(ElementSide.Left);
    public BorderBuilder OnX => AddRule(ElementSide.Horizontal);
    public BorderBuilder OnY => AddRule(ElementSide.Vertical);
    public BorderBuilder OnAll => AddRule(ElementSide.All);
    public BorderBuilder FromStart => AddRule(ElementSide.InlineStart);
    public BorderBuilder FromEnd => AddRule(ElementSide.InlineEnd);

    // ----- Size chaining -----
    public BorderBuilder S0 => ChainWithSize(0);
    public BorderBuilder S1 => ChainWithSize(1);
    public BorderBuilder S2 => ChainWithSize(2);
    public BorderBuilder S3 => ChainWithSize(3);
    public BorderBuilder S4 => ChainWithSize(4);
    public BorderBuilder S5 => ChainWithSize(5);

    // ----- Breakpoint chaining -----
    public BorderBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);
    public BorderBuilder OnMobile => ChainWithBreakpoint(Breakpoint.Mobile);
    public BorderBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);
    public BorderBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);
    public BorderBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);
    public BorderBuilder OnWideScreen => ChainWithBreakpoint(Breakpoint.ExtraExtraLarge);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private BorderBuilder AddRule(ElementSide side)
    {
        int size = _rules.Count > 0 ? _rules[^1].Size : 0;
        Breakpoint? bp = _rules.Count > 0 ? _rules[^1].Breakpoint : null;

        if (_rules.Count > 0 && _rules[^1].Side == ElementSide.All)
        {
            _rules[^1] = new BorderRule(size, side, bp);
        }
        else
        {
            _rules.Add(new BorderRule(size, side, bp));
        }

        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private BorderBuilder ChainWithSize(int size)
    {
        _rules.Add(new BorderRule(size, ElementSide.All, null));
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private BorderBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new BorderRule(0, ElementSide.All, breakpoint));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        BorderRule last = _rules[lastIdx];
        _rules[lastIdx] = new BorderRule(last.Size, last.Side, breakpoint);
        return this;
    }

    /// <summary>Gets the CSS class string for the current configuration.</summary>
    public string ToClass()
    {
        if (_rules.Count == 0)
            return string.Empty;

        using var sb = new PooledStringBuilder();
        var first = true;

        for (var i = 0; i < _rules.Count; i++)
        {
            BorderRule rule = _rules[i];

            string sizeTok = GetSizeToken(rule.Size);

            if (sizeTok.Length == 0)
                continue;

            string sideTok = GetSideToken(rule.Side);
            string bpTok = GetBreakpointToken(rule.Breakpoint);

            if (!first)
                sb.Append(' ');
            else
                first = false;

            sb.Append(_baseToken);

            if (sideTok.Length != 0)
                sb.Append(sideTok);

            sb.Append('-');

            if (bpTok.Length != 0)
            {
                sb.Append(bpTok);
                sb.Append('-');
            }

            sb.Append(sizeTok);
        }

        return sb.ToString();
    }

    /// <summary>Gets the CSS style string for the current configuration.</summary>
    public string ToStyle()
    {
        if (_rules.Count == 0)
            return string.Empty;

        var sb = new PooledStringBuilder();
        var first = true;

        try
        {
            for (var i = 0; i < _rules.Count; i++)
            {
                BorderRule rule = _rules[i];
                string? sizeVal = GetSizeValue(rule.Size);
                if (sizeVal is null)
                    continue;

                switch (rule.Side)
                {
                    case ElementSide.AllValue:
                        AppendStyle(ref first, ref sb, "border-width", sizeVal);
                        break;

                    case ElementSide.TopValue:
                        AppendStyle(ref first, ref sb, "border-top-width", sizeVal);
                        break;

                    case ElementSide.RightValue:
                        AppendStyle(ref first, ref sb, "border-right-width", sizeVal);
                        break;

                    case ElementSide.BottomValue:
                        AppendStyle(ref first, ref sb, "border-bottom-width", sizeVal);
                        break;

                    case ElementSide.LeftValue:
                        AppendStyle(ref first, ref sb, "border-left-width", sizeVal);
                        break;

                    case ElementSide.HorizontalValue:
                    case ElementSide.LeftRightValue:
                        AppendStyle(ref first, ref sb, "border-left-width", sizeVal);
                        AppendStyle(ref first, ref sb, "border-right-width", sizeVal);
                        break;

                    case ElementSide.VerticalValue:
                    case ElementSide.TopBottomValue:
                        AppendStyle(ref first, ref sb, "border-top-width", sizeVal);
                        AppendStyle(ref first, ref sb, "border-bottom-width", sizeVal);
                        break;

                    case ElementSide.InlineStartValue:
                        AppendStyle(ref first, ref sb, "border-inline-start-width", sizeVal);
                        break;

                    case ElementSide.InlineEndValue:
                        AppendStyle(ref first, ref sb, "border-inline-end-width", sizeVal);
                        break;

                    default:
                        AppendStyle(ref first, ref sb, "border-width", sizeVal);
                        break;
                }
            }

            return sb.ToString();
        }
        finally
        {
            sb.Dispose();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void AppendStyle(ref bool first, ref PooledStringBuilder sb, string prop, string val)
    {
        if (!first) sb.Append("; ");
        else first = false;

        sb.Append(prop);
        sb.Append(": ");
        sb.Append(val);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetSizeToken(int size)
    {
        return size switch
        {
            0 => _token0,
            1 => _token1,
            2 => _token2,
            3 => _token3,
            4 => _token4,
            5 => _token5,
            _ => string.Empty
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetSideToken(ElementSide side)
    {
        switch (side)
        {
            case ElementSide.AllValue:
                return string.Empty;
            case ElementSide.TopValue:
                return _sideT;
            case ElementSide.RightValue:
                return _sideE;
            case ElementSide.BottomValue:
                return _sideB;
            case ElementSide.LeftValue:
                return _sideS;
            case ElementSide.HorizontalValue:
            case ElementSide.LeftRightValue:
                return _sideX;
            case ElementSide.VerticalValue:
            case ElementSide.TopBottomValue:
                return _sideY;
            case ElementSide.InlineStartValue:
                return _sideS;
            case ElementSide.InlineEndValue:
                return _sideE;
            default:
                return string.Empty;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string? GetSizeValue(int size)
    {
        return size switch
        {
            0 => "0",
            1 => "1px",
            2 => "2px",
            3 => "3px",
            4 => "4px",
            5 => "5px",
            _ => null
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetBreakpointToken(Breakpoint? breakpoint)
    {
        if (breakpoint is null)
            return string.Empty;

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