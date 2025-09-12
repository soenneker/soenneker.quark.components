namespace Soenneker.Quark.Components.Display;

/// <summary>
/// Simplified display utility with fluent API and Bootstrap-first approach.
/// </summary>
public static class Display
{
    /// <summary>
    /// Display none (hidden).
    /// </summary>
    public static DisplayBuilder None => new("none");

    /// <summary>
    /// Display inline.
    /// </summary>
    public static DisplayBuilder Inline => new("inline");

    /// <summary>
    /// Display inline-block.
    /// </summary>
    public static DisplayBuilder InlineBlock => new("inline-block");

    /// <summary>
    /// Display block.
    /// </summary>
    public static DisplayBuilder Block => new("block");

    /// <summary>
    /// Display flex.
    /// </summary>
    public static DisplayBuilder Flex => new("flex");

    /// <summary>
    /// Display inline-flex.
    /// </summary>
    public static DisplayBuilder InlineFlex => new("inline-flex");

    /// <summary>
    /// Display grid.
    /// </summary>
    public static DisplayBuilder Grid => new("grid");

    /// <summary>
    /// Display inline-grid.
    /// </summary>
    public static DisplayBuilder InlineGrid => new("inline-grid");

    /// <summary>
    /// Display table.
    /// </summary>
    public static DisplayBuilder Table => new("table");

    /// <summary>
    /// Display table-cell.
    /// </summary>
    public static DisplayBuilder TableCell => new("table-cell");

    /// <summary>
    /// Display table-row.
    /// </summary>
    public static DisplayBuilder TableRow => new("table-row");
}
