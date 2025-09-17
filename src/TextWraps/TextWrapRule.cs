using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.TextWraps;

internal readonly record struct TextWrapRule(string Value, Breakpoint? Breakpoint);