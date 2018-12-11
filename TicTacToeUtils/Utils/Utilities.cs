using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeAI
{
    public class Utilities
    {
        private DBManager db;
        private MoveDBUpdater dBUpdater;
        public Utilities()
        {
            dBUpdater = new MoveDBUpdater();
            db = new DBManager();
        }
        #region Copy methods
        public int[] CopyArray(int[] copyArray, int[] originalArray)
        {
            for (int i = 0; i < originalArray.Length; i++)
            {
                copyArray[i] = originalArray[i];
            }
            return copyArray;
        }

        public List<State> CopyList(List<State> copyOfGameStates, List<State> gameStates)
        {
            foreach (State state in gameStates)
            {
                copyOfGameStates.Add(state);
            }
            return copyOfGameStates;
        }
        #endregion
        #region Get methods
        public State[] GetPossibleMoves(int moveNum, int[] board)
        {
            if (moveNum == 1)
            {
                return db.GetFirstMoveStates().ToArray();
            }
            else
            {
                return db.GetPossibleMoves(db.GetStateByBoard(board)).ToArray();
            }
        }

        public int[] MakeARandomMove(int moveNum, int[] board)
        {
            Random rnd = new Random();
            State[] states = db.GetFirstMoveStates().ToArray();
            if (moveNum != 1)
            {
                states = GetPossibleMoves(moveNum, board);
            }
            return states[rnd.Next(states.Length)].boardState;
        }

        public int[] GetBestNextMove(int moveNum, int[] board, int AiTurn)
        {
            double bestScore = 0;
            int[] bestBoard = new int[9];
            State[] states = states = GetPossibleMoves(moveNum, board);
            if(AiTurn == 1)
            {
                return states[GetMaxValueIndex(states)].boardState;
            }
            else
            {
                return states[GetMinValueIndex(states)].boardState;
            }
        }

        private int GetMinValueIndex(State[] states)
        {
            double bestScore = 100.0;
            double score = 0.0;
            int minValueIndex = 0;
            for (int i = 0; i < states.Length; i++)
            {
                score = states[i].stateScore;
                if (score <= bestScore)
                {
                    bestScore = score;
                    minValueIndex = i;
                }
                if (bestScore == -10.0)
                {
                    return minValueIndex;
                }
            }
            return minValueIndex;
        }

        private int GetMaxValueIndex(State[] states)
        {
            double bestScore = -100.0;
            double score = 0.0;
            int maxValueIndex = 0;
            for (int i = 0; i < states.Length; i++)
            {
                score = states[i].stateScore;
                if (score >= bestScore)
                {
                    bestScore = score;
                    maxValueIndex = i;
                }
                if (bestScore == 10.0)
                {
                    return maxValueIndex;
                }
            }
            return maxValueIndex;
        }

        #endregion
        #region Update methods

        public void UpdateMinMaxValueForScore(double score, int[] boardState)
        {
            if (score == 10.0)
            {
                dBUpdater.UpdateMinMaxValueForWin(boardState);
            }
            else if (score == -10.0)
            {
                dBUpdater.UpdateMinMaxValueForLose(boardState);
            }
            else
            {
                dBUpdater.UpdateMinMaxValueForDraw(boardState);
            }
        }

        public double UpdateMinmaxValue(int[] board, int moveNum, int turn)
        {
            if (CheckIfSomeoneWon(board))
            {
                if (turn == 1)
                {
                    dBUpdater.UpdateMinMaxValueForWin(board);
                    return 10.0;
                }
                else
                {
                    dBUpdater.UpdateMinMaxValueForLose(board);
                    return -10.0;
                }
            }
            else
            {
                dBUpdater.UpdateMinMaxValueForDraw(board);
                return 0.0;
            }
        }
        #endregion
        #region Verify methods
        public void VarifyAllPossibleMoves()
        {
            for(int i = 1; i < 10; i++)
            {
               List<State> states = db.GetAllMoveByMoveNum(i);
                foreach (State state in states)
                {
                    if (state.PossibleMoves.Count > (9-i))
                    {
                        Console.WriteLine("Move Num: " + state.moveNum + " Possible Moves: " + state.PossibleMoves.Count);
                    }
                }
            }
        }
        #endregion
        #region Game status method
        public bool HasGameEnded(int[] board, int moveNum)
        {
            return (!((moveNum <= 9) && (!CheckIfSomeoneWon(board))));
        }
        public bool CheckIfSomeoneWon(int[] board)
        {
            if (CheckVerticals(board)) return true;
            if (CheckHorizonals(board)) return true;
            if (CheckCrosses(board)) return true;
            return false;
        }
        public bool CheckCrosses(int[] board)
        {
            int middleSquare = board[4];
            if ((board[0] == middleSquare) && (middleSquare == board[8]) && (middleSquare != 0)) return true;
            if ((middleSquare == board[2]) && (middleSquare == board[6]) && (middleSquare != 0)) return true;
            return false;
        }
        public bool CheckHorizonals(int[] board)
        {
            int firstOfFirst = board[0];
            int firstOfSecond = board[3];
            int firstOfThird = board[6];
            if ((firstOfFirst == board[1]) && (firstOfFirst == board[2]) && (firstOfFirst != 0)) return true;
            if ((firstOfSecond == board[4]) && (firstOfSecond == board[5]) && (firstOfSecond != 0)) return true;
            if ((firstOfThird == board[7]) && (firstOfThird == board[8]) && (firstOfThird != 0)) return true;
            return false;
        }
        public bool CheckVerticals(int[] board)
        {
            int firstOfFirst = board[0];
            int firstOfSecond = board[1];
            int firstOfThird = board[2];
            if ((firstOfFirst == board[3]) && (firstOfFirst == board[6]) && (firstOfFirst != 0)) return true;
            if ((firstOfSecond == board[4]) && (firstOfSecond == board[7]) && (firstOfSecond != 0)) return true;
            if ((firstOfThird == board[5]) && (firstOfThird == board[8]) && (firstOfThird != 0)) return true;
            return false;
        }
        #endregion
    }
}
