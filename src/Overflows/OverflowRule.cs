using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Overflows;

/// <summary>
/// Represents a single overflow rule with optional breakpoint.
/// </summary>
internal record OverflowRule(string Overflow, Breakpoint? Breakpoint = null);