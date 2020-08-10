using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MinesweeperWebApp.Models
{
    public class Cell
    {
        public int Row { get; set; }

        public int Column { get; set; }

        public bool Visited { get; set; }

        public bool Live { get; set; }

        public int LiveNeighbors { get; set; }

        public bool Flagged { get; set; }

        public Cell()
        {
            Row = -1;
            Column = -1;
            Visited = false;
            Live = false;
            LiveNeighbors = 0;
            Flagged = false;
        }
    }
}