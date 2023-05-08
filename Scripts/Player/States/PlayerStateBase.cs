using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Suv
{
    public abstract class PlayerStateBase
    {
        public virtual void OnEnter(Player.PlayerCharacterController owner, PlayerStateBase prevState) { }

        public virtual void OnUpdate(Player.PlayerCharacterController owner) { }

        public virtual void OnExit(Player.PlayerCharacterController owner, PlayerStateBase nextState) { }
    }
}
