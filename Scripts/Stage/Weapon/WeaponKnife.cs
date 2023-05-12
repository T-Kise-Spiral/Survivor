using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Suv
{
    public class WeaponKnife : WeaponBase
    {
        private bool isUsing = false;
        private Vector3 _moveVec = Vector3.zero;

        protected override void OnUseWeapon()
        {
            base.OnUseWeapon();

            // ????????????
            _moveVec = (StageManager.I.PlayerCharacter.transform.position + StageManager.I.PlayerCharacter.LastInputVec) - StageManager.I.PlayerCharacter.transform.position;
            _moveVec.y = 0;
            _moveVec.Normalize();

            isUsing = true;
        }

        private void Update()
        {
            if (!isUsing) return;

            transform.Translate(_moveVec * Time.deltaTime *_weaponData.MoveSpeed);
        }

        protected override void UseEnd()
        {
            base.UseEnd();

            isUsing = false;
        }
    }
}
