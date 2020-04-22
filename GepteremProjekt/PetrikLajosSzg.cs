using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GepteremProjekt
{
    class PetrikLajosSzg
    {
		private List<Gepterem> geptermek;

		public List<Gepterem> Geptermek
		{
			get { return geptermek; }
		}

		public PetrikLajosSzg(string fileName)
		{
			geptermek = new List<Gepterem>();
			try
			{
				StreamReader reader = new StreamReader(fileName);
				string sor;
				while (!reader.EndOfStream)
				{
					string nev = reader.ReadLine();
					string[] sorTomb = reader.ReadLine().Split(';');
					int sorokSzama = int.Parse(sorTomb[0]);
					int oszlopokSzama = int.Parse(sorTomb[1]);
					int[,] ertekelesek = new int[sorokSzama, oszlopokSzama];
					string gepSor = "";
					int j = 0;
					while ((gepSor = reader.ReadLine()) != "")
					{
						sorTomb = gepSor.Split(';');
						for (int i = 0; i < oszlopokSzama-1; i++)
						{
							ertekelesek[j, i] = int.Parse(sorTomb[i]);
						}
						j++;
					}
					
					geptermek.Add(new Gepterem(nev, sorokSzama, oszlopokSzama, ertekelesek));
				}
				reader.Close();
			}
			catch (IOException e)
			{

				MessageBox.Show(e.ToString());
				Environment.Exit(0);
			}

		}
	}
}
