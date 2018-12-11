using System;
using System.Collections.Generic;

namespace TicTacToeAI
{
    public class AiTrainer
    {
        private InfoGenerator infoGen;
        private Utilities util;
        public AiTrainer()
        {
            infoGen = new InfoGenerator();
            util = new Utilities();
        }

        public void StartTraining()
        {
            int[] board = new int[9];
            for (int i = 0; i < board.Length; i++)
            {
                board[i] = 0;
            }
            infoGen.InsertValidMoves(board, 1, 1);
            Console.WriteLine("Insertion has ended!");

            util.VarifyAllPossibleMoves();
            Console.WriteLine("Checking Ended!");

            infoGen.TrainAllMoves(board, 1, 1, 1);
            Console.WriteLine("Training has ended");

            Console.ReadKey();
        }       
    }
}