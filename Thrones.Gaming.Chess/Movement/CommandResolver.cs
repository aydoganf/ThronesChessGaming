using System;
using System.Collections.Generic;
using System.Text;
using Thrones.Gaming.Chess.SessionManagement;

namespace Thrones.Gaming.Chess.Movement
{
    internal struct CommandDetail 
    {
        public bool IsCorrect { get; set; }
        public string ReturnMessage { get; set; }
        public int From_X { get; set; }
        public int From_Y { get; set; }
        public int To_X { get; set; }
        public int To_Y { get; set; }

        public CommandDetail(bool isCorrect, string returnMessage, int from_x = default, int from_y = default, int to_x = default, int to_y = default) : this()
        {
            IsCorrect = isCorrect;
            ReturnMessage = returnMessage;
            From_X = from_x;
            From_Y = from_y;
            To_X = to_x;
            To_Y = to_y;
        }
    }

    internal static class CommandResolver
    {
        public static CommandDetail Resolve(string command)
        {
            string[] commandArray = command.Split(" ");
            string from = commandArray[1];
            string to = commandArray[2];

            if (from.Length != 2)
            {
                return new CommandDetail(false, "from location parameter is wrong!");
            }

            if (to.Length != 2)
            {
                return new CommandDetail(false, "from location parameter is wrong!");
            }

            if (Table.xAxisConverter.ContainsKey(from[0].ToString()) == false || Table.xAxisConverter.ContainsKey(to[0].ToString()) == false)
            {
                return new CommandDetail(false, "location is wrong!");
            }

            int fromX = Table.xAxisConverter[from[0].ToString()];
            int fromY = int.Parse(from[1].ToString());

            int toX = Table.xAxisConverter[to[0].ToString()];
            int toY = int.Parse(to[1].ToString());

            return new CommandDetail(true, string.Empty, fromX, fromY, toX, toY);
        }
    }
}
