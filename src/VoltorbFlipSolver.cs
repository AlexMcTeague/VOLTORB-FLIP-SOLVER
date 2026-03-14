using System.Collections.Generic;
using Godot;

namespace VoltorbFlipSolver;

[Tool]
public partial class VoltorbFlipSolver : Node2D {
	[Export] public GridContainer board;
    [Export] public Label messageLabel;

    public Dictionary<Vector2I, FlipTile> flipTileDict = new Dictionary<Vector2I, FlipTile>();
    public List<LabelTile> labelTiles = new List<LabelTile>();

    public int SafeTileCount {
        get {
            int count = 0;
            foreach (var flipTile in flipTileDict.Values) {
                if (flipTile.IsSafe) {
                    count++;
                }
            }
            return count;
        }
    }

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
                flipTileDict.Add(flipTile.position, flipTile);
            }
            var labelTile = LabelTile.PairedScene.Instantiate<LabelTile>();
            labelTile.labelIndex = i;
            labelTile.isRow = true;
            AssociateTiles(labelTile);
            board.AddChild(labelTile);
            labelTiles.Add(labelTile);
        }

        for (int i = 0; i < 5; i++) {
            var labelTile = LabelTile.PairedScene.Instantiate<LabelTile>();
            labelTile.labelIndex = i;
            labelTile.isRow = false;
            AssociateTiles(labelTile);
            board.AddChild(labelTile);
            labelTiles.Add(labelTile);
        }
    }

    public void AssociateTiles(LabelTile labelTile) {
        for (int i = 0; i < 5; i++) {
            if (labelTile.isRow) {
                labelTile.flipTiles.Add(flipTileDict[new Vector2I(labelTile.labelIndex, i)]);
            } else {
                labelTile.flipTiles.Add(flipTileDict[new Vector2I(i, labelTile.labelIndex)]);
            }
        }
    }

    public void Randomize() {
        messageLabel.Text = "";
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

    public void Solve() {
        messageLabel.Text = "";
        int prevSafeTiles = -1;

        while (SafeTileCount != prevSafeTiles) {
            prevSafeTiles = SafeTileCount;

            foreach (LabelTile labelTile in labelTiles) {
                if (labelTile.rowMultDropdown.Selected > ((5 - labelTile.bombDropdown.Selected) * 3)
                    || (labelTile.rowMultDropdown.Selected + labelTile.bombDropdown.Selected) < 5
                ) {
                    string rowOrCol = labelTile.isRow ? "Row " : "Column ";
                    messageLabel.Text = "Error: Impossible multiplier/bomb combination on " + rowOrCol + " " + (labelTile.labelIndex + 1);
                    return;
                }

                int confirmedBombsCount = 0;
                List<FlipTile> possibleBombs = new List<FlipTile>();
                List<FlipTile> unsolvedTiles = new List<FlipTile>();
                // TODO: Condense labelTile.bombDropdown.Selected into expectedBombs
                // TODO: Replace all the "5" magic numbers with a property (or two) to define how many rows/columns are in the grid
                int unsolvedSafeTileCount = 5 - labelTile.bombDropdown.Selected;
                int remainingMult = labelTile.rowMultDropdown.Selected;

                        foreach (FlipTile flipTile in labelTile.flipTiles) {
                    if (!flipTile.IsSafe) {
                        possibleBombs.Add(flipTile);
                        }

                            if (flipTile.IsSolved) {
                                if (flipTile.IsSafe) {
                                    remainingMult -= flipTile.GetMult();
                            unsolvedSafeTileCount--;
                                } else {
                            confirmedBombsCount++;
                                }
                            } else {
                                unsolvedTiles.Add(flipTile);
                            }
                        }

                // If the number of tiles where bombs are *possible* is equal to the number of expected bombs, we can confirm those tiles are in fact bombs
                // The other tiles in that row can't be bombs
                if (possibleBombs.Count == labelTile.bombDropdown.Selected) {
                    foreach (FlipTile flipTile in unsolvedTiles) {
                        if (possibleBombs.Contains(flipTile)) {
                            flipTile.SetValue(0);
                        } else {
                            flipTile.label0.Visible = false;
                        }
                    }
                }

                // If the number of confirmed bombs is equal to the number of expected bombs, any unconfirmed bombs can be marked safe
                // Note: This will also mark all tiles in the row safe if the expected bomb count is 0
                if (confirmedBombsCount == labelTile.bombDropdown.Selected) {
                    foreach (FlipTile flipTile in unsolvedTiles) {
                                    flipTile.label0.Visible = false;
                                }
                            }

                // Calculate the remaining multiplier total in this row, excluding solved safe tiles, and bombs
                if (remainingMult < unsolvedSafeTileCount + 2) {
                    foreach (FlipTile flipTile in unsolvedTiles) {
                        flipTile.label3.Visible = false;
                        }
                }
                if (remainingMult < unsolvedSafeTileCount + 1) {
                            foreach (FlipTile flipTile in unsolvedTiles) {
                        flipTile.label2.Visible = false;
                    }
                }
                            }
                        }
                        
        // After comprehensively checking for changes, now we can update the display
                }
            }

    public void Restart() {
        messageLabel.Text = "";
        foreach (FlipTile fliptile in flipTileDict.Values) {
            fliptile.Reset();
        }
    }

    public void Reset() {
        messageLabel.Text = "";
        Restart();
        foreach (LabelTile labelTile in labelTiles) {
            labelTile.Reset();
        }
    }
}
