using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeAI
{
    public class State
    {
        public ObjectId _id { get; set; }
        public int numPlayed { get; set; }
        public int moveNum { get; set; }
        public int[] boardState { get; set; }
        public List<int[]> PossibleMoves { get; set; }
        public double stateScore { get; set; }

        public State(int[] boardState, int moveNum)
        {
            this.boardState = boardState;
            this.moveNum = moveNum;
        }
        public State(int numPlayed, int moveNum,int[] boardState, List<int[]> PossibleMoves, double stateScore)
        {
            this.numPlayed = numPlayed;
            this.moveNum = moveNum;
            this.boardState = boardState;
            this.PossibleMoves = PossibleMoves;
            this.stateScore = stateScore;
        }
    }
}
