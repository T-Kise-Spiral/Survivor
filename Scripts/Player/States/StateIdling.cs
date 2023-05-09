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
                // コマの回転力が０ or 耐久値が０
                if (owner._spinningTop.IsStop) return;

                // コマが地面についたら遷移する
                if (owner._spinningTop.Movable)
                {
                    owner.ChangeState(_stateMoving);
                }
            }
        }
    }
}
