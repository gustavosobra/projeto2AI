/// @file
/// @brief This file contains the ::G10NossaAIThinker class.
///
/// @author Nuno Fachada
/// @date 2019
/// @copyright [MPLv2](http://mozilla.org/MPL/2.0/)

using System.Threading;
using UnityEngine;

/// <summary>
/// Implementation of an AI that will always play in sequence, from the first
/// to the last column. It will start by spending all round pieces, and only
/// then start using the square pieces.
/// </summary>
public class G10NossaAIThinker : IThinker
{
    // Last column played
    private int numMoves;
    private int lastCol;
    private int myDepth = 0;
    PShape shape = PShape.Round;

    /// @copydoc IThinker.Think
    /// <seealso cref="IThinker.Think"/>
    public FutureMove Think(Board board, CancellationToken ct)
    {
        // The move to perform
        FutureMove move;

        // Is this task to be cancelled?
        if (ct.IsCancellationRequested) return FutureMove.NoMove;

        move = Negamax(board, PColor.Red, 4, ct);


        // Return move
        return move;
    }

    public FutureMove Negamax(Board board, PColor turn, int depth, CancellationToken ct)
    {
            FutureMove bestMove = default;
            PColor proxTurn =
                turn == PColor.White ? PColor.Red : PColor.White;

        if (ct.IsCancellationRequested)
            return FutureMove.NoMove;
        else
        {
            if (myDepth == depth)
                return bestMove;

            myDepth++;

            for (int i = 0; i < board.cols; i++)
            {
                for (int j = 0; j < board.rows; j++)
                {
                    Vector2Int pos = new Vector2Int(i, j);

                    if(board[i, j] == null)
                    {
                        int roundPieces = board.PieceCount(board.Turn, PShape.Round);

                        int squarePieces = board.PieceCount(board.Turn, PShape.Square);

                        if (shape == PShape.Round)
                            if (roundPieces == 0)
                                shape = PShape.Square;
                            else
                            if (squarePieces == 0)
                                shape = PShape.Round;

                        FutureMove move = default;

                        board.DoMove(shape, i);

                        if (board.CheckWinner() == Winner.None)
                            move = Negamax(board, proxTurn, depth, ct);

                        board.UndoMove();

                    }
                }
            }
                    /*if (board.Turn == PColor.Red)
                        bestMove = new FutureMove(board.cols - 1, PShape.Round);
                    else
                        bestMove = new FutureMove(0, PShape.Round);*/
                }
        return bestMove;
    }
}