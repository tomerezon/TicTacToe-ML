using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeAI;

namespace TicTacToeUtils.Utils
{
    public class GameplayUtils
    {
        private DBManager db;
        private Utilities util;
        private Random rnd = new Random();
        public GameplayUtils()
        {
            db = new DBManager();
            util = new Utilities();
        }
        public bool CheckIfWantsToPlay()
        {
            Console.WriteLine("Do you want to play a game? [Y/N]");
            string input = Console.ReadLine();
            if (input.ToUpper() == "Y") return true;
            return false;
        }
        public void PrintFinalScore(int AIscore, int playerScore, int draws)
        {
            Console.WriteLine("Final score:");
            Console.WriteLine("AI: " + AIscore);
            Console.WriteLine("Player: " + playerScore);
            Console.WriteLine("Draws: " + draws);
            Console.ReadKey();
        }
        public int[] ConductAMove(int[] board, int moveNum, int turn, int AI)
        {
            int[] nextBoard;
            if(turn == AI)
            {
                nextBoard = util.GetBestNextMove(moveNum, board, AI);
                board = util.CopyArray(board, nextBoard);
            }
            else
            {
                int index = GetValidMoveFromUser(board);
                board[index] = turn;
            }
            return board;
        }
        public void PrintCurrentGameInterface(int[] board, int aIScore, int playerScore, int draws, bool printNextMove = false)
        {
            for (int i = 0; i < board.Length; i++)
            {
                PrintBoardCell(board[i]);
                GoDownALineIfNeeded(i); 
            }
            Console.WriteLine("");
            PrintNextMoveOptions();
            Console.WriteLine("");
            PrintScoreboard(aIScore, playerScore, draws);
        }
        private void GoDownALineIfNeeded(int i)
        {
            if (i % 3 == 2)
            {
                Console.WriteLine(" ");
                if(i != 8)
                    Console.WriteLine("------------");
            }
            else
                Console.Write("|");
        }
        private void PrintScoreboard(int aIScore, int playerScore, int draws)
        {
            Console.WriteLine("AI:" + aIScore + " Player:" + playerScore + " Draws:" + draws);
            Console.WriteLine("");
        }
        private void PrintNextMoveOptions()
        {
            Console.WriteLine("0, 1, 2");
            Console.WriteLine("3, 4, 5");
            Console.WriteLine("6, 7, 8");
        }
        private void PrintBoardCell(int cell)
        {
            if (cell == 1)
            {
                Console.Write(" X ");
            }
            if (cell == -1)
            {
                Console.Write(" O ");
            }
            if (cell == 0)
            {
                Console.Write("   ");
            }
            
        }
        private int GetValidMoveFromUser(int[] board)
        {
            Console.WriteLine("Where do you want to place your next move?");
            string input = Console.ReadLine();
            int index;
            while (!Int32.TryParse(input, out index))
            {
                Console.WriteLine("You have not entered an number, please enter a valid number!");
                input = Console.ReadLine();
            }
            if(board[index] != 0)
            {
                Console.Write("Invalid move! ");
                return GetValidMoveFromUser(board);
            }
            return index;
        }
    }
}
