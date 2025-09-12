using Soenneker.Quark.Enums.Breakpoints;
using Soenneker.Quark.Enums.ElementSides;

namespace Soenneker.Quark.Components.Margin;

/// <summary>
/// Represents a single margin rule with optional breakpoint.
/// </summary>
internal record MarginRule(int Size, ElementSide Side, Breakpoint? Breakpoint = null);
