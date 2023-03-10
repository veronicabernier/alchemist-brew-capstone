using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LeverRotate : MonoBehaviour
{
    public enum LeverPositions
    {
        Down,
        MiddleFront,
        Up,
        MiddleBack
    }

    [LabeledArrayAttribute(typeof(LeverPositions))]
    public GameObject[] leverPositions = new GameObject[4];
    //0: down, 1: middle front, 2: up, 3: middle back
    int curObject = 0;

    public Transform beansParend;
    public Transform groundParent;

    public float yOffset;
    public float fadeSeconds = 0.5f;
    public Transform beansParent;

    private Vector3 mousePositionOffset;
    private bool dragging = false;
    [System.NonSerialized]
    public bool isDirty = false;

    private List<GameObject> beanSets = new List<GameObject>();
    private List<GameObject> groundSets = new List<GameObject>();
    private int curBean = 0;

    // Start is called before the first frame update
    void Start()
    {
        PrepareLeverPositions();
        PopulateBeanLists();
    }

    private void PopulateBeanLists()
    {
        foreach (Transform ch in beansParent.GetComponentsInChildren<Transform>().Skip(1))
        {
            beanSets.Add(ch.gameObject);
        }
        foreach (Transform ch in groundParent.GetComponentsInChildren<Transform>().Skip(1))
        {
            ch.gameObject.SetActive(false);
            groundSets.Add(ch.gameObject);
        }
    }


    private void PrepareLeverPositions()
    {
        leverPositions[0].SetActive(true);
        //turn off all opacities except of first object
        for (int i = 1; i < leverPositions.Length; i++)
        {
            leverPositions[i].SetActive(true);
            foreach (SpriteRenderer sr in leverPositions[i].GetComponentsInChildren<SpriteRenderer>())
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dragging)
        {
            MoveLever();
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDown()
    {
        mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
        dragging = true;
    }

    private void OnMouseUp()
    {
        dragging = false;
    }

    private void MoveLever()
    {
        Vector3 curOffset = mousePositionOffset - (gameObject.transform.position - GetMouseWorldPosition());
        //Debug.Log(curOffset);
        if (curObject == 0)
        {
            //is down --> goes middle front
            if (curOffset.y > yOffset / 2)
            {
                //middle front
                MoveLeverActions();
            }
        }
        else if (curObject == 1)
        {
            //is middle front --> goes up
            if (curOffset.y > yOffset)
            {
                //up
                MoveLeverActions();
                Grind();
            }
        }
        else if (curObject == 2)
        {
            //is up --> goes middle back
            if (curOffset.y < -yOffset / 2)
            {
                //middle back
                MoveLeverActions();
            }
        }
        else if (curObject == 3)
        {
            //is middle back --> goes down
            if (curOffset.y < -yOffset)
            {
                //down
                MoveLeverActions();
            }
        }
    }

    private void MoveLeverActions()
    {
        if (!isDirty)
        {
            isDirty = true;
        }
        foreach (SpriteRenderer sr in leverPositions[curObject].GetComponentsInChildren<SpriteRenderer>())
        {
            StartCoroutine(FadeOut(sr));
        }
        curObject += 1;
        if (curObject > leverPositions.Length - 1)
        {
            curObject = 0;
        }
        foreach (SpriteRenderer sr in leverPositions[curObject].GetComponentsInChildren<SpriteRenderer>())
        {
            StartCoroutine(FadeIn(sr));
        }
        beansParent.localScale = new Vector3(beansParent.localScale.x * -1, beansParent.localScale.y, beansParent.localScale.z);
    }

    IEnumerator FadeOut(SpriteRenderer sr)
    {
        //float duration = 2f;
        for (float opacity = 1f; opacity >= -0.05f; opacity -= 0.05f)
        {
            //Debug.Log(opacity);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, opacity);
            yield return new WaitForSeconds(0.05f*fadeSeconds);
        }
    }

    IEnumerator FadeIn(SpriteRenderer sr)
    {
        //float duration = 2f;
        for (float opacity = 0f; opacity <= 1.05f; opacity += 0.05f)
        {
            //Debug.Log(opacity);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, opacity);
            yield return new WaitForSeconds(0.05f * fadeSeconds);
        }
    }

    private void Grind()
    {
        if (curBean < beanSets.Count && curBean < groundSets.Count)
        {
            beanSets[curBean].SetActive(false);
            groundSets[curBean].SetActive(true);
            curBean += 1;
        }
    }
}
