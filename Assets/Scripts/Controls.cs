using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public Transform ParentTransform;
    public bool ismoving = false;

    private const float ScaleSpeed = 0.07f;
    private const float MoveSpeed = 0.15f;
    public Controls otherplayer;

    // Start is called before the first frame update
    void Start()
    {
        ParentTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Movement(Vector2 direction)
    {
        string[,] layout = TileManager.Instance.layout;
        if (ParentTransform.position.x + direction.x<0 || ParentTransform.position.x + direction.x>19 || ParentTransform.position.y + direction.y<0 || ParentTransform.position.y + direction.y>14){
            yield return AnimMovement(direction);
            GameObject.Destroy(gameObject);
            yield break;
        }
        Vector2 nextpos = new Vector2((int)ParentTransform.position.x + (int)direction.x, (int)ParentTransform.position.y + (int)direction.y);
        string next = layout[(int)nextpos.x, (int)nextpos.y];
        bool shouldmove = true;
        if (next == "B"){
            yield return AnimBonk(direction);
            shouldmove = false;
        }
        else if (next == "I"){
            yield return AnimMovement(direction);
            shouldmove = false;
            yield return Movement(direction);
        }
        if (shouldmove){
            yield return AnimMovement(direction);
        }
        if (next == "L"){
            GameObject.Destroy(gameObject);
            yield break;
        }
        else if (next == "P"){
            yield return AnimScale(true);
            if (ParentTransform.position == (Vector3)TileManager.Portal1){
                ParentTransform.position = (Vector3)TileManager.Portal2;
            }
            else{
                ParentTransform.position = (Vector3)TileManager.Portal1;
            }
            yield return AnimScale(false);
        }
        string newpos = layout[(int)ParentTransform.position.x, (int)ParentTransform.position.y];
        if (otherplayer.ismoving == false){
            if (newpos == "F"){
                if (layout[(int)otherplayer.ParentTransform.position.x, (int)otherplayer.ParentTransform.position.y] == "F"){
                    TileManager.Instance.NextLevel();
                }
            }
        }
    }

    public IEnumerator AnimMovement(Vector2 direction)
    {
        ismoving = true;
        Vector3 origin = ParentTransform.position;
        Vector3 destination = ParentTransform.position + (Vector3)direction;
        float animlength = MoveSpeed;
        while (animlength > 0){
            animlength -= Time.deltaTime;
            float progress = (MoveSpeed - animlength)/MoveSpeed;
            ParentTransform.position = origin + (Vector3)direction * progress;
            yield return null;
        }
        ParentTransform.position = destination;
        ismoving = false;
    }

    public IEnumerator AnimScale(bool isshrink)
    {
        ismoving = true;
        Transform ChildTransform = this.ParentTransform.Find("Start");
        Vector3 start = isshrink? Vector3.one : Vector3.zero;
        Vector3 end = isshrink? Vector3.zero : Vector3.one;
        float animlength = ScaleSpeed;
        while (animlength > 0){
            animlength -= Time.deltaTime;
            float progress = (ScaleSpeed - animlength)/ScaleSpeed;
            ChildTransform.localScale = start + (end - start) * progress;
            yield return null;
        }
        ChildTransform.localScale = end;
        ismoving = false;
    } 

    public IEnumerator AnimBonk(Vector2 direction)
    {
        ismoving = true;
        Vector3 origin = ParentTransform.position;
        Vector3 destination = ParentTransform.position + (Vector3)direction * 0.3f;
        float animlength = MoveSpeed * 0.3f;
        while (animlength > 0){
            animlength -= Time.deltaTime;
            float progress = (MoveSpeed * 0.3f - animlength)/MoveSpeed;
            ParentTransform.position = origin + (destination - origin) * progress;
            yield return null;
        }
        animlength = MoveSpeed * 0.3f;
        while (animlength > 0){
            animlength -= Time.deltaTime;
            float progress = (MoveSpeed * 0.3f - animlength)/MoveSpeed;
            ParentTransform.position = destination + (origin - destination) * progress;
            yield return null;
        }
        ParentTransform.position = origin;
        ismoving = false;
    }
}
