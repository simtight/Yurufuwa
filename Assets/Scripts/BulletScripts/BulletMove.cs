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
        // 今は適当に右に進むだけのやつ。挙動は後で書く。
        transform.position += new Vector3(5.0f,0.0f,0.0f) * Time.deltaTime;


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        Destroy(this.gameObject);
    }
}
