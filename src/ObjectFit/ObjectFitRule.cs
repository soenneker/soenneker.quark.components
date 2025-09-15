using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.ObjectFit;

/// <summary>
/// Represents a single object-fit rule with optional breakpoint.
/// </summary>
internal record ObjectFitRule(string Fit, Breakpoint? Breakpoint = null);


