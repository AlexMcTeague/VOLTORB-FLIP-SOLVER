using Godot;

namespace VoltorbFlipSolver;

public partial class LabelTile : PanelContainer {
    public static PackedScene PairedScene => ResourceLoader.Load<PackedScene>("res://scene/LabelTile.tscn");

    [Export] public OptionButton rowMultDropdown;
    [Export] public OptionButton bombDropdown;

    // Called when both the node and its children have entered the scene tree for the first time.
    public override void _Ready() {

	}
}
