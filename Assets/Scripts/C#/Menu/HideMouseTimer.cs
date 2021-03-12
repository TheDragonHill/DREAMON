using UnityEngine;

public class HideMouseTimer : MonoBehaviour
{
    [SerializeField]
    float showMouseDuration = 1; 

    Vector3 lastCursorPosition;

    float timer;
    bool timerSet = false;

    public delegate void MouseShow();
    public event MouseShow OnMouseShow;


    void Start()
    {
        Cursor.visible = false;
        lastCursorPosition = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        CheckMouseMovement();
    }

    void CheckMouseMovement()
    {
        if (!Cursor.visible && lastCursorPosition != Input.mousePosition)
        {
            Cursor.visible = true;
            //Invoke Event
            OnMouseShow();
        }
        else
        {
            // Check Position
            if(lastCursorPosition == Input.mousePosition)
            {
                if (!timerSet)
                {
                    timerSet = true;
                    timer = Time.time;
                }
                // Check Timer
                else if (Time.time - timer >= showMouseDuration)
                {
                    Cursor.visible = false;
                    timerSet = false;
                    // Invoke Event
                    OnMouseShow();
                }

            }
            else
            {
                timerSet = false;
            }

            lastCursorPosition = Input.mousePosition;
        }
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }

}
