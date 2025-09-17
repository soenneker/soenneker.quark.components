using Microsoft.AspNetCore.Components;

namespace Soenneker.Quark.Components.Abstract;

public interface IElement : IComponent
{
    RenderFragment? ChildContent { get; set; }
}