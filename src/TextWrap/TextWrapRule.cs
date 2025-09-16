using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.TextWrap;

internal readonly record struct TextWrapRule(string Value, Breakpoint? Breakpoint);