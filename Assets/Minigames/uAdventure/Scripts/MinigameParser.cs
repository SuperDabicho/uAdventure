using UnityEngine;
using System.Collections;
using uAdventure.Core;
using System;
using System.Xml;

namespace uAdventure.Minigame
{
	[DOMParser(typeof(Minigame))]
	[DOMParser("minigame")]
	public class MinigameParser : IDOMParser
	{
		public object DOMParse(XmlElement element, params object[] parameters)
		{
			var mg = new Minigame(element.Attributes["id"].Value);
			mg.Content = element.SelectSingleNode("content").InnerText;
			mg.Documentation = element.SelectSingleNode("documentation").InnerText;
			//mg.fallosPermitidos = (int)element.SelectSingleNode ("fallos").InnerText;
				
			XmlNode pre = element.SelectSingleNode ("preguntas");
			int cont = 0;
			foreach( XmlNode i in pre.SelectNodes ("item")){
				mg.addOption ();
				mg.setPregunta (cont, i.InnerText);
				cont++;
			}
			cont = 0;
			XmlNode resp = element.SelectSingleNode ("respuestas");
			foreach( XmlNode i in resp.SelectNodes ("item")){
				mg.setRespuesta (cont, i.InnerText);
				cont++;
			}

			mg.Conditions = DOMParserUtility.DOMParse (element.SelectSingleNode("condition"), parameters) as Conditions ?? new Conditions();
			mg.Effects 	  = DOMParserUtility.DOMParse (element.SelectSingleNode("effect"), parameters) 	  as Effects ?? new Effects();

			return mg;
		}
	}
}