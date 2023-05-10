using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Suv
{
    public partial class PlayerCharacter
    {
        public class StateIdling : PlayerStateBase
        {
            public override void OnUpdate(PlayerCharacter owner)
            {
                if (owner._playerInput.actions["Move"].IsPressed())
                {
                    owner.ChangeState(_stateMoving);
                }
            }
        }
    }
}
