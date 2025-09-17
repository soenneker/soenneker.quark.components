using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Widths;

/// <summary>
/// Represents a single width rule with optional breakpoint.
/// </summary>
internal record WidthRule(string Size, Breakpoint? Breakpoint = null);


