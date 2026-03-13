using Godot;

namespace VoltorbFlipSolver;

public partial class FlipTile : PanelContainer {
    public static PackedScene PairedScene => ResourceLoader.Load<PackedScene>("res://scene/FlipTile.tscn");

    [Export] public Label label0;
    [Export] public Label label1;
    [Export] public Label label2;
    [Export] public Label label3;

    // Called when both the node and its children have entered the scene tree for the first time.
    public override void _Ready() {
		
	}
}
