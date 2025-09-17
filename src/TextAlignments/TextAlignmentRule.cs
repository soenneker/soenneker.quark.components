using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.TextAlignments;

internal readonly record struct TextAlignmentRule(string Value, Breakpoint? Breakpoint);