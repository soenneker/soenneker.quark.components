using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.LineHeights;

internal readonly record struct LineHeightRule(string Value, Breakpoint? Breakpoint);