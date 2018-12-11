using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeAI
{
    class Program
    {
        static void Main(string[] args)
        {
            AiTrainer trainer = new AiTrainer();
            trainer.StartTraining();
        }
    }
}
