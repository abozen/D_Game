using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public TileScript TileScript;

    float timeCo = TileScript.timeCo;
    ArrayList mousePosesX = new ArrayList();
    ArrayList mousePosesY = new ArrayList();
    // Start is called before the first frame update
    void Start()
    {
        mousePosesX = MapScript.mousePosesX;
        mousePosesY = MapScript.mousePosesY;

    }

    // Update is called once per frame
    void Update()
    {
        
            StartCoroutine(Move());

            if(GetTile(transform.position.x, transform.position.y).GetComponent<TileScript>().trapping || (transform.position.x == 2.5f && transform.position.y == 3.5f))
            {
                Die();
            }

    }

    IEnumerator Move() {

            yield return new WaitForSeconds(1);

            if(Input.GetKeyDown("space"))
            {
                //TileScript.isStarted = true;
                for(int i = 0; i < mousePosesX.Count; i++)
                {
                    yield return new WaitForSeconds(1 * timeCo);
                    //Debug.Log("x :" + mousePosesX[i] +"  y :" + mousePosesY[i]);
                    StartCoroutine(Move());
                    transform.position = new Vector3((float) mousePosesX[i], (float) mousePosesY[i], -3);
                    
                    //Debug.Log(transform.position.x + " " + transform.position.y);
                    // pause 1-5 seconds until the next coin spawns
                    

                }
            }

        
	}

    GameObject GetTile(float x, float y)
    {
        float tileNumX = x - 0.5f + 3;
        float tileNumY = y - 0.5f + 4;

        return GameObject.Find($"Tile {tileNumX} {tileNumY}");
    }
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.tag);
        if(other.GetComponent<TileScript>().trapping)
        {
  
            Destroy(gameObject);
            SceneManager.LoadScene("GameScene");
            Debug.Log("asd");
        }
    }

    public void Die() 
    {
        SceneManager.LoadScene("GameScene");
        ArrayList mousePosesX = new ArrayList();
        ArrayList mousePosesY = new ArrayList();
        MapScript.mousePosesX = new ArrayList();
        MapScript.mousePosesY = new ArrayList();
        
    }
}