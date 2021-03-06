﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Client.Cmd.Wrappers;
using TicTacToe.Game.Rules;
using TicTacToe.Game._3x3;

namespace TicTacToe.Client.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=========================================");
            Console.WriteLine("               TIC TAC TOE");
            Console.WriteLine("=========================================");
            Console.WriteLine();

            CmdGame game = new CmdGame(new GameManager());

            game.ReqisterPlayer(new UserPlayer("Человек"));
            //Здесь можно поменять алгоритм игры (он должен быть наследован от IPlayer)
            game.ReqisterPlayer(new Bot("Бот"));

            game.Run();

            Console.WriteLine("Нажмите любую клавишу для выхода.");
            Console.ReadKey();
        }
    }
}
