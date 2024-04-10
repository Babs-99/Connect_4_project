using System;

namespace Connect4Game
{
    class Program
    {
        static char[,] board = new char[6, 7]; 
        static bool gameEnded = false;
        static char currentPlayer = 'X'; // Player 'X' always starts the game

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                InitializeBoard();
                gameEnded = false;
                currentPlayer = 'X';

                Console.WriteLine("Welcome to Connect 4!");
                Console.WriteLine("1. Play 2-player mode");
                Console.WriteLine("2. Quit");

                Console.Write("Choose a mode: ");
                int mode = int.Parse(Console.ReadLine());

                if (mode == 1)
                {
                    PlayTwoPlayerMode();
                }
                else if (mode == 2)
                {
                    break; // Exit the game
                }
                else
                {
                    Console.WriteLine("Invalid mode selection. Please try again.");
                }
            }
        }

        static void InitializeBoard()
        {
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    board[row, col] = ' ';
                }
            }
        }

        static void PrintBoard()
        {
            Console.WriteLine("  1 2 3 4 5 6 7");
            for (int row = 0; row < 6; row++)
            {
                Console.Write("| ");
                for (int col = 0; col < 7; col++)
                {
                    Console.Write(board[row, col] + " ");
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("---------------");
        }

        static void PlayTwoPlayerMode()
        {
            while (!gameEnded)
            {
                Console.Clear();
                PrintBoard();
                Console.WriteLine($"Player {currentPlayer}'s turn:");

                Console.Write("Enter column number (1-7): ");
                int column = int.Parse(Console.ReadLine()) - 1;

                if (column < 0 || column > 6)
                {
                    Console.WriteLine("Invalid column number. Please try again.");
                    continue;
                }

                if (!DropPiece(column))
                {
                    Console.WriteLine("Column is full. Please choose another column.");
                    continue;
                }

                if (CheckWin())
                {
                    gameEnded = true;
                    Console.Clear();
                    PrintBoard();
                    Console.WriteLine($"Player {currentPlayer} wins!");
                }
                else if (CheckDraw())
                {
                    gameEnded = true;
                    Console.Clear();
                    PrintBoard();
                    Console.WriteLine("It's a draw!");
                }

                // Switch player
                currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';
            }
        }

        static bool DropPiece(int column)
        {
            for (int row = 5; row >= 0; row--)
            {
                if (board[row, column] == ' ')
                {
                    board[row, column] = currentPlayer;
                    return true;
                }
            }
            return false; // Column is full
        }

        static bool CheckWin()
        {
            // Check rows
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (board[row, col] == currentPlayer &&
                        board[row, col + 1] == currentPlayer &&
                        board[row, col + 2] == currentPlayer &&
                        board[row, col + 3] == currentPlayer)
                    {
                        return true;
                    }
                }
            }

            // Check columns
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    if (board[row, col] == currentPlayer &&
                        board[row + 1, col] == currentPlayer &&
                        board[row + 2, col] == currentPlayer &&
                        board[row + 3, col] == currentPlayer)
                    {
                        return true;
                    }
                }
            }

            // Check diagonals
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (board[row, col] == currentPlayer &&
                        board[row + 1, col + 1] == currentPlayer &&
                        board[row + 2, col + 2] == currentPlayer &&
                        board[row + 3, col + 3] == currentPlayer)
                    {
                        return true;
                    }
                }
            }

            for (int row = 0; row < 3; row++)
            {
                for (int col = 3; col < 7; col++)
                {
                    if (board[row, col] == currentPlayer &&
                        board[row + 1, col - 1] == currentPlayer &&
                        board[row + 2, col - 2] == currentPlayer &&
                        board[row + 3, col - 3] == currentPlayer)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        static bool CheckDraw()
        {
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    if (board[row, col] == ' ')
                    {
                        return false; // Board is not full
                    }
                }
            }
            return true; // Board is full (draw)
        }
    }
}

