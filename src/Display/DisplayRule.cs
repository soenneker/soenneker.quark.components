using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Display;

/// <summary>
/// Represents a single display rule with optional breakpoint.
/// </summary>
internal record DisplayRule(string Display, Breakpoint? Breakpoint = null);
