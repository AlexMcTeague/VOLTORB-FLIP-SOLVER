using System;
using Godot;

namespace VoltorbFlipSolver;

[Tool]
public partial class FlipTile : PanelContainer {
    public static PackedScene PairedScene => ResourceLoader.Load<PackedScene>("res://scene/FlipTile.tscn");

    [Export] public Label label0;
    [Export] public Label label1;
    [Export] public Label label2;
    [Export] public Label label3;

    public Vector2I position; // (row, column) zero-based

    public void Reset() {
        label0.Visible = true;
        label1.Visible = true;
        label2.Visible = true;
        label3.Visible = true;
    }
}
