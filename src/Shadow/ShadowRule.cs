using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Shadow;

internal readonly record struct ShadowRule(string Value, Breakpoint? Breakpoint);