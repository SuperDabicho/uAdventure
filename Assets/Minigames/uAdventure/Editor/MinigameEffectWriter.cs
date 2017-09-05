using UnityEngine;
using System.Collections;
using uAdventure.Editor;
using System;
using System.Xml;

namespace uAdventure.Minigame
{
	[DOMWriter(typeof(MinigameEffect))]
	public class MinigameEffectWriter : ParametrizedDOMWriter
	{
		protected override void FillNode(XmlNode node, object target, params IDOMWriterParam[] options)
		{
			var element = node as XmlElement;
			var effect = target as MinigameEffect;
			element.SetAttribute("idTarget", effect.minigameId);
			DOMWriterUtility.DOMWrite(element, effect.getConditions());

		}

		protected override string GetElementNameFor(object target)
		{
			return "minigameEffect";
		}
	}
}