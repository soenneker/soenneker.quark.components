using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.FontWeights;

internal readonly record struct FontWeightRule(string Value, Breakpoint? Breakpoint);