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
        public class StateMoving : EnemyStateBase
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
                OnMove(owner, _ctsDictionary[owner.gameObject].Token).Forget();
            }

            private async UniTask OnMove(EnemyBase owner, CancellationToken token)
            {
                // プレイヤーの生成が終わるまで待機させる
                while (true)
                {
                    await UniTask.DelayFrame(1, cancellationToken: token);
                    if (StageManager.I.PlayerCharacter) break;
                }

                // 生きている間、ダメージを受けていないときは基本的にずっとプレイヤーへ近づく
                while (true)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(owner._moveWaitTime), cancellationToken: token);

                    if (token.IsCancellationRequested) break;

                    // 移動
                    Vector3 moveVec = StageManager.I.PlayerCharacter.transform.position - owner.transform.position;
                    moveVec.Normalize();
                    moveVec *= owner._moveSpped;
                    owner._rigidbody.AddForce(moveVec, ForceMode.Impulse);
                }
            }

            // EnemyBaseのOnTriggerEnter( 武器の攻撃を受けたときに呼び出している )
            public void MoveCancel(EnemyBase owner)
            {
                if (_ctsDictionary.ContainsKey(owner.gameObject))
                    _ctsDictionary[owner.gameObject]?.Cancel();
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
