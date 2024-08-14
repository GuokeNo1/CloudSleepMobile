using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Seeker))]
public class SleeperBehaviour : MonoBehaviour
{
    public int SleeperId = 0;
    public int LastSleepId = 0;
    [SerializeField] private Sleeper data;
    [SerializeField] private Seeker seeker;
    [SerializeField] private Animator animator;
    [SerializeField] private NamePannel namePannel;
    [SerializeField] public GameObject body;
    private Path path;
    private bool isMe = false;
    private bool isSleep = false;
    private string sleepId = "-1";
    private BedBehaviour bed;
    public string UserName { get => namePannel.NickName; }
    public void SetME(bool me=false)
    {
        isMe = me;
        CancelInvoke("Heat");
        if (me)
        {
            InvokeRepeating("Heat", 5, 5);
        }
    }
    private void Heat()
    {
        var manager = FindObjectOfType<SleeperManager>();
        manager.SetPos(transform.position);
    }
    public void SetAnimator(Animator animator)
    {
        this.animator = animator;
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Sleeper");
    }
    public void SetData(Sleeper data)
    {
        this.data = data;
        GetComponentInChildren<SpriteRenderer>().sprite = data.mainSprite;
    }
    public void SetName(string name)
    {
        namePannel?.SetName(name);
    }
    public void Chart(string content)
    {
        namePannel?.Chart(content);
    }
    public void SetPos(Vector2 pos)
    {
        if (!isMe)
            transform.position = pos;
    }
    public void Leave()
    {
        Destroy(gameObject);
    }
    private void Awake()
    {
        seeker.pathCallback += OnPathDelegate;
    }

    private void OnPathDelegate(Path p)
    {
        path = p;
        TweenMove(0);
    }
    private void TweenMove(int index)
    {
        if (index >= path.vectorPath.Count)
        {
            animator.Play("idle");
            if (isMe && sleepId != "-1" && bed.isEmpty())
            {
                var manager = FindObjectOfType<SleeperManager>();
                manager.SendSleep(sleepId);
                manager.SetPos(bed.GetPos());
                sleepId = "-1";
            }
            else
            {
                sleepId = "-1";
            }
            return;
        }
        animator.Play("run");
        var target = path.vectorPath[index];
        var dis = Vector3.Distance(target, transform.position);
        gameObject.LeanMove(target, dis/2).setOnComplete(() => {
            TweenMove(index + 1);
        });
    }
    public void MoveTo(Vector2 pos)
    {
        Rect cameraRect = new Rect();
        cameraRect.size = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize) * 2;
        cameraRect.center = Camera.main.transform.position;

        if (isMe)
        {
            sleepId = "-1";
            if (isSleep)
            {
                var manager = FindObjectOfType<SleeperManager>();
                manager.SendGetUP();
                path = null;
                return;
            }
            else
            {
                var boxs = Physics2D.OverlapCircleAll(pos, .05f);
                var minbox = boxs.Length > 0 && boxs[0].GetComponent<BedBehaviour>() != null ? boxs[0] : null;
                foreach (var box in boxs)
                {
                    var bb = box.GetComponent<BedBehaviour>();
                    if (bb != null)
                    {
                        if (minbox==null || Vector2.Distance(box.transform.position, pos) < Vector2.Distance(minbox.transform.position, pos))
                        {
                            minbox = box;
                        }
                    }
                }
                if(minbox != null)
                {
                    var bb = minbox.GetComponent<BedBehaviour>();
                    bed = bb;
                    var manager = FindObjectOfType<BedManager>();
                    sleepId = manager.GetID(bb);
                }
            }
        }
        else
        {
            if (isSleep)
                return;
        }

        if (cameraRect.Contains((Vector2)transform.position) || cameraRect.Contains(pos))
        {
            if (animator.gameObject.activeSelf)
                animator.Play("run");
            seeker.StartPath(transform.position, pos);
        }
        else
        {
            SetPos(pos);
        }
    }
    public void Sleep(string id)
    {
        transform.position = BedManager.instance.GetPos(id);
        body.SetActive(false);
        isSleep = true;
    }
    public void getup()
    {
        body.SetActive(true);
        isSleep = false;
    }
    /*
    private void Update()
    {
        if (path != null && pathIndex < path.vectorPath.Count)
        {
            var pos = path.vectorPath[pathIndex];
            if (transform.position != pos)
            {
                transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * 2.1f);
            }
            else
            {
                pathIndex++;
            }
        }else if (path!=null && pathIndex >= path.vectorPath.Count)
        {
            animator.Play("idle");
            path = null;
            if (isMe && sleepId != "-1" && bed.isEmpty())
            {
                var manager = FindObjectOfType<SleeperManager>();
                manager.SendSleep(sleepId);
                manager.SetPos(bed.GetPos());
                sleepId = "-1";
            }
            else
            {
                sleepId = "-1";
            }
        }
    }
    */
    public void SetType(string id)
    {
    }
    public void SetEmote(int n)
    {
        CancelInvoke("BackDefaultSprite");
        GetComponentInChildren<SpriteRenderer>().sprite = data.subSprites[n];
        Invoke("BackDefaultSprite", 3);
    }
    private void BackDefaultSprite()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = data.mainSprite;
    }
}
