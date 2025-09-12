using Soenneker.Quark.Enums.Breakpoints;
using Soenneker.Quark.Enums.ElementSides;

namespace Soenneker.Quark.Components.Padding;

/// <summary>
/// Represents a single padding rule with optional breakpoint.
/// </summary>
internal record PaddingRule(int Size, ElementSide Side, Breakpoint? Breakpoint = null);
