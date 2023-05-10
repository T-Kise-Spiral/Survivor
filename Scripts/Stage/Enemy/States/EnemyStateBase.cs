using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Suv
{
    public class EnemyStateBase
    {
        public virtual void OnEnter(EnemyBase owner, EnemyStateBase prevState) { }

        public virtual void OnUpdate(EnemyBase owner) { }

        public virtual void OnExit(EnemyBase owner, EnemyStateBase nextState) { }
    }
}