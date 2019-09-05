using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {

    private bool isClick = false;
    public float maxDis = 3;
    [HideInInspector]
    public  SpringJoint2D sp;
    protected Rigidbody2D rg;

    public LineRenderer right;
    public Transform RightPos;
    public LineRenderer left;
    public Transform LeftPos;

    public GameObject boom;

    protected TestMyTrail mytrail;

    private bool canMove = true;

    public float smooth = 3;

    public AudioClip select;
    public AudioClip fly;

    private bool isFly = false;

    public Sprite hurt;
    protected SpriteRenderer render;

    private void Awake()
    {
        sp = GetComponent<SpringJoint2D>();
        rg = GetComponent<Rigidbody2D>();
        mytrail = GetComponent<TestMyTrail>();
        render = GetComponent<SpriteRenderer>();
    }
    private void OnMouseDown()
    {
        if (canMove)
        {
            AudioPlay(select);
            isClick = true;
            rg.isKinematic = true;
        }
    }

    private void OnMouseUp()
    {
        if (canMove)
        {
            isClick = false;
            rg.isKinematic = false;
            Invoke("Fly", 0.1f);
            //Stop line
            right.enabled = false;
            left.enabled = false;
            canMove = false;
        }
    }

    private void Update()
    {
        if (isClick)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position += new Vector3(0, 0, -Camera.main.transform.position.z);
            if (Vector3.Distance(transform.position, RightPos.position) > maxDis)
            {
                Vector3 pos = (transform.position - RightPos.position).normalized;
                pos *= maxDis;
                transform.position = pos + RightPos.position;
            }
            Line(); 
        }

        //Camera Following
        float posX = transform.position.x;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, 
            new Vector3(Mathf.Clamp(posX, 0, 15), Camera.main.transform.position.y, Camera.main.transform.position.z), smooth * Time.deltaTime);

        if (isFly)
        {
            if (Input.GetMouseButtonDown(0)){
                ShowSkill();
            }
        }
    }

    void Fly()
    {
        isFly = true;
        AudioPlay(fly);
        mytrail.StartTrails();
        sp.enabled = false;
        Invoke("Next", 5);
    }

    void Line()
    {
        right.enabled = true;
        left.enabled = true;

        right.SetPosition(0, RightPos.position);
        right.SetPosition(1, transform.position);

        left.SetPosition(0, LeftPos.position);
        left.SetPosition(1, transform.position);
    }

    protected virtual void Next()
    {
        GameManager._instance.birds.Remove(this);
        Destroy(gameObject);
        Instantiate(boom, transform.position, Quaternion.identity);
        GameManager._instance.NextBird();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isFly = false;
        mytrail.ClearTrails();
    }

    public void AudioPlay(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }

    public virtual void ShowSkill()
    {
        isFly = false;
    }

    public void Hurt()
    {
        render.sprite = hurt;
    }
}
