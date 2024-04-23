using System;

namespace ConnectFourGame
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectFour connectFourGame = new ConnectFour();
            connectFourGame.Run();
        }
    }

    class ConnectFour
    {
        private GameBoard board;
        private GamePlayer firstPlayer; // First player = X
        private GamePlayer secondPlayer; // Second player = O
        private GamePlayer activePlayer;

        public ConnectFour()
        {
            Console.WriteLine("Welcome to Connect Four!");
            Console.WriteLine("Choose game mode:");
            Console.WriteLine("1. Play against Computer");
            Console.WriteLine("2. Play against another player");
            Console.Write("Enter your choice: ");
            int mode = int.Parse(Console.ReadLine());

            if (mode == 1)
            {
                firstPlayer = new HumanPlayer('X');
                secondPlayer = new ComputerPlayer('O');
            }
            else if (mode == 2)
            {
                firstPlayer = new HumanPlayer('X');
                secondPlayer = new HumanPlayer('O');
            }
            else
            {
                Console.WriteLine("Invalid choice. Quitting...");
                Environment.Exit(0);
            }

            board = new GameBoard();
            activePlayer = firstPlayer; // Player X starts the game
        }

        public void Run()
        {
            GamePlayer previousPlayer = activePlayer; // Makes the previous player the active player

            while (!board.GameOver())
            {
                Console.Clear(); 
                board.Display(); // Shows the current board state
                Console.WriteLine($"Player {activePlayer.PlayerSymbol}'s turn:");
                activePlayer.MakeMove(board);
                previousPlayer = activePlayer; // Saves the active player before switching it
                activePlayer = (activePlayer == firstPlayer) ? secondPlayer : firstPlayer; // Switches the player
            }

            Console.Clear();
            board.Display();

            // Display the winner based on who the previous player is
            if (board.HasWinner())
            {
                Console.WriteLine($"Player {previousPlayer.PlayerSymbol} wins!"); // Display previous player as the winner
            }
            else
            {
                Console.WriteLine("The game is a draw!");
            }
        }
    }

    class GameBoard
    {
        private char[,] boardGrid = new char[6, 7]; // 6 x 7 gameboard grid

        public GameBoard()
        {
            ResetBoard();
        }

        private void ResetBoard() // This resets the board by filling it with space
        {
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    boardGrid[row, col] = ' ';
                }
            }
        }

        public void Display()
        {
            Console.WriteLine("  1 2 3 4 5 6 7");
            for (int row = 0; row < 6; row++)
            {
                Console.Write("| ");
                for (int col = 0; col < 7; col++)
                {
                    Console.Write(boardGrid[row, col] + " ");
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("---------------");
        }

        public bool PlacePiece(int column, char playerSymbol)
        {
            for (int row = 5; row >= 0; row--) // Places piece from the bottom row upwards
            {
                if (boardGrid[row, column] == ' ') // Checks if the cell is empty
                {
                    boardGrid[row, column] = playerSymbol;
                    return true;
                }
            }
            return false;
        }

        public bool HasWinner()
        {
            // Checks rows
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (boardGrid[row, col] != ' ' &&
                        boardGrid[row, col] == boardGrid[row, col + 1] &&
                        boardGrid[row, col] == boardGrid[row, col + 2] &&
                        boardGrid[row, col] == boardGrid[row, col + 3])
                    {
                        return true;
                    }
                }
            }

            // Checks columns
            for (int col = 0; col < 7; col++)
            {
                for (int row = 0; row < 3; row++)
                {
                    if (boardGrid[row, col] != ' ' &&
                        boardGrid[row, col] == boardGrid[row + 1, col] &&
                        boardGrid[row, col] == boardGrid[row + 2, col] &&
                        boardGrid[row, col] == boardGrid[row + 3, col])
                    {
                        return true;
                    }
                }
            }

            // Checks diagonals
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (boardGrid[row, col] != ' ' &&
                        boardGrid[row, col] == boardGrid[row + 1, col + 1] &&
                        boardGrid[row, col] == boardGrid[row + 2, col + 2] &&
                        boardGrid[row, col] == boardGrid[row + 3, col + 3])
                    {
                        return true;
                    }
                }
            }

            for (int row = 0; row < 3; row++)
            {
                for (int col = 3; col < 7; col++)
                {
                    if (boardGrid[row, col] != ' ' &&
                        boardGrid[row, col] == boardGrid[row + 1, col - 1] &&
                        boardGrid[row, col] == boardGrid[row + 2, col - 2] &&
                        boardGrid[row, col] == boardGrid[row + 3, col - 3])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool GameOver()
        {
            return HasWinner() || BoardIsFull();
        }

        private bool BoardIsFull()
        {
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    if (boardGrid[row, col] == ' ')
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    class GamePlayer
    {
        public char PlayerSymbol { get; protected set; }

        public GamePlayer(char symbol) // Initializes player with a symbol
        {
            PlayerSymbol = symbol;
        }

        public virtual void MakeMove(GameBoard board)
        {
            throw new NotImplementedException();
        }
    }

    class HumanPlayer : GamePlayer
    {
        public HumanPlayer(char symbol) : base(symbol)
        {
        }

        public override void MakeMove(GameBoard board) // The method for the human player to make a move
        {
            bool moveIsValid = false;
            // Loops until a valid move is made
            while (!moveIsValid)
            {
                Console.Write("Choose column (1-7): ");
                int column;
                if (int.TryParse(Console.ReadLine(), out column) && column >= 1 && column <= 7)
                {
                    column--;

                    if (board.PlacePiece(column, PlayerSymbol))
                    {
                        moveIsValid = true;
                    }
                    else
                    {
                        Console.WriteLine("Column is full. Pick another column.");
                    }
                }
                else
                {
                    Console.WriteLine("Enter a number from 1 to 7.");
                }
            }
        }
    }

    class ComputerPlayer : GamePlayer // The method for the computer to make a move
    {
        public ComputerPlayer(char symbol) : base(symbol)
        {
        }

        public override void MakeMove(GameBoard board) // Randomizes the computers move
        {
            Random random = new Random();
            int column;
            do
            {
                column = random.Next(0, 7);
            } while (!board.PlacePiece(column, PlayerSymbol));
        }
    }
}
