namespace Soenneker.Quark.Components.Abstract;

public interface ILengthBuilder : ICssBuilder
{
    static abstract string CssPropertyName { get; }
}