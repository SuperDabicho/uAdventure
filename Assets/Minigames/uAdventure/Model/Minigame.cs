using System;
using uAdventure.Core;

namespace uAdventure.Minigame
{
	public class Minigame : HasId, Documented
	{
		public Minigame(string id)
		{
			Id = id;
			Conditions = new Conditions();
			Effects = new Effects();
			Respuestas = new string[4];
			Preguntas = new string[4];
		}
			
		public string Id { get; set; }

		public string Documentation { get; set; }

		public string Content { get; set; }

		public string[] Respuestas { get; set; }

		public string[] Preguntas { get; set; }

		public string getPregunta(int i)
		{
			return Preguntas[i];
		}

		public string getRespuesta(int i)
		{
			return Respuestas[i];
		}

		public void setPregunta(int i, string nueva)
		{
			Preguntas[i] = nueva;
		}
			
		public void setRespuesta(int i, string nueva)
		{
			Respuestas[i] = nueva;
		}

		public void addOption(){

			string[] p = new string[Preguntas.Length + 1];
			string[] r = new string[Respuestas.Length + 1];

			int i = 0;

			while(Preguntas.Length > i) {
				p [i] = Preguntas [i];
				r [i] = Respuestas [i];
				i++;
			}

			p [i] = "";
			r [i] = "";

			Preguntas = p;
			Respuestas = r;

		}

		public string printPretty(string[] lista){

			string s = "";
			foreach (var i in lista) {
				s+="<item>"+i+"</item>\n";
			}
			return s;
		}

		public void delOption(){

			string[] p = new string[Preguntas.Length - 1];
			string[] r = new string[Respuestas.Length - 1];

			int i = 0;

			while(p.Length > i){
				p [i] = Preguntas [i];
				r [i] = Respuestas [i];
				i++;
			}

			Preguntas = p;
			Respuestas = r;

		}

		public Conditions Conditions { get; set; }
		public Effects Effects { get; set; }

		public string getDocumentation()
		{
			return Documentation;
		}

		public string getId()
		{
			return Id;
		}

		public void setDocumentation(string documentation)
		{
			Documentation = documentation;
		}

		public void setId(string id)
		{
			Id = id;
		}
	}
}