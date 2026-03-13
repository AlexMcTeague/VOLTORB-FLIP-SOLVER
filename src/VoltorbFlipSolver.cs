using System.Collections.Generic;
using Godot;

namespace VoltorbFlipSolver;

[Tool]
public partial class VoltorbFlipSolver : Node2D {
	[Export] public GridContainer board;

	public List<FlipTile> flipTiles = new List<FlipTile>();
    public List<LabelTile> labelTiles = new List<LabelTile>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        // Create the board
        // The board is a 5x5 grid of FlipTiles, with a column of LabelTiles to the right, and a row of LabelTiles below
		// All of these tiles are stored within the same 6-column GridContainer

        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 5; j++) {
                var flipTile = FlipTile.PairedScene.Instantiate<FlipTile>();
                board.AddChild(flipTile);
                flipTiles.Add(flipTile);
            }
            var labelTile = LabelTile.PairedScene.Instantiate<LabelTile>();
            board.AddChild(labelTile);
            labelTiles.Add(labelTile);
        }

        for (int i = 0; i < 5; i++) {
            var labelTile = LabelTile.PairedScene.Instantiate<LabelTile>();
            board.AddChild(labelTile);
            labelTiles.Add(labelTile);
        }
    }

    public void Reset() {
        foreach (var flipTile in flipTiles) {
            flipTile.Reset();
        }
        foreach (var labelTile in labelTiles) {
            labelTile.Reset();
        }
    }
}
