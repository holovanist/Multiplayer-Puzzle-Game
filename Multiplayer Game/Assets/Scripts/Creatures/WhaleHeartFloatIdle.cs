using UnityEngine;

public class WhaleHeartFloatIdle : MonoBehaviour
{
    [SerializeField] float moveSpeed = 0.01f;
    [SerializeField] float boostTime = 0.3f;
    [SerializeField] Vector3 restPositionLocal = Vector3.zero;
    [SerializeField] Vector3 jerkPositionLocal = Vector3.zero;

    bool goingToRest = true;
    float timer = 0;
    private void FixedUpdate()
    {
        //only check if it needs to change direction if its moving to the jerk point
        if (!goingToRest)
        {
            CheckMovementDirection();
        }
        Move();
    }
    private void Update()
    {
        timer += Time.deltaTime;
    }
    void Move()
    {
        // moves towards different target based off current state
        if (goingToRest)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, restPositionLocal, moveSpeed);
        }
        else
        {
            //has more move speed to get a more jerky effect.
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, jerkPositionLocal, moveSpeed * 4f);
        }
    }
    void CheckMovementDirection()
    {
        //if timer is bigger than the boost time, switch directions.
        if (timer >= boostTime)
        {
            goingToRest = true;
        }
    }
    public void BoostSpeed()
    {
        //this function is called by a animation event on the whale heart beating animation

        //resets parameters to renable the jerk
        goingToRest = false;
        timer = 0;
    }
}