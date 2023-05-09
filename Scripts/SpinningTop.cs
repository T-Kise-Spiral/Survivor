using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Suv
{
    public class SpinningTop : MonoBehaviour
    {
        [SerializeField] PlayerCharacter playerCharacter = null;
        [SerializeField] GameObject spinningTopObject = null;

        private Rigidbody rigidbody;

        private float rotationSpeed = 5.0f; // コマの回転
        public bool IsStop => rotationSpeed <= 0.0f;

        private bool _movable = false;
        public bool Movable => _movable;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            rigidbody.maxAngularVelocity = 180.0f;
        }

        private void FixedUpdate()
        {
            // コマ本体の回転
            //spinningTopObject.transform.Rotate(0, rotationSpeed, 0);

            // ここから移動処理
            if (!_movable) return;

            // playerCharacter.transform.Rotate(0, playerCharacter.CurvePow, 0);
            rigidbody.AddTorque(playerCharacter.CurvePow, 0,0, ForceMode.Force);
            rigidbody.AddForce(playerCharacter.transform.forward.normalized * playerCharacter.AddforcePow, ForceMode.Force);

            Debug.DrawRay(playerCharacter.transform.position, playerCharacter.transform.forward * 2.0f, Color.blue);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag(TagManager.TAG_GROUND)) return;

            _movable = true;
        }

        private void OnCollisionExit(Collision collision)
        {
            if (!collision.gameObject.CompareTag(TagManager.TAG_GROUND)) return;

            _movable = false;
        }
    }
}
