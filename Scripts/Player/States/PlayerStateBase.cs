using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Suv
{
    public abstract class PlayerStateBase
    {
        public virtual void OnEnter(PlayerCharacter owner, PlayerStateBase prevState) { }

        public virtual void OnUpdate(PlayerCharacter owner) { }

        public virtual void OnExit(PlayerCharacter owner, PlayerStateBase nextState) { }
    }
}
