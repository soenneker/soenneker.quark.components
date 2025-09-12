using Microsoft.AspNetCore.Components;

namespace Soenneker.Quark.Components;

public abstract class Element : Component
{
    [Parameter] 
    public RenderFragment ChildContent { get; set; }
}