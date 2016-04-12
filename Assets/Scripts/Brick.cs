using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour {

    public Sprite[] hitSprites;
    public static int breakableCount = -1;

    private int timesHit;
    private LevelManager levelManager;
    private bool isBreakable;
    public GameObject smoke;

	// Use this for initialization
	void Start () {
        isBreakable = (this.tag == "Breakable");
        if (isBreakable) {
            breakableCount++;
            print(breakableCount);
        }
        timesHit = 0;
        levelManager = GameObject.FindObjectOfType<LevelManager>();
	}

    // Update is called once per frame
    void Update(){

    }

    void OnCollisionEnter2D(Collision2D collision){
        timesHit++;
       
        int maxHits = hitSprites.Length + 1;
        if (timesHit >= maxHits){
            breakableCount--;
            if (smoke) {
                PuffSmoke();
            }
            print(breakableCount);
            levelManager.BrickDestroyed();
            
            Destroy(gameObject, 0.1f);
        }
        else {
            LoadSprites();
        }
    }

    void PuffSmoke() {
        GameObject smokePuff = Instantiate(smoke, gameObject.transform.position, Quaternion.identity) as GameObject;
        smokePuff.GetComponent<ParticleSystem>().startColor = gameObject.GetComponent<SpriteRenderer>().color;
    }

    void SimulateWin() {
        levelManager.LoadNextLevel();
    }

    void LoadSprites() {
        int spriteIndex = timesHit - 1;
        if (hitSprites[spriteIndex])
        {
            this.GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
        }
        else {
            Debug.LogError("Brick Sprite missing");
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (isBreakable)
        {
            this.GetComponent<AudioSource>().Play();
        }
    }
}
