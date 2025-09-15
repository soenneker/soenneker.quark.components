using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Soenneker.Blazor.Extensions.EventCallback;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Common;
using Soenneker.Quark.Components.Height;
using Soenneker.Quark.Components.Margin;
using Soenneker.Quark.Components.ObjectFit;
using Soenneker.Quark.Components.Overflow;
using Soenneker.Quark.Components.Padding;
using Soenneker.Quark.Components.Position;
using Soenneker.Quark.Components.TextSize;
using Soenneker.Quark.Components.Width;
using Soenneker.Quark.Dtos.Colors;
using Soenneker.Quark.Enums.DisplayTypes;
using Soenneker.Quark.Enums.Floats;
using Soenneker.Quark.Enums.Shadows;
using Soenneker.Quark.Enums.TextAlignments;
using Soenneker.Quark.Enums.TextDecorations.Line;
using Soenneker.Quark.Enums.VerticalAligns;
using Soenneker.Quark.Enums.Visibilities;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark.Components;

///<inheritdoc cref="Abstract.IComponent"/>
public abstract class Component : ComponentBase, Abstract.IComponent
{
    private bool _disposed;
    private bool _asyncDisposed;

    [Parameter]
    public string? Id { get; set; }

    [Parameter]
    public string? Class { get; set; }

    [Parameter]
    public string? Style { get; set; }

    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public int? TabIndex { get; set; }

    [Parameter]
    public bool Hidden { get; set; }

    [Parameter]
    public DisplayType? Display { get; set; }

    [Parameter]
    public Visibility? Visibility { get; set; }

    [Parameter]
    public Float? Float { get; set; }

    [Parameter]
    public VerticalAlign? VerticalAlign { get; set; }

    [Parameter]
    public Enums.TextOverflows.TextOverflow? TextOverflow { get; set; }

    [Parameter]
    public Shadow? BoxShadow { get; set; }

    [Parameter]
    public CssValue<MarginBuilder>? Margin { get; set; }

    [Parameter]
    public CssValue<PaddingBuilder>? Padding { get; set; }

    [Parameter]
    public CssValue<PositionBuilder>? Position { get; set; }

    [Parameter]
    public CssValue<TextSizeBuilder>? TextSize { get; set; }

    [Parameter]
    public CssValue<WidthBuilder>? Width { get; set; }

    [Parameter]
    public CssValue<HeightBuilder>? Height { get; set; }

    [Parameter]
    public CssValue<OverflowBuilder>? Overflow { get; set; }

    [Parameter]
    public CssValue<ObjectFitBuilder>? ObjectFit { get; set; }

    [Parameter]
    public TextAlignment? TextAlignment { get; set; }

    [Parameter]
    public TextDecorationLine? TextDecorationLine { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnDoubleClick { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnMouseOver { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnMouseOut { get; set; }

    [Parameter]
    public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; }

    [Parameter]
    public EventCallback<FocusEventArgs> OnFocus { get; set; }

    [Parameter]
    public EventCallback<FocusEventArgs> OnBlur { get; set; }

    [Parameter]
    public EventCallback<ElementReference> OnElementRefReady { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? Attributes { get; set; }

    [Parameter]
    public Color TextColor { get; set; }

    [Parameter]
    public string? Role { get; set; }

    [Parameter]
    public string? AriaLabel { get; set; }

    [Parameter]
    public string? AriaDescribedBy { get; set; }

    protected ElementReference ElementRef { get; set; }

    protected bool Disposed => _disposed;

    protected bool AsyncDisposed => _asyncDisposed;

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && OnElementRefReady.HasDelegate)
            return OnElementRefReady.InvokeAsync(ElementRef);

        return Task.CompletedTask;
    }

    protected virtual Dictionary<string, object> BuildAttributes()
    {
        int guess = 14 + (Attributes?.Count ?? 0);
        var attrs = new Dictionary<string, object>(guess);

        // NOT 'using var' – we need to pass by ref into helpers.
        var cls = new PooledStringBuilder(64);
        var sty = new PooledStringBuilder(128);

        try
        {
            if (!Class.IsNullOrEmpty()) cls.Append(Class!);
            if (!Style.IsNullOrEmpty()) sty.Append(Style!);

            // Simple/static attributes
            if (!Id.IsNullOrEmpty()) attrs["id"] = Id!;
            if (!Title.IsNullOrEmpty()) attrs["title"] = Title!;
            if (TabIndex.HasValue) attrs["tabindex"] = TabIndex.Value;
            if (Hidden) attrs["hidden"] = true;
            if (!Role.IsNullOrEmpty()) attrs["role"] = Role!;
            if (!AriaLabel.IsNullOrEmpty()) attrs["aria-label"] = AriaLabel!;
            if (!AriaDescribedBy.IsNullOrEmpty()) attrs["aria-describedby"] = AriaDescribedBy!;

            // Inline style enums
            if (Display != null) AppendStyleDecl(ref sty, "display: ", Display.Value);
            if (Visibility != null) AppendStyleDecl(ref sty, "visibility: ", Visibility.Value);
            if (Float != null) AppendStyleDecl(ref sty, "float: ", Float.Value);
            if (VerticalAlign != null) AppendStyleDecl(ref sty, "vertical-align: ", VerticalAlign.Value);
            if (TextOverflow != null) AppendStyleDecl(ref sty, "text-overflow: ", TextOverflow.Value);
            if (BoxShadow != null) AppendStyleDecl(ref sty, "box-shadow: ", BoxShadow.Value);
            if (TextAlignment != null) AppendStyleDecl(ref sty, "text-align: ", TextAlignment.Value);
            if (TextDecorationLine != null) AppendStyleDecl(ref sty, "text-decoration-line: ", TextDecorationLine.Value);

            // CssValue<> properties
            AddCss(ref sty, ref cls, Margin);
            AddCss(ref sty, ref cls, Padding);
            AddCss(ref sty, ref cls, Position);
            AddCss(ref sty, ref cls, TextSize);
            AddCss(ref sty, ref cls, Width);
            AddCss(ref sty, ref cls, Height);
            AddCss(ref sty, ref cls, Overflow);
            AddCss(ref sty, ref cls, ObjectFit);

            // Events — assign callbacks directly (no Factory.Create allocations)
            if (OnClick.HasDelegate) attrs["onclick"] = OnClick;
            if (OnDoubleClick.HasDelegate) attrs["ondblclick"] = OnDoubleClick;
            if (OnMouseOver.HasDelegate) attrs["onmouseover"] = OnMouseOver;
            if (OnMouseOut.HasDelegate) attrs["onmouseout"] = OnMouseOut;
            if (OnKeyDown.HasDelegate) attrs["onkeydown"] = OnKeyDown;
            if (OnFocus.HasDelegate) attrs["onfocus"] = OnFocus;
            if (OnBlur.HasDelegate) attrs["onblur"] = OnBlur;

            if (cls.Length > 0) attrs["class"] = cls.ToString();
            if (sty.Length > 0) attrs["style"] = sty.ToString();

            // Merge user attributes last (allow overrides)
            if (Attributes != null)
            {
                foreach (KeyValuePair<string, object> kv in Attributes)
                    attrs[kv.Key] = kv.Value;
            }

            return attrs;
        }
        finally
        {
            sty.Dispose();
            cls.Dispose();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void AppendClass(ref PooledStringBuilder b, string s)
    {
        if (s.IsNullOrEmpty()) 
            return;

        if (b.Length != 0)
            b.Append(' ');

        b.Append(s);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void AppendStyleDecl(ref PooledStringBuilder b, string nameColonSpace, object value)
    {
        if (b.Length != 0)
        {
            b.Append(';');
            b.Append(' ');
        }

        b.Append(nameColonSpace);
        b.Append(value.ToString()!);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void AppendStyleDecl(ref PooledStringBuilder b, string fullDecl)
    {
        if (fullDecl.IsNullOrEmpty()) 
            return;

        if (b.Length != 0)
        {
            b.Append(';');
            b.Append(' ');
        }

        b.Append(fullDecl);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void AddCss<T>(ref PooledStringBuilder styB, ref PooledStringBuilder clsB, CssValue<T>? v) where T : class, ICssBuilder
    {
        if (v is { IsEmpty: false })
        {
            var s = v.Value.ToString();

            if (s.Length == 0)
                return;

            if (v.Value.IsCssStyle) 
                AppendStyleDecl(ref styB, s);
            else 
                AppendClass(ref clsB, s);
        }
    }

    protected virtual Task HandleClick(MouseEventArgs e) => OnClick.InvokeIfHasDelegate(e);
    protected virtual Task HandleDoubleClick(MouseEventArgs e) => OnDoubleClick.InvokeIfHasDelegate(e);
    protected virtual Task HandleMouseOver(MouseEventArgs e) => OnMouseOver.InvokeIfHasDelegate(e);
    protected virtual Task HandleMouseOut(MouseEventArgs e) => OnMouseOut.InvokeIfHasDelegate(e);
    protected virtual Task HandleKeyDown(KeyboardEventArgs e) => OnKeyDown.InvokeIfHasDelegate(e);
    protected virtual Task HandleFocus(FocusEventArgs e) => OnFocus.InvokeIfHasDelegate(e);
    protected virtual Task HandleBlur(FocusEventArgs e) => OnBlur.InvokeIfHasDelegate(e);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(true);
        Dispose(false);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            OnDispose();
            _disposed = true;
        }
    }

    protected virtual async ValueTask DisposeAsync(bool disposing)
    {
        if (!_asyncDisposed && disposing)
        {
            await OnDisposeAsync();
            _asyncDisposed = true;
        }
    }

    protected virtual void OnDispose()
    {
    }

    protected virtual Task OnDisposeAsync() => Task.CompletedTask;
}