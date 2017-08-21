using UnityEngine;
using System.Collections;
using uAdventure.Core;
using System;
using System.Collections.Generic;

namespace uAdventure.Minigame
{
	public class MinigameEffect : AbstractEffect
	{
		public enum ListType { BlackList, WhiteList }

		public MinigameEffect()
		{
			ValidIds = new List<string>();
		}

		public override EffectType getType()
		{
			return EffectType.CUSTOM_EFFECT;
		}

		public string PromptMessage { get; set; }
		public List<string> ValidIds { get; set; }
		public ListType SelectionType { get; set; }
	}
}