using UnityEngine;

public class PathFollower : MonoBehaviour
{
    // Array of path nodes 
    Node[] PathNode;

    // speed of the gameObject movement
    public float movementSpeed;

    // speed of transition from a point to another
    float timer;

    // the current node where the gameobject should go
    int currentNodeIndex = 0;

    // the current node position
    Vector3 currentPosHolder;

    // the start position of the gameobject
    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        PathNode = GameObject.FindGameObjectWithTag("Path").GetComponentsInChildren<Node>();
        //foreach(Node n in PathNode)
        //{
        //    Debug.Log(n.name);
        //}
        CheckNode();
    }

    /**
     * <summary>
     * This method gets the new node and saves its position.
     * The position is the next position where the gameobject should move to
     * </summary>
     */
    void CheckNode()
    {
        timer = 0;
        currentPosHolder = PathNode[currentNodeIndex].transform.position;
        startPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        DrawLine();
        //Debug.Log(currentNodeIndex);
        // time of the lerping
        timer += Time.deltaTime / movementSpeed;
        if (gameObject.transform.position != currentPosHolder)
        {
            // move gameobject to node
            gameObject.transform.position = Vector3.MoveTowards(transform.position, currentPosHolder, Time.deltaTime * movementSpeed);
        }
        else
        {
            // if we are not to the last node
            if (currentNodeIndex < PathNode.Length - 1)
            {
                // go to the next one
                currentNodeIndex++;
                // reset timer and get next node
                CheckNode();
            }
        }
    }

    /**
     * <summary>
     * Shows a debug line between the dots
     * </summary> 
     */
    void DrawLine()
    {
        for (int i = 0; i < PathNode.Length; i++)
        {
            if (i < PathNode.Length - 1)
            {
                Debug.DrawLine(PathNode[i].transform.position, PathNode[i + 1].transform.position, Color.green);
            }
        }
    }
}
