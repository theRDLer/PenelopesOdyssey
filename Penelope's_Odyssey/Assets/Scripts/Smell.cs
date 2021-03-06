using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Invector.vCharacterController;

public class Smell : MonoBehaviour
{
    private NavMeshPath path;
    private ParticleSystem[] hotdog;
    private Animator anim;
    private GameObject target;
    private LineRenderer line;

    public GameManager gm;
  
    public List<GameObject> targets = new List<GameObject>();

    public AudioSource sniffSound;
    public AudioSource squeakSound;

    public GameObject backpackFoundIcon;
    public GameObject compassFoundIcon;
    public GameObject squishFoundIcon;
    public GameObject bookFoundIcon;
    public GameObject crownFoundIcon;

    public vThirdPersonController controller;

    void Start()
    {
        path = new NavMeshPath();
        target = targets[0];
        controller = GetComponent<vThirdPersonController>();
        anim = GetComponent<Animator>();
        line = GetComponentInChildren<LineRenderer>();

        hotdog = FindObjectsOfType<ParticleSystem>();

        backpackFoundIcon.gameObject.SetActive(false);
        compassFoundIcon.gameObject.SetActive(false);
        squishFoundIcon.gameObject.SetActive(false);
        bookFoundIcon.gameObject.SetActive(false);
        crownFoundIcon.gameObject.SetActive(false);
    }

    void Update()
    {
        // Calculate NavMesh path.
        NavMesh.CalculatePath(transform.position, target.transform.position, NavMesh.AllAreas, path);

        // Draw the path on mouse click
        if (Input.GetMouseButtonDown(0) && !gm.paused) // Only sniff if not paused
        {
            StartCoroutine("Sniff");

            if (gm.paused == false)
                sniffSound.Play();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            for (int i = 0; i < hotdog.Length; i++)
            {
                hotdog[i].Play();
            }
        }

        // Stop drawing hotdogs
        if (Input.GetMouseButtonUp(1))
        {
            for (int i = 0; i < hotdog.Length; i++)
            {
                hotdog[i].Stop();
            }
        }

    }

    private IEnumerator Sniff()
    {
        controller.lockMovement = true;
        controller.lockRotation = true;
        DrawPath(path);
        anim.SetBool("IsSniffing", true);

        yield return new WaitForSeconds(anim.runtimeAnimatorController.animationClips[7].length);

        controller.lockMovement = false;
        controller.lockRotation = false;
        anim.SetBool("IsSniffing", false);
        line.positionCount = 0;
    }

    void DrawPath(NavMeshPath path)
    {
        if (path.corners.Length < 2) //if the path has 1 or no corners, there is no need
            return;

        line.positionCount = path.corners.Length; //set the array of positions to the amount of corners
        line.SetPositions(path.corners);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("object"))
        {
            targets.Remove(other.gameObject);
            target = targets[0];
            squeakSound.Play();
        }

        if (other.gameObject.name == "Backpack")
            backpackFoundIcon.gameObject.SetActive(true);
        else if (other.gameObject.name == "Compass")
            compassFoundIcon.gameObject.SetActive(true);
        else if (other.gameObject.name == "SquishMouse")
            squishFoundIcon.gameObject.SetActive(true);
        else if (other.gameObject.name == "Book")
            bookFoundIcon.gameObject.SetActive(true);
        else if (other.gameObject.name == "BaseballHat")
            crownFoundIcon.gameObject.SetActive(true);
    }

    /*
         void drawCombined(Vector3 startPoint, float amplitude, float wavelength, NavMeshPath path)
    {
        float x = 0f;
        float y;
        float k = 2 * Mathf.PI / wavelength;
        line.positionCount = path.corners.Length + 50;
        for (int i = 0; i < line.positionCount; i++)
        {
            x += i * 0.001f;
            y = amplitude * Mathf.Sin(k * x);
            line.SetPosition(i ,new Vector3(x, y, 0) + startPoint);
        }
    }

    void DrawSineWave(Vector3 startPoint, float amplitude, float wavelength)
    {
        float x = 0f;
        float y;
        float k = 2 * Mathf.PI / wavelength;
        line.positionCount = 200;
        for (int i = 0; i < line.positionCount; i++)
        {
            x += i * 0.001f;
            y = amplitude * Mathf.Sin(k * x);
            line.SetPosition(i, new Vector3(x, y, 0) + startPoint);
        }
    }
    */

    /*
 public virtual void SniffObject()
 {

     controller.lockMovement = true;
     DrawPath(path);
     //drawCombined(gameObject.transform.position, 0.5f, 2f, path);

     if (gm.paused == false)
         audioSource.Play();

     if (Input.GetMouseButtonUp(0))
     {
         controller.lockMovement = false;
         line.positionCount = 0;

     }
 }

 public virtual void SniffFood()
 {
     for (int i = 0; i < hotdog.Length; i++)
     {
         hotdog[i].Play();
     }

     if (Input.GetMouseButtonUp(1))
     {
         for (int i = 0; i < hotdog.Length; i++)
         {
             hotdog[i].Stop();
         }
     }
 }
*/

}
