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
                flipTile.position = new Vector2I(i, j);
                board.AddChild(flipTile);
                flipTiles.Add(flipTile);
            }
            var labelTile = LabelTile.PairedScene.Instantiate<LabelTile>();
            labelTile.labelIndex = i;
            labelTile.isRow = true;
            board.AddChild(labelTile);
            labelTiles.Add(labelTile);
        }

        for (int i = 0; i < 5; i++) {
            var labelTile = LabelTile.PairedScene.Instantiate<LabelTile>();
            labelTile.labelIndex = i;
            labelTile.isRow = false;
            board.AddChild(labelTile);
            labelTiles.Add(labelTile);
        }
    }

    public void Randomize() {
        var rand = new RandomNumberGenerator();
        rand.Randomize();

        foreach (var labelTile in labelTiles) {
            int bombCount = rand.RandiRange(0, 5);
            int multCount = 5 - bombCount;
            int multTotal = 0;

            for (int i = 1; i <= multCount; i++) {
                float randomFloat = rand.Randf();
                switch (randomFloat) {
                    case < 0.55f:
                        multTotal += 1;
                        break;
                    case < 0.85f:
                        multTotal += 2;
                        break;
                    default:
                        multTotal += 3;
                        break;
                }
            }
                
            labelTile.rowMultDropdown.Selected = multTotal;
            labelTile.bombDropdown.Selected = bombCount;
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
