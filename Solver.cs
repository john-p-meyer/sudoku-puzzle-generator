public class Solver
{

    Puzzle puzzle;

    public Solver(Puzzle puzzle)
    {
        this.puzzle = new Puzzle(puzzle.ToArray());
    }

    public bool Solve(List<Puzzle> Solutions, List<Puzzle> Attempts, bool StopOnFirstSolution = true, bool StopOnSecondSolution = true)
    {
        // Generate List of Possible values
        if (!GeneratePossibleValues())
        {
            return false;
        }

        // Mark Final Values for squares that have 1 Possible value
        if (!MarkSinglePotentials())
        {
            return false;
        }

        //Console.WriteLine(this.DisplayBoard());

        // Check if the puzzle is now solved
        if (IsSolved())
        {
            bool found = false;

            foreach (Puzzle currentSolution in Solutions)
            {
                if (currentSolution.CompareTo(this.puzzle) == 1)
                {
                    found = true;
                }
            }

            if (!found)
            {
                Solutions.Add(this.puzzle);
                return true;
            }
        }
        else
        {
            //Console.WriteLine(this.puzzle.DisplayBoard());
            // Choose first square that has potential and loop through squares and all possible values
            return BruteForceSolutions(Solutions, Attempts, StopOnFirstSolution, StopOnSecondSolution);
        }

        return false;
    }

    private bool GeneratePossibleValues()
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                if (!GeneratePossibleValuesForSquare(x, y))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool GeneratePossibleValuesForSquare(int x, int y)
    {
        this.puzzle.Board[x, y].PossibleValues.Clear();

        if (this.puzzle.Board[x, y].FinalValue is not null)
        {
            this.puzzle.Board[x, y].PossibleValues.Add((int)puzzle.Board[x, y].FinalValue!);
            return true;
        }

        for (int i = 1; i <= 9; i++)
        {
            this.puzzle.Board[x, y].PossibleValues.Add(i);
        }

        // Determine possible values based on column and row
        for (int i = 0; i < 9; i++)
        {
            if (this.puzzle.Board[i, y].FinalValue is not null)
            {
                this.puzzle.Board[x, y].PossibleValues.Remove((int)puzzle.Board[i, y].FinalValue!);
            }

            if (this.puzzle.Board[x, i].FinalValue is not null)
            {
                this.puzzle.Board[x, y].PossibleValues.Remove((int)puzzle.Board[x, i].FinalValue!);
            }
        }

        // Remove possible values in rest of square
        int squareX = (int)(x / 3);
        int squareY = (int)(y / 3);

        for (int i = 3 * squareX; i < 3 * (squareX + 1); i++)
        {
            for (int j = 3 * squareY; j < 3 * (squareY + 1); j++)
            {
                if (x != i && y != j && this.puzzle.Board[i, j].FinalValue is not null)
                {
                    this.puzzle.Board[x, y].PossibleValues.Remove((int)puzzle.Board[i, j].FinalValue!);
                }
            }
        }

        return this.puzzle.Board[x, y].PossibleValues.Count > 0;
    }

    private bool MarkSinglePotentials()
    {
        bool found = true;

        while (found)
        {
            found = false;
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (this.puzzle.Board[x, y].PossibleValues.Count == 1 && this.puzzle.Board[x, y].FinalValue is null)
                    {
                        this.puzzle.Board[x, y].FinalValue = this.puzzle.Board[x, y].PossibleValues[0];
                        found = true;
                    }
                }
            }

            if (!GeneratePossibleValues())
            {
                return false;
            }
        }

        return true;
    }

    public bool IsValid()
    {
        for (int i = 1; i <= 9; i++)
        {

            // Check rows
            for (int y = 0; y < 9; y++)
            {
                int count = 0;

                for (int x = 0; x < 9; x++)
                {
                    if (this.puzzle.Board[x, y].FinalValue is not null)
                    {
                        if ((int)puzzle.Board[x, y].FinalValue! == i)
                        {
                            count++;
                        }
                    }
                }

                if (count > 1)
                {
                    return false;
                }
            }

            // Check columns
            for (int x = 0; x < 9; x++)
            {
                int count = 0;

                for (int y = 0; y < 9; y++)
                {
                    if (this.puzzle.Board[x, y].FinalValue is not null)
                    {
                        if ((int)puzzle.Board[x, y].FinalValue! == i)
                        {
                            count++;
                        }
                    }
                }

                if (count > 1)
                {
                    return false;
                }
            }

            // Check squares
            for (int squareY = 0; squareY < 3; squareY++)
            {
                for (int squareX = 0; squareX < 3; squareX++)
                {
                    int count = 0;
                    for (int y = 3 * squareY; y < 3 * (squareY + 1); y++)
                    {
                        for (int x = 3 * squareX; x < 3 * (squareX + 1); x++)
                        {
                            if (this.puzzle.Board[x, y].FinalValue is not null)
                            {
                                if ((int)puzzle.Board[x, y].FinalValue! == i) {
                                    count++;
                                }
                            }
                        }
                    }
                    if (count > 1) {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public bool IsSolved()
    {
        // Check for blanks
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                if (this.puzzle.Board[x, y].FinalValue is null)
                {
                    return false;
                }
            }
        }

        // Check rows
        for (int y = 0; y < 9; y++)
        {
            int sum = 0;

            for (int x = 0; x < 9; x++)
            {
                if (this.puzzle.Board[x, y].FinalValue is not null)
                {
                    sum += (int)puzzle.Board[x, y].FinalValue!;
                }
            }

            if (sum != 45)
            {
                return false;
            }
        }

        // Check columns
        for (int x = 0; x < 9; x++)
        {
            int sum = 0;

            for (int y = 0; y < 9; y++)
            {
                if (this.puzzle.Board[x, y].FinalValue is not null)
                {
                    sum += (int)puzzle.Board[x, y].FinalValue!;
                }
            }

            if (sum != 45)
            {
                return false;
            }
        }

        return true;
    }

    public bool BruteForceSolutions(List<Puzzle> Solutions, List<Puzzle> Attempts, bool StopOnFirstSolution, bool StopOnSecondSolution)
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                bool solutionFound = this.puzzle.Board[x, y].FinalValue is not null;

                if (this.puzzle.Board[x, y].FinalValue is null)
                {
                    foreach (int possibleValue in this.puzzle.Board[x, y].PossibleValues)
                    {
                        Puzzle testPuzzle = new Puzzle(this.puzzle.ToArray());
                        testPuzzle.Board[x, y].FinalValue = possibleValue;

                        bool puzzleAttempted = false;

                        foreach (Puzzle attemptedPuzzle in Attempts)
                        {
                            if (testPuzzle.CompareTo(attemptedPuzzle) == 1)
                            {
                                puzzleAttempted = true;
                            }
                        }

                        if (!puzzleAttempted)
                        {
                            Solver testPuzzleSolver = new Solver(testPuzzle);

                            //Console.WriteLine(testPuzzle.DisplayBoard());

                            if (testPuzzleSolver.IsValid()) {
                                Attempts.Add(testPuzzle);

                                solutionFound = solutionFound || testPuzzleSolver.Solve(Solutions, Attempts, StopOnFirstSolution, StopOnSecondSolution);

                                if ((solutionFound && StopOnFirstSolution) || (Solutions.Count > 1 && StopOnSecondSolution))
                                {
                                    x = 9;
                                    y = 9;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (!solutionFound)
                    return false;
            }
        }

        return Solutions.Count > 0;
    }

    public string DisplayBoard()
    {
        return this.puzzle.DisplayBoard();
    }

    public string DisplayPossibleBoard()
    {
        return this.puzzle.DisplayPossibleBoard();
    }
}
