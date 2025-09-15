using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Gap;

/// <summary>
/// Represents a single gap rule with optional breakpoint.
/// </summary>
internal record GapRule(int Size, Breakpoint? Breakpoint = null);