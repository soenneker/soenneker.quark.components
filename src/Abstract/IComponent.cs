using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Soenneker.Quark;

/// <summary>
/// Base component class that serves as the building block for all HTML elements in Quark.
/// </summary>
public interface IComponent : ICoreComponent
{
    /// <summary>
    /// Gets or sets additional CSS class names to be merged into the rendered element's <c>class</c> attribute.
    /// </summary>
    string? Class { get; set; }

    /// <summary>
    /// Gets or sets additional inline CSS declarations to be merged into the element's <c>style</c> attribute.
    /// </summary>
    string? Style { get; set; }

    /// <summary>
    /// Gets or sets the HTML <c>title</c> attribute (commonly used for native tooltips).
    /// </summary>
    string? Title { get; set; }

    /// <summary>
    /// Gets or sets the HTML <c>tabindex</c> value to control keyboard tab order.
    /// </summary>
    int? TabIndex { get; set; }

    /// <summary>
    /// Gets or sets whether the element is hidden using the boolean HTML <c>hidden</c> attribute.
    /// </summary>
    bool Hidden { get; set; }

    /// <summary>
    /// Gets or sets the CSS <c>vertical-align</c> value to apply inline.
    /// </summary>
    CssValue<VerticalAlignBuilder>? VerticalAlign { get; set; }

    /// <summary>
    /// Gets or sets the CSS <c>text-overflow</c> value to apply inline.
    /// </summary>
    CssValue<TextOverflowBuilder>? TextOverflow { get; set; }

    /// <summary>
    /// Gets or sets the CSS <c>box-shadow</c> value to apply inline.
    /// </summary>
    CssValue<BoxShadowBuilder>? BoxShadow { get; set; }

    /// <summary>
    /// 
    /// </summary>
    CssValue<FloatBuilder>? Float { get; set; }

    /// <summary>
    /// 
    /// </summary>
    CssValue<DisplayBuilder>? Display { get; set; }


    /// <summary>
    /// 
    /// </summary>
    CssValue<VisibilityBuilder>? Visibility { get; set; }

    /// <summary>
    /// Gets or sets the margin configuration. Will emit either classes or inline style based on the builder.
    /// </summary>
    CssValue<MarginBuilder>? Margin { get; set; }

    /// <summary>
    /// Gets or sets the padding configuration. Will emit either classes or inline style based on the builder.
    /// </summary>
    CssValue<PaddingBuilder>? Padding { get; set; }

    /// <summary>
    /// Gets or sets the position/layout configuration (e.g., absolute, relative, offsets).
    /// </summary>
    CssValue<PositionBuilder>? Position { get; set; }

    /// <summary>
    /// Gets or sets the text size configuration (e.g., font-size utilities).
    /// </summary>
    CssValue<TextSizeBuilder>? TextSize { get; set; }

    /// <summary>
    /// Gets or sets the width configuration (e.g., fixed, responsive, utility classes).
    /// </summary>
    CssValue<WidthBuilder>? Width { get; set; }

    /// <summary>
    /// Gets or sets the height configuration (e.g., fixed, responsive, utility classes).
    /// </summary>
    CssValue<HeightBuilder>? Height { get; set; }

    /// <summary>
    /// Gets or sets the overflow configuration (e.g., hidden, auto, scroll).
    /// </summary>
    CssValue<OverflowBuilder>? Overflow { get; set; }

    /// <summary>
    /// Gets or sets the object-fit configuration for replaced content (e.g., images, video).
    /// </summary>
    CssValue<ObjectFitBuilder>? ObjectFit { get; set; }

    /// <summary>
    /// Gets or sets the CSS <c>text-align</c> value to apply inline.
    /// </summary>
    CssValue<TextAlignmentBuilder>? TextAlignment { get; set; }

    /// <summary>
    /// Gets or sets the CSS <c>text-decoration-line</c> value to apply inline.
    /// </summary>
    CssValue<TextDecorationBuilder>? TextDecorationLine { get; set; }

    /// <summary>
    /// Invoked when the element is clicked.
    /// </summary>
    EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// Invoked when the element is double-clicked.
    /// </summary>
    EventCallback<MouseEventArgs> OnDoubleClick { get; set; }

    /// <summary>
    /// Invoked when the pointer moves over the element.
    /// </summary>
    EventCallback<MouseEventArgs> OnMouseOver { get; set; }

    /// <summary>
    /// Invoked when the pointer leaves the element.
    /// </summary>
    EventCallback<MouseEventArgs> OnMouseOut { get; set; }

    /// <summary>
    /// Invoked when a key is pressed while the element has focus.
    /// </summary>
    EventCallback<KeyboardEventArgs> OnKeyDown { get; set; }

    /// <summary>
    /// Invoked when the element receives focus.
    /// </summary>
    EventCallback<FocusEventArgs> OnFocus { get; set; }

    /// <summary>
    /// Invoked when the element loses focus.
    /// </summary>
    EventCallback<FocusEventArgs> OnBlur { get; set; }

    /// <summary>
    /// Invoked after the element reference (<see cref="ElementReference"/>) becomes available on first render.
    /// The <see cref="ElementReference"/> is passed to the callback.
    /// </summary>
    EventCallback<ElementReference> OnElementRefReady { get; set; }

    /// <summary>
    /// Gets or sets the text color to apply (implementation-specific mapping to classes or inline style).
    /// </summary>
    CssValue<ColorBuilder>? TextColor { get; set; }

    /// <summary>
    /// Gets or sets the background color to apply (implementation-specific mapping to classes or inline style).
    /// </summary>
    CssValue<ColorBuilder>? BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the ARIA <c>role</c> attribute for accessibility semantics.
    /// </summary>
    string? Role { get; set; }

    /// <summary>
    /// Gets or sets the ARIA <c>aria-label</c> attribute to provide an accessible label.
    /// </summary>
    string? AriaLabel { get; set; }

    /// <summary>
    /// Gets or sets the ARIA <c>aria-describedby</c> attribute to reference descriptive content.
    /// </summary>
    string? AriaDescribedBy { get; set; }
}