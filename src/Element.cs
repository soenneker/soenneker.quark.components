using Microsoft.AspNetCore.Components;
using Soenneker.Quark.Components.Abstract;

namespace Soenneker.Quark.Components;

///<inheritdoc cref="IElement"/>
public abstract class Element : Component, IElement
{
    [Parameter] 
    public RenderFragment? ChildContent { get; set; }
}