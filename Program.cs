using System;

namespace Connect4Game
{
    class Program
    {
        static void Main(string[] args)
        {
            Connect4Game game = new Connect4Game();
            game.Start();
        }
    }

    class Connect4Game
    {
        private Board board;
        private Player player1;
        private Player player2;
        private Player currentPlayer;

        public Connect4Game()
        {
            board = new Board();
            player1 = new HumanPlayer('X');
            player2 = new ComputerPlayer('O');
            currentPlayer = player1; //Player X starts the ga,e
        }

        public void Start()
        {
            while (!board.IsGameOver())
            {
                Console.Clear();
                board.PrintBoard();
                Console.WriteLine($"Player {currentPlayer.Symbol}'s turn:");
                currentPlayer.MakeMove(board);
                currentPlayer = (currentPlayer == player1) ? player2 : player1; // this switches the players
            }

            Console.Clear();
            board.PrintBoard();
            if (board.CheckWin())
            {
                Console.WriteLine($"Player {currentPlayer.Symbol} wins!");
            }
            else
            {
                Console.WriteLine("It's a draw!");
            }
        }
    }

    class Board
    {
        private char[,] board = new char[6, 7];

        public Board()
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    board[row, col] = ' ';
                }
            }
        }

        public void PrintBoard()
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

        public bool DropPiece(int column, char symbol)
        {
            for (int row = 5; row >= 0; row--)
            {
                if (board[row, column] == ' ')
                {
                    board[row, column] = symbol;
                    return true;
                }
            }
            return false; // return when column is full
        }

        public bool CheckWin()
        {
            // This checks the rows
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (board[row, col] != ' ' &&
                        board[row, col] == board[row, col + 1] &&
                        board[row, col] == board[row, col + 2] &&
                        board[row, col] == board[row, col + 3])
                    {
                        return true;
                    }
                }
            }

            // This checks the columns
            for (int col = 0; col < 7; col++)
            {
                for (int row = 0; row < 3; row++)
                {
                    if (board[row, col] != ' ' &&
                        board[row, col] == board[row + 1, col] &&
                        board[row, col] == board[row + 2, col] &&
                        board[row, col] == board[row + 3, col])
                    {
                        return true;
                    }
                }
            }

            // This checks the diagonals
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (board[row, col] != ' ' &&
                        board[row, col] == board[row + 1, col + 1] &&
                        board[row, col] == board[row + 2, col + 2] &&
                        board[row, col] == board[row + 3, col + 3])
                    {
                        return true;
                    }
                }
            }

            for (int row = 0; row < 3; row++)
            {
                for (int col = 3; col < 7; col++)
                {
                    if (board[row, col] != ' ' &&
                        board[row, col] == board[row + 1, col - 1] &&
                        board[row, col] == board[row + 2, col - 2] &&
                        board[row, col] == board[row + 3, col - 3])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsGameOver()
        {
            return CheckWin() || IsBoardFull();
        }

        private bool IsBoardFull()
        {
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    if (board[row, col] == ' ')
                    {
                        return false; // Return if board is full
                    }
                }
            }
            return true; // Return if board is not full which makes it a draw
        }
    }

    class Player
    {
        public char Symbol { get; protected set; }

        public Player(char symbol)
        {
            Symbol = symbol;
        }

        public virtual void MakeMove(Board board)
        {
            throw new NotImplementedException();
        }
    }

    class HumanPlayer : Player
    {
        public HumanPlayer(char symbol) : base(symbol)
        {
        }

        public override void MakeMove(Board board)
        {
            bool validMove = false;
            while (!validMove)
            {
                Console.Write("Enter column number (1-7): ");
                int column;
                if (int.TryParse(Console.ReadLine(), out column) && column >= 1 && column <= 7)
                {
                    column--; 
                    if (board.DropPiece(column, Symbol))
                    {
                        validMove = true;
                    }
                    else
                    {
                        Console.WriteLine("Column is full. Please choose another column.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 7.");
                }
            }
        }
    }

    class ComputerPlayer : Player
    {
        public ComputerPlayer(char symbol) : base(symbol)
        {
        }

        public override void MakeMove(Board board)
        {
            Random random = new Random();
            int column;
            do
            {
                column = random.Next(0, 7); // Generates random column index
            } while (!board.DropPiece(column, Symbol));
        }
    }
}
