using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeAI
{
    public class DBManager
    {
        //public IMongoDatabase InitDB()
        //{
        //    MongoClient client = new MongoClient("mongodb://127.0.0.1:27017");
        //    return client.GetDatabase("TicTacToe");
        //}
        //public IMongoCollection<State> InitStatesCollection()
        //{
        //    IMongoDatabase dataBase = InitDB();
        //    return dataBase.GetCollection<State>("States");
        //}
        public IMongoDatabase InitDB()
        {
            MongoClient client = new MongoClient(@"mongodb://guest:a123456@ds129904.mlab.com:29904/tictactoe");
            return client.GetDatabase("tictactoe");
        }
        public IMongoCollection<State> InitStatesCollection()
        {
            IMongoDatabase dataBase = InitDB();
            return dataBase.GetCollection<State>("States");
        }
        public List<State> GetFirstMoveStates()
        {
            return InitStatesCollection().Find(x => x.moveNum == 1).ToList();
        }

        public List<State> GetAllMoveByMoveNum(int moveNum)
        {
            return InitStatesCollection().Find(x => x.moveNum == moveNum).ToList();
        }

        public List<State> GetPossibleMoves(State currentState)
        {
            List<State> possibleStates = new List<State>();
            foreach (int[] possibleMove in currentState.PossibleMoves)
            {
                possibleStates.Add(GetStateByBoard(possibleMove));
            }
            return possibleStates;
        }

        public bool IsStateExists(int[] board)
        {
            List<State> state = InitStatesCollection().Find(x => x.boardState == board).ToList();
            if (state.Any()) return true;
            return false;
        }
        public void InsertNewMoveToPrevState(int[] prevState, int[] boardState)
        {
            State previousState = GetStateByBoard(prevState);
            if (!DoesMoveExistInList(previousState.PossibleMoves, boardState))
            {
                previousState.PossibleMoves.Add(boardState);
                InitStatesCollection().DeleteOne(Builders<State>.Filter.Where(x => x.boardState == previousState.boardState));
                InitStatesCollection().InsertOne(previousState);
            }  
        }
        public void CreateNewState(int moveNumber, int[] boardState)
        {
            State state = new State(0, moveNumber, boardState, new List<int[]>(), 1.0);
            InitStatesCollection().InsertOne(state);
        }
        public State GetStateByBoard(int[] boardState)
        {
            List<State> state = InitStatesCollection().Find(x => x.boardState == boardState).ToList();
            return state.First();
        }
        public void UpdateState(State state)
        {
            InitStatesCollection().DeleteOne(Builders<State>.Filter.Where(x => x.boardState == state.boardState));
            InitStatesCollection().InsertOne(state);
        }
        public bool DoesMoveExistInList(List<int[]> list, int[] move)
        {
            foreach (int[] boardState in list)
            {
                if (Enumerable.SequenceEqual(boardState, move)) return true;
            }
            return false;
        }
    }
}
