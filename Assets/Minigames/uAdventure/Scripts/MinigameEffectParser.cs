using UnityEngine;
using System.Collections;

using uAdventure.Core;


namespace uAdventure.Minigame
{
	[DOMParser(typeof(MinigameEffect))]
	[DOMParser("minigameEffect")]
	public class MinigameEffectParser : IDOMParser {

		public object DOMParse (System.Xml.XmlElement element, params object[] parameters)
		{
			var effect = new MinigameEffect (element.Attributes ["idTarget"].Value);
			effect.setConditions (DOMParserUtility.DOMParse (element.SelectSingleNode ("condition"), parameters) as Conditions ?? new Conditions ());

			return effect;
		} 

	}
}