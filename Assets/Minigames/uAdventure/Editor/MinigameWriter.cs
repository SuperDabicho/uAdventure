using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System.Collections;
using uAdventure.Editor;
using System;
using System.Xml;

namespace uAdventure.Minigame
{
	[DOMWriter(typeof(Minigame))]
	public class MinigameWriter : ParametrizedDOMWriter
	{
		protected override void FillNode(XmlNode node, object target, params IDOMWriterParam[] options)
		{
			var element = node as XmlElement;
			var mg = target as Minigame;
			element.SetAttribute("id", mg.Id);
			AddNode(element, "content", mg.Content);
			AddNode(element, "documentation", mg.Documentation);
			AddNode (element, "preguntas", mg.printPretty(mg.Preguntas));
			AddNode (element, "respuestas", mg.printPretty(mg.Respuestas));
			DOMWriterUtility.DOMWrite(element, mg.Conditions);
			DOMWriterUtility.DOMWrite(element, mg.Effects);
		}

		protected override string GetElementNameFor(object target)
		{
			return "minigame";
		}
	}
}