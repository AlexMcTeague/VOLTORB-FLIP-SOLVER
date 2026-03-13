using Godot;

namespace VoltorbFlipSolver;

[Tool]
public partial class LabelTile : PanelContainer {
    public static PackedScene PairedScene => ResourceLoader.Load<PackedScene>("res://scene/LabelTile.tscn");

    [Export] public OptionButton rowMultDropdown;
    [Export] public OptionButton bombDropdown;
}
