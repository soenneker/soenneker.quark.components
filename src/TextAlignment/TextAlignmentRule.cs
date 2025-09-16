using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.TextAlignment;

internal readonly record struct TextAlignmentRule(string Value, Breakpoint? Breakpoint);