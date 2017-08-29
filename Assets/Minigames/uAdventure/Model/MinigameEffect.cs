using UnityEngine;
using System.Collections;
using uAdventure.Core;
using System;
using System.Collections.Generic;

namespace uAdventure.Minigame
{
	public class MinigameEffect : AbstractEffect
	{

		public MinigameEffect(string minigameId)
		{
			this.minigameId = minigameId;
		}

		public override EffectType getType()
		{
			return EffectType.CUSTOM_EFFECT;
		}

		public void setMinigameId (string minigameId){
			this.minigameId = minigameId;
		}


		public string getMinigameId(){
			return this.minigameId;
		}


		public string minigameId;



	}
}