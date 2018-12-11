using System;
using System.Collections.Generic;
using TicTacToeAI;
using TicTacToeUtils.Utils;

namespace AdminTicTacToeAI
{
    public class Game
    {
        private GameplayUtils gameplay;
        private Utilities util;
        private int moveNum;
        private int[] board = new int[9];
        private int whoIsAI = 1;
        private int turn;
        private int AIScore;
        private int playerScore;
        private int draws;
        private DBManager db;
        public Game(){
            AIScore = 0;
            playerScore = 0;
            draws = 0;
            db = new DBManager();
            util = new Utilities();
            gameplay = new GameplayUtils();
        }
        public void Play()
        {
            while (gameplay.CheckIfWantsToPlay())
            {
                InitializeGame();
                while (!util.HasGameEnded(board, moveNum))
                {
                    gameplay.PrintCurrentGameInterface(board, AIScore, playerScore, draws);
                    board = gameplay.ConductAMove(board, moveNum, turn, whoIsAI);
                    SwitchTurns();
                }
                gameplay.PrintCurrentGameInterface(board, AIScore, playerScore, draws, true);
                UpdateScoreBoard(board, whoIsAI);
            }
            gameplay.PrintFinalScore(AIScore, playerScore, draws);
        }

        private void SwitchTurns()
        {
            turn = turn * (-1);
            moveNum = moveNum + 1;
        }

        private void UpdateScoreBoard(int[] board, int whoIsAI)
        {
            if (!util.CheckIfSomeoneWon(board))
            {
                draws = draws + 1;
            }
            if (util.CheckIfSomeoneWon(board))
            {
                if (turn * (-1) == whoIsAI) AIScore = AIScore + 1;
                else playerScore = playerScore + 1;
            }
        }

        private void InitializeGame()
        {
            whoIsAI = whoIsAI * (-1);
            turn = 1;
            moveNum = 1;
            for (int i=0; i < board.Length; i++)
            {
                board[i] = 0;
            }
        }
    }
}