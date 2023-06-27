using System;

namespace ConnectFour
{
    abstract class Player
    {
        public string Name { get; set; }
        public char Piece { get; set; }

        public abstract int GetMove(char[,] board);
    }

    class HumanPlayer : Player
    {
        public override int GetMove(char[,] board)
        {
            Console.Write($"{Name}, choose a column (1-7): ");
            int col = Convert.ToInt32(Console.ReadLine());
            return col - 1; // Adjusting the column value to be zero-based
        }
    }

    class ConnectFourGame
    {
        private char[,] board;
        private Player player1;
        private Player player2;
        private int turn;

        private const int Rows = 6;
        private const int Cols = 7;

        public ConnectFourGame(Player player1, Player player2)
        {
            board = CreateBoard();
            this.player1 = player1;
            this.player2 = player2;
            turn = 0;
        }

        private char[,] CreateBoard()
        {
            char[,] board = new char[Rows, Cols];
            return board;
        }

        private void DropPiece(int row, int col, char piece)
        {
            board[row, col] = piece;
        }

        private int GetNextOpenRow(int col)
        {
            for (int r = 0; r < Rows; r++)
            {
                if (board[r, col] == 0)
                {
                    return r;
                }
            }
            return -1;
        }

        private bool WinningMove(char piece)
        {
            // Check horizontal locations for win
            for (int c = 0; c < Cols - 3; c++)
            {
                for (int r = 0; r < Rows; r++)
                {
                    if (board[r, c] == piece && board[r, c + 1] == piece && board[r, c + 2] == piece && board[r, c + 3] == piece)
                    {
                        return true;
                    }
                }
            }

            // Check vertical locations for win
            for (int c = 0; c < Cols; c++)
            {
                for (int r = 0; r < Rows - 3; r++)
                {
                    if (board[r, c] == piece && board[r + 1, c] == piece && board[r + 2, c] == piece && board[r + 3, c] == piece)
                    {
                        return true;
                    }
                }
            }

            // Check positively sloped diagonals
            for (int c = 0; c < Cols - 3; c++)
            {
                for (int r = 0; r < Rows - 3; r++)
                {
                    if (board[r, c] == piece && board[r + 1, c + 1] == piece && board[r + 2, c + 2] == piece && board[r + 3, c + 3] == piece)
                    {
                        return true;
                    }
                }
            }

            // Check negatively sloped diagonals
            for (int c = 0; c < Cols - 3; c++)
            {
                for (int r = 3; r < Rows; r++)
                {
                    if (board[r, c] == piece && board[r - 1, c + 1] == piece && board[r - 2, c + 2] == piece && board[r - 3, c + 3] == piece)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void PrintBoard()
        {
            for (int r = Rows - 1; r >= 0; r--)
            {
                for (int c = 0; c < Cols; c++)
                {
                    if (board[r, c] == 'X')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("X ");
                    }
                    else if (board[r, c] == 'O')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("O ");
                    }
                    else
                    {
                        Console.Write("- ");
                    }
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }

        public void Play()
        {
            bool gameover = false;

            while (!gameover)
            {
                Player currentPlayer = (turn == 0) ? player1 : player2;

                int col = currentPlayer.GetMove(board);
                if (IsValidMove(col))
                {
                    int row = GetNextOpenRow(col);
                    DropPiece(row, col, currentPlayer.Piece);
                    if (WinningMove(currentPlayer.Piece))
                    {
                        PrintBoard();// Print the board after the winning move
                        Console.WriteLine($"{currentPlayer.Name} wins!");

                        if (PlayAgain())
                        {
                            ResetBoard();
                            PrintBoard();
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                PrintBoard();
                turn = (turn + 1) % 2;
            }
        }

        private bool IsValidMove(int col)
        {
            return col >= 0 && col < Cols && board[Rows - 1, col] == 0;
        }

        private void ResetBoard()
        {
            board = CreateBoard();
            turn = 0;
        }

        private bool PlayAgain()
        {
            Console.Write("Do you want to play again? (Y/N): ");
            string input = Console.ReadLine().Trim().ToUpper();
            return input == "Y" || input == "YES";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Player player1 = new HumanPlayer { Name = "Player 1", Piece = 'X' };
            Player player2 = new HumanPlayer { Name = "Player 2", Piece = 'O' };

            ConnectFourGame game = new ConnectFourGame(player1, player2);
            game.Play();
        }
    }
}
