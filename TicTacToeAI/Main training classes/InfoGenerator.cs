using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeAI
{
    public class InfoGenerator
    {
        private DBManager db;
        private Utilities util;
        private MoveDBUpdater dBUpdater;
        public InfoGenerator()
        {
            dBUpdater = new MoveDBUpdater();
            db = new DBManager();
            util = new Utilities();
        }


        public void InsertValidMoves(int[] board, int turn, int moveNum)
        {
            int[] prevBoard = new int[9];
            prevBoard = util.CopyArray(prevBoard, board);
            if (!util.HasGameEnded(board, moveNum))
            {
                for (int i = 0; i < board.Length; i++)
                {
                    if (IsCellEmpty(board[i]))
                    {
                        board[i] = turn;
                        if (!db.IsStateExists(board))
                            db.CreateNewState(moveNum, board);
                        InsertValidMoves(board, turn * (-1), moveNum + 1);
                        if (moveNum > 1)
                            db.InsertNewMoveToPrevState(prevBoard, board);
                        board[i] = 0;
                    }
                }
            }
        }

        private bool IsCellEmpty(int cell)
        {
            if (cell == 0)
                return true;
            return false;
        }

        public double TrainAllMoves(int[] board, int turn, int moveNum, int level)
        {
            if (level > 1)
            {
                if (HasMoveBeenPlayedBefore(db.GetStateByBoard(board).stateScore))
                    return db.GetStateByBoard(board).stateScore;
            }
            if (util.HasGameEnded(board, moveNum))
            {
                return util.UpdateMinmaxValue(board, moveNum, turn * (-1));
            }
            State[] possibleStates = util.GetPossibleMoves(moveNum, board);
            if (level % 2 == 1)
            {
                return TrainForMaxValue(possibleStates, board, turn, moveNum, level); 
            }
            else
            {
                return TrainForMinValue(possibleStates, board, turn, moveNum, level);
            }
        }

        private double TrainForMinValue(State[] possibleStates, int[] board, int turn, int moveNum, int level)
        {
            double score = 10000;
            for (int i = 0; i < possibleStates.Length; i++)
            {
                double nextScore = TrainAllMoves(possibleStates[i].boardState, turn * (-1), moveNum + 1, level + 1);
                if (nextScore < score)
                    score = nextScore;
            }
            if (level > 1)
                util.UpdateMinMaxValueForScore(score, board);
            return score;
        }

        private double TrainForMaxValue(State[] possibleStates, int[] board, int turn, int moveNum, int level)
        {
            double score = -10000;
            for (int i = 0; i < possibleStates.Length; i++)
            {
                double nextScore = TrainAllMoves(possibleStates[i].boardState, turn * (-1), moveNum + 1, level + 1);
                if (nextScore > score)
                    score = nextScore;
            }
            if (level > 1)
                util.UpdateMinMaxValueForScore(score, board);
            return score;
        }

        private bool HasMoveBeenPlayedBefore(double stateScore)
        {
            if (stateScore != 1.0)
                return true;
            return false;
        }
    }
}
