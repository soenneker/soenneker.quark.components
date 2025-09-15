using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.FontWeight;

internal readonly record struct FontWeightRule(string Value, Breakpoint? Breakpoint);