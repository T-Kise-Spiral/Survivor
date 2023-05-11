using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Suv
{
    public partial class EnemyBase
    {
        public class StateReceivingDamage : EnemyStateBase
        {
            private Dictionary<GameObject, CancellationTokenSource> _ctsDictionary = new Dictionary<GameObject, CancellationTokenSource>();

            public override void OnEnter(EnemyBase owner, EnemyStateBase prevState)
            {
                if (!_ctsDictionary.ContainsKey(owner.gameObject))
                {
                    _ctsDictionary.Add(owner.gameObject, new CancellationTokenSource());
                }

                _ctsDictionary[owner.gameObject]?.Cancel();
                _ctsDictionary[owner.gameObject] = new CancellationTokenSource();
                OnReceveDamageCoolTimeAsync(owner, _ctsDictionary[owner.gameObject].Token).Forget();
            }

            // ダメージ判定後、一定時間停止してから動き出す
            private async UniTask OnReceveDamageCoolTimeAsync(EnemyBase owner, CancellationToken token)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(owner._receiveDamageCoolTime), cancellationToken: token);

                if (!token.IsCancellationRequested)
                {
                    owner.ChangeState(_stateMoving);
                }
            }

            public void OnDestroy(EnemyBase owner)
            {
                if (_ctsDictionary.ContainsKey(owner.gameObject))
                {
                    _ctsDictionary[owner.gameObject]?.Cancel();
                    _ctsDictionary[owner.gameObject]?.Dispose();
                    _ctsDictionary[owner.gameObject] = null;
                }
            }
        }
    }
}
