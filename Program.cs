// See https://aka.ms/new-console-template for more information
int?[] finalValues = { null, 7, 5, null, 3, null, null, 6, null, 
                       4, null, null, null, null, null, null, null, 8, 
                       null, null, null, null, null, 6, null, 3, null, 
                       null, null, null, null, null, 7, 2, null, 5, 
                       null, null, null, 8, 9, 1, null, null, null, 
                       1, null, 6, 2, null, null, null, null, null, 
                       null, 1, null, 3, null, null, null, null, null, 
                       5, null, null, null, null, null, null, null, 9, 
                       null, 3, null, null, 2, null, 1, 7, null };
//Puzzle puzzle = new Puzzle(finalValues);

Puzzle solution = new Puzzle();
solution.GenerateSolutionBoard();
Puzzle puzzle = solution.GeneratePuzzleBoard(60);

Solver solver = new Solver(puzzle);
List<Puzzle> Attempts = new List<Puzzle>();
List<Puzzle> Solutions = new List<Puzzle>();
solver.Solve(Solutions, Attempts, false);

//int attemptCount = 1;
int solutionCount = 1;

Console.Write(puzzle.DisplayBoard());
Console.WriteLine();

Console.Write(solution.DisplayBoard());
Console.WriteLine();

Console.WriteLine("Attempted: " + Attempts.Count.ToString());
/*foreach(Puzzle attempt in Attempts) {
    Console.WriteLine("Attempt:" + attemptCount++);
    Console.Write(attempt.DisplayBoard());
    Console.WriteLine();
}*/

Console.WriteLine("Solved: " + Solutions.Count.ToString());
foreach(Puzzle solvedPuzzle in Solutions) {
    Console.WriteLine("Solution:" + solutionCount++);
    Console.Write(solvedPuzzle.DisplayBoard());
    Console.WriteLine();
}


