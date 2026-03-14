using System;
using System.Linq;
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

    // Whether this tile only has one possible value remaining
    public bool IsSafe {
        get {
            return !label0.Visible;
        }
    }

    // Whether this tile's value has been determined
    public bool IsSolved {
        get {
            return (new[] { label0.Visible, label1.Visible, label2.Visible, label3.Visible }.Count(x => x) == 1);
        }
    }

    public int GetMult() {
        if (!IsSafe) {
            throw new InvalidOperationException("Cannot get multiplier of a tile that is not safe");
        }
        if (!IsSolved) {
            throw new InvalidOperationException("Cannot get multiplier of a tile that is not solved");
        }
        if (label1.Visible) {
            return 1;
        } else if (label2.Visible) {
            return 2;
        } else if (label3.Visible) {
            return 3;
        } else {
            throw new InvalidOperationException("Invalid state for FlipTile: no labels are visible");
        }
    }

    public void SetValue(int value) {
        switch (value) {
            case 0:
                label0.Visible = true;
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                break;
            case 1:
                label0.Visible = false;
                label1.Visible = true;
                label2.Visible = false;
                label3.Visible = false;
                break;
            case 2:
                label0.Visible = false;
                label1.Visible = false;
                label2.Visible = true;
                label3.Visible = false;
                break;
            case 3:
                label0.Visible = false;
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = true;
                break;
            default:
                throw new ArgumentException("Invalid value for FlipTile: " + value);
        }
    }

    public void Reset() {
        label0.Visible = true;
        label1.Visible = true;
        label2.Visible = true;
        label3.Visible = true;
    }
}
