using System.Collections;
using System.Drawing;
using System.Text;

public class Puzzle : IComparable<Puzzle> {
    public Square[,] Board { get; set; }

    public Puzzle()
    {
        Board = new Square[9,9];
    }

    public Puzzle(int?[] finalValues) {
        int x = 0;
        int y = 0;

        Board = new Square[9,9];

        foreach(int? finalValue in finalValues) {
            Board[x, y] = new Square(finalValue);
            x++;

            if (x == 9) {
                x = 0;
                y++;
                if (y == 9) {
                    return;
                }
            }
        }
    }

    public void GenerateSolutionBoard() {
        Random random = new Random();
        bool found = false;

        while(!found) {
            found = true;

            for (int x = 0; x < 9; x++) {
                for (int y = 0; y < 9; y++) {
                    Board[x, y] = new Square();
                }
            }

            for(int x = 0; x < 9; x++) {
                for(int y = 0; y < 9; y++) {
                    if (Board[x, y].PossibleValues.Count == 0) {
                        x = 9;
                        y = 9;
                        found = false;
                    } else {
                        Board[x, y].FinalValue = Board[x, y].PossibleValues[(int)(random.NextInt64(Board[x, y].PossibleValues.Count))];

                        // Remove possible values of rest of row/column
                        for(int i = 0; i < 9; i++) {
                            if (x != i) {
                                Board[i, y].PossibleValues.Remove((int)Board[x, y].FinalValue!);
                            }

                            if (y != i) {
                                Board[x, i].PossibleValues.Remove((int)Board[x, y].FinalValue!);
                            }
                        }

                        // Remove possible values in rest of square
                        int squareX = (int)(x / 3);
                        int squareY = (int)(y / 3);

                        for(int i = 3 * squareX; i < 3 * (squareX + 1); i++) {
                            for(int j = 3 * squareY; j < 3 * (squareY + 1); j++) {
                                if (x != i && y != j) {
                                    Board[i, j].PossibleValues.Remove((int)Board[x, y].FinalValue!);
                                }
                            }    
                        }                
                    }
                }
            }
        } 
    }

    public Puzzle GeneratePuzzleBoard(int missingTarget) {
        Puzzle puzzle = new Puzzle(ToArray());
        int currentMissing = 0;
        Random random = new Random();
        List<Point> attemptedTargets = new List<Point>();

        while(currentMissing < missingTarget) {
            int x = random.Next(9);
            int y = random.Next(9);
            Point testPoint = new Point(x, y);

            if (attemptedTargets.Count == 81) {
                return puzzle;
            }

            while(attemptedTargets.Contains(testPoint)) {
                x = random.Next(9);
                y = random.Next(9);
                testPoint = new Point(x, y);
            }

            attemptedTargets.Add(testPoint);

            if (puzzle.Board[x, y].FinalValue is not null) {
                puzzle.Board[x, y].FinalValue = null;

                Solver solver = new Solver(puzzle);
                List<Puzzle> Attempts = new List<Puzzle>();
                List<Puzzle> Solutions = new List<Puzzle>();
                
                solver.Solve(Solutions, Attempts, false, true);

                if (Solutions.Count == 1) {
                    currentMissing++;
                } else {
                    puzzle.Board[x, y].FinalValue = Board[x, y].FinalValue;
                }
            }
        }

        return puzzle;        
    }

    public string DisplayBoard() {
        StringBuilder board = new StringBuilder();
        board.AppendLine("+-+-+-+-+-+-+-+-+-+");
        for(int y = 0; y < 9; y++) {
            for(int x = 0; x < 9; x++) {
                board.Append("+");
                board.Append(Board[x, y].FinalValue == null ? " " : Board[x, y].FinalValue);
            }

            board.AppendLine("+");
        }
        board.AppendLine("+-+-+-+-+-+-+-+-+-+");
        
        return board.ToString();
    }

    public string DisplayPossibleBoard() {
        StringBuilder board = new StringBuilder();
        board.AppendLine("+------+------+------+------+------+------+------+------+------+");
        for(int y = 0; y < 9; y++) {
            for(int row = 0; row < 3; row++) {
                for(int x = 0; x < 9; x++) {                
                    board.Append("|");
                    for(int column = 1; column <= 3; column++) {
                        if(Board[x, y].PossibleValues.Contains(row * 3 + column)){
                            board.Append((row * 3 + column).ToString());
                            board.Append("-");
                        } else {
                            board.Append(" ");
                            board.Append("-");
                        }
                    }                    
                }
                board.AppendLine("|");                                    
            }
            board.AppendLine("+------+------+------+------+------+------+------+------+------+");
        }        
        
        return board.ToString();
    }

    public int?[] ToArray() {
        int?[] values = new int?[81];

        for(int y = 0; y < 9; y++) {
            for(int x = 0; x < 9; x++) {
                values[y * 9 + x] = Board[x, y].FinalValue;
            }
        }

        return values;
    }

    public int CompareTo(Puzzle? other) {
        if (other is null) {
            return -1;
        }
        
        try {
            for(int y = 0; y < 9; y++) {
                for(int x = 0; x < 9; x++) {
                    if (this.Board[x, y].FinalValue != other.Board[x, y].FinalValue) {
                        return -1;
                    }
                }
            }
        } 
        catch {
            return -1; 
        }

        return 1;
    }
}

