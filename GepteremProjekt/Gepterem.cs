using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GepteremProjekt
{
    public class Gepterem
    {
		private int[,] ertekeles;
		private int helyDb;
		private string nev;
		private int sorDb;

		public int SorDb
		{
			get { return sorDb; }
		}


		public int[,] Ertekeles
		{
			get { return ertekeles; }
		}


		public int HelyDb
		{
			get { return helyDb; }
		}

		

		public string Nev
		{
			get { return nev; }
		}

		public Gepterem(string nev, int sorDb, int helyDb, int[,] ertekeles)
		{
			this.nev = nev;
			this.sorDb = sorDb;
			this.helyDb = helyDb;
			this.ertekeles = ertekeles;
		}

		public double Avg()
		{
			return (from int item in ertekeles
					where item > 0
					select item).Average();
			//List<int> l = new List<int>();
			//foreach (var item in ertekeles)
			//{
			//	if (item > 0)
			//	{
			//		l.Add(item);
			//	}
			//}
			//return l.Average();
		}
	}
}
