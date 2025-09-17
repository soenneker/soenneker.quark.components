using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.BoxShadow;

internal readonly record struct BoxShadowRule(string Value, Breakpoint? Breakpoint);