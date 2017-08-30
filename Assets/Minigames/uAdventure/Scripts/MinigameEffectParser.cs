using UnityEngine;
using System.Collections;

using uAdventure.Core;


namespace uAdventure.Minigame
{
[DOMParser(typeof(MinigameEffect))]
[DOMParser("launch-minigame")]
public class MinigameEffectParser : IDOMParser {
	#region IDOMParser implementation

	public object DOMParse (System.Xml.XmlElement element, params object[] parameters)
	{
		//var chapter = parameters [0] as Chapter;

		return new MinigameEffect ();
	} 

	#endregion
}
}