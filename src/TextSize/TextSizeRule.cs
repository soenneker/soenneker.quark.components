using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.TextSize;

/// <summary>
/// Represents a single text size rule with optional breakpoint.
/// </summary>
internal record TextSizeRule(string Size, Breakpoint? Breakpoint = null);
