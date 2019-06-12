using System;
using System.Collections.Generic;
using System.Text;

namespace MyBGServer.Models
{
	public class Board
	{
		public Cell[] MyCells { get; set; }
		public Dice Dice1 { get; set; }
		public Dice Dice2 { get; set; }
		public string User1 { get; set; }
		public string User2 { get; set; }
		public int User1FirstThrow { get; set; }
		public int User2FirstThrow { get; set; }
		public Board()
		{
			MyCells = new Cell[24];
			for (int i = 1; i < 24; i++)
				MyCells[i] = new Cell();
			MyCells[0] = new Cell { ChipColor = 1 };
			MyCells[0].NumOfChip = 15;
			MyCells[23] = new Cell { ChipColor = 2 };
			MyCells[23].NumOfChip = 15;
		}
	}
}
