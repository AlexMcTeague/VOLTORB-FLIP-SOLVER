using Godot;

namespace VoltorbFlipSolver;

[Tool]
public partial class FlipTile : PanelContainer {
    public static PackedScene PairedScene => ResourceLoader.Load<PackedScene>("res://scene/FlipTile.tscn");

    [Export] public Label label0;
    [Export] public Label label1;
    [Export] public Label label2;
    [Export] public Label label3;
}
