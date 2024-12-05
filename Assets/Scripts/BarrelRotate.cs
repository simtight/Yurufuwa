using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelRotate : MonoBehaviour
{
    [Tooltip("プレイヤーのゲームオブジェクト")]
    [SerializeField] private GameObject Player;

    [SerializeField] private Transform barrelTransform;
    [SerializeField] private float rotationSpeed = 6.0f;

    private void Update()
    {
        Vector3 diff = (Player.gameObject.transform.position - this.transform.position);

        barrelTransform.rotation = Quaternion.FromToRotation(Vector3.up, diff);
    }
}
