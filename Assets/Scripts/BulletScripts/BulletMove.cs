using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ���͓K���ɉE�ɐi�ނ����̂�B�����͌�ŏ����B
        transform.position += new Vector3(5.0f,0.0f,0.0f) * Time.deltaTime;


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        Destroy(this.gameObject);
    }
}
