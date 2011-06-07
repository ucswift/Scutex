using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace WaveTech.Scutex.Model
{
	//public enum Characters
	//{
	//  // 0 = 0,
	//  // 1 = 1,
	//  // 2 = 2,
	//  // 3 = 3,
	//  // 4 = 4,
	//  // 5 = 5,
	//  // 6 = 6,
	//  // 7 = 7,
	//  // 8 = 8,
	//  // 9 = 9,
	//  A = 10,
	//  B = 11,
	//  C = 12,
	//  D = 13,
	//  E = 14,
	//  F = 15,
	//  G = 16,
	//  H = 17,
	//  I = 18,
	//  J = 19,
	//  K = 20,
	//  L = 21,
	//  M = 22,
	//  N = 23,
	//  O = 24,
	//  P = 25,
	//  Q = 26,
	//  R = 27,
	//  S = 28,
	//  T = 29,
	//  U = 30,
	//  V = 31,
	//  W = 32,
	//  X = 33,
	//  Y = 34,
	//  Z = 35
	//}

	/// <summary>
	/// CharacterMaps are used to convert from Integer values to Letter values. As license
	/// keys support more then normal Hex characters 0-F, the system needs to support assigning
	/// a numerical value to each letter beyond F.
	/// </summary>
	public class CharacterMap
	{
		[SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
		public static Dictionary<int, char> Map = new Dictionary<int, char>
		{
			{ 0, char.Parse("0")},
			{ 1, char.Parse("1")},
			{ 2, char.Parse("2")},
			{ 3, char.Parse("3")},
			{ 4, char.Parse("4")},
			{ 5, char.Parse("5")},
			{ 6, char.Parse("6")},
			{ 7, char.Parse("7")},
			{ 8, char.Parse("8")},
			{ 9, char.Parse("9")},
			{ 10, char.Parse("A")},
			{ 11, char.Parse("B")},
			{ 12, char.Parse("C")},
			{ 13, char.Parse("D")},
			{ 14, char.Parse("E")},
			{ 15, char.Parse("F")},
			{ 16, char.Parse("G")},
			{ 17, char.Parse("H")},
			{ 18, char.Parse("I")},
			{ 19, char.Parse("J")},
			{ 20, char.Parse("K")},
			{ 21, char.Parse("L")},
			{ 22, char.Parse("M")},
			{ 23, char.Parse("N")},
			{ 24, char.Parse("O")},
			{ 25, char.Parse("P")},
			{ 26, char.Parse("Q")},
			{ 27, char.Parse("R")},
			{ 28, char.Parse("S")},
			{ 29, char.Parse("T")},
			{ 30, char.Parse("U")},
			{ 31, char.Parse("V")},
			{ 32, char.Parse("W")},
			{ 33, char.Parse("X")},
			{ 34, char.Parse("Y")},
			{ 35, char.Parse("Z")}
		};


		[SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
		public static Dictionary<char, int> ReverseMap = new Dictionary<char, int>
		{
			{ char.Parse("0"), 0},
			{ char.Parse("1"), 1},
			{ char.Parse("2"), 2},
			{ char.Parse("3"), 3},
			{ char.Parse("4"), 4},
			{ char.Parse("5"), 5},
			{ char.Parse("6"), 6},
			{ char.Parse("7"), 7},
			{ char.Parse("8"), 8},
			{ char.Parse("9"), 9},
			{ char.Parse("A"), 10},
			{ char.Parse("B"), 11},
			{ char.Parse("C"), 12},
			{ char.Parse("D"), 13},
			{ char.Parse("E"), 14},
			{ char.Parse("F"), 15},
			{ char.Parse("G"), 16},
			{ char.Parse("H"), 17},
			{ char.Parse("I"), 18},
			{ char.Parse("J"), 19},
			{ char.Parse("K"), 20},
			{ char.Parse("L"), 21},
			{ char.Parse("M"), 22},
			{ char.Parse("N"), 23},
			{ char.Parse("O"), 24},
			{ char.Parse("P"), 25},
			{ char.Parse("Q"), 26},
			{ char.Parse("R"), 27},
			{ char.Parse("S"), 28},
			{ char.Parse("T"), 29},
			{ char.Parse("U"), 30},
			{ char.Parse("V"), 31},
			{ char.Parse("W"), 32},
			{ char.Parse("X"), 33},
			{ char.Parse("Y"), 34},
			{ char.Parse("Z"), 35}
		};
	}
}