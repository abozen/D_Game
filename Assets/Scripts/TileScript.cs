using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TileScript : MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor, trapColor, orgColor, goalColor;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] public GameObject highlight, txt;
    [SerializeField] private TextMeshPro text;
    
    public static float timeCo = 0.75f;
    public float trappingTime = 1f;

    public bool isClicked = false;
    public bool isTrap = false;
    public bool trapping = false;
    public bool isGoal = false;
    public bool isStarted = false;

    public void Init(bool isOffset)
    {
        renderer.color = isOffset ? offsetColor : baseColor;
        orgColor = renderer.color;

    }

    void Start()
    {
        if(isTrap)
        {
            renderer.color = trapColor;
            StartCoroutine(SetTrapColor());
            txt.SetActive(true);
            text.text = trappingTime.ToString();
            //text.GetComponent<UnityEngine.UI.Text>().text = trappingTime.ToString();
        }
        //renderer.color = isGoal ? goalColor : orgColor;
        
    }

    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            isStarted = true;
            //txt.SetActive(false);
        }
    }

    IEnumerator SetTrapColor()
    {
        while(true)
        {   
            if(isStarted)
            if(!trapping)
            {
                txt.SetActive(true);
                renderer.color = trapColor;
                trapping = !trapping;
                yield return new WaitForSeconds(trappingTime * timeCo);
            }else
            {
                txt.SetActive(false);
                renderer.color = orgColor;
                trapping = !trapping;
                yield return new WaitForSeconds(2 * timeCo);
            }

            yield return new WaitForSeconds(0 * timeCo);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
 
        if(other.tag == "Player" && trapping)
        {
  
            Destroy(other.gameObject);
            SceneManager.LoadScene("GameScene");
            Debug.Log("asd");
        }
    }

    private void OnMouseEnter() {
        if(!isClicked)
        highlight.SetActive(true);    
    }
    private void OnMouseExit() {
        if(!isClicked)
        highlight.SetActive(false);  
    }
}
