using System;

namespace TicTacToeAI
{
    public class MoveDBUpdater
    {
        private DBManager db;
        public MoveDBUpdater()
        {
            db = new DBManager();
        }
        public void UpdateMinMaxValueForWin(int[] boardState)
        {
            State state = db.GetStateByBoard(boardState);
            state.stateScore = 10.0;
            state.numPlayed = state.numPlayed + 1;
            db.UpdateState(state);
        }

        public void UpdateMinMaxValueForLose(int[] boardState)
        {
            State state = db.GetStateByBoard(boardState);
            state.stateScore = -10.0;
            state.numPlayed = state.numPlayed + 1;
            db.UpdateState(state);
        }

        public void UpdateMinMaxValueForDraw(int[] boardState)
        {
            State state = db.GetStateByBoard(boardState);
            state.stateScore = 0.0;
            state.numPlayed = state.numPlayed + 1;
            db.UpdateState(state);
        }
    }
}