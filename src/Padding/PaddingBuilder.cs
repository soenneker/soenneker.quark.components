using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Utils.PooledStringBuilders;
using Soenneker.Quark.Components.Abstract;
using Soenneker.Quark.Enums.Breakpoints;
using Soenneker.Quark.Enums.ElementSides;
using Soenneker.Quark.Enums.Size;
using Soenneker.Quark.Components.Utilities;

namespace Soenneker.Quark.Components.Padding;

/// <summary>
/// High-performance padding builder with fluent API for chaining padding rules.
/// </summary>
public sealed class PaddingBuilder : ICssBuilder
{
    private readonly List<PaddingRule> _rules = new(4);

    // ----- Class tokens -----
    private const string _baseToken = "p";

    // ----- Size tokens -----
    private const string _token0 = "0";
    private const string _token1 = "1";
    private const string _token2 = "2";
    private const string _token3 = "3";
    private const string _token4 = "4";
    private const string _token5 = "5";
    private const string _tokenAuto = "auto";

    // ----- Side tokens (Bootstrap naming) -----
    private const string _sideT = "t";
    private const string _sideE = "e";
    private const string _sideB = "b";
    private const string _sideS = "s";
    private const string _sideX = "x";
    private const string _sideY = "y";

    internal PaddingBuilder(int size, Breakpoint? breakpoint = null)
    {
        // negative size = auto (kept for parity with your original)
        int normalized = size >= 0 ? size : -1;
        _rules.Add(new PaddingRule(normalized, ElementSide.All, breakpoint));
    }

    internal PaddingBuilder(List<PaddingRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    // ----- Side chaining -----
    public PaddingBuilder FromTop => AddRule(ElementSide.Top);
    public PaddingBuilder FromRight => AddRule(ElementSide.Right);
    public PaddingBuilder FromBottom => AddRule(ElementSide.Bottom);
    public PaddingBuilder FromLeft => AddRule(ElementSide.Left);
    public PaddingBuilder OnX => AddRule(ElementSide.Horizontal);
    public PaddingBuilder OnY => AddRule(ElementSide.Vertical);
    public PaddingBuilder OnAll => AddRule(ElementSide.All);
    public PaddingBuilder FromStart => AddRule(ElementSide.InlineStart);
    public PaddingBuilder FromEnd => AddRule(ElementSide.InlineEnd);

    // ----- Size chaining -----
    public PaddingBuilder S0 => ChainWithSize(0);
    public PaddingBuilder S1 => ChainWithSize(1);
    public PaddingBuilder S2 => ChainWithSize(2);
    public PaddingBuilder S3 => ChainWithSize(3);
    public PaddingBuilder S4 => ChainWithSize(4);
    public PaddingBuilder S5 => ChainWithSize(5);

    // ----- Breakpoint chaining -----
    public PaddingBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);
    public PaddingBuilder OnMobile => ChainWithBreakpoint(Breakpoint.Mobile);
    public PaddingBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);
    public PaddingBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);
    public PaddingBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);
    public PaddingBuilder OnWideScreen => ChainWithBreakpoint(Breakpoint.ExtraExtraLarge);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private PaddingBuilder AddRule(ElementSide side)
    {
        // Use last size & breakpoint if present; default to 0 when absent
        int size = _rules.Count > 0 ? _rules[^1].Size : 0;
        Breakpoint? bp = _rules.Count > 0 ? _rules[^1].Breakpoint : null;

        if (_rules.Count > 0 && _rules[^1].Side == ElementSide.All)
        {
            // Replace last "All" with specific side using same size/bp
            _rules[^1] = new PaddingRule(size, side, bp);
        }
        else
        {
            _rules.Add(new PaddingRule(size, side, bp));
        }

        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private PaddingBuilder ChainWithSize(int size)
    {
        _rules.Add(new PaddingRule(size, ElementSide.All, null));
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private PaddingBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new PaddingRule(0, ElementSide.All, breakpoint));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        PaddingRule last = _rules[lastIdx];
        _rules[lastIdx] = new PaddingRule(last.Size, last.Side, breakpoint);
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
            PaddingRule rule = _rules[i];

            string sizeTok = GetSizeToken(rule.Size);
            if (sizeTok.Length == 0)
                continue;

            string sideTok = GetSideToken(rule.Side); // "", "t", "e", "b", "s", "x", "y"
            string bpTok = BreakpointUtilities.GetBreakpointToken(rule.Breakpoint); // "", "sm", "md", ...

            if (!first) sb.Append(' ');
            else first = false;

            // Build: p{side?}-{bp?}-{size}
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
                PaddingRule rule = _rules[i];
                string? sizeVal = GetSizeValue(rule.Size);
                if (sizeVal is null)
                    continue;

                switch (rule.Side)
                {
                    case ElementSide.AllValue:
                        AppendStyle(ref first, ref sb, "padding", sizeVal);
                        break;

                    case ElementSide.TopValue:
                        AppendStyle(ref first, ref sb, "padding-top", sizeVal);
                        break;

                    case ElementSide.RightValue:
                        AppendStyle(ref first, ref sb, "padding-right", sizeVal);
                        break;

                    case ElementSide.BottomValue:
                        AppendStyle(ref first, ref sb, "padding-bottom", sizeVal);
                        break;

                    case ElementSide.LeftValue:
                        AppendStyle(ref first, ref sb, "padding-left", sizeVal);
                        break;

                    case ElementSide.HorizontalValue:
                    case ElementSide.LeftRightValue:
                        AppendStyle(ref first, ref sb, "padding-left", sizeVal);
                        AppendStyle(ref first, ref sb, "padding-right", sizeVal);
                        break;

                    case ElementSide.VerticalValue:
                    case ElementSide.TopBottomValue:
                        AppendStyle(ref first, ref sb, "padding-top", sizeVal);
                        AppendStyle(ref first, ref sb, "padding-bottom", sizeVal);
                        break;

                    case ElementSide.InlineStartValue:
                        AppendStyle(ref first, ref sb, "padding-inline-start", sizeVal);
                        break;

                    case ElementSide.InlineEndValue:
                        AppendStyle(ref first, ref sb, "padding-inline-end", sizeVal);
                        break;

                    default:
                        // Fallback like "all"
                        AppendStyle(ref first, ref sb, "padding", sizeVal);
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
            -1 => _tokenAuto, // "auto"
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
                return _sideS; // Bootstrap uses "s" for start
            case ElementSide.InlineEndValue:
                return _sideE; // Bootstrap uses "e" for end
            default:
                return string.Empty;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string? GetSizeValue(int size)
    {
        // Match your original rem scale and "auto"
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

}