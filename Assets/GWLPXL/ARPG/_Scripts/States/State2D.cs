
using UnityEngine;

namespace GWLPXL.ARPGCore.States.com
{


    public class State2D : MonoBehaviour, I2DStateMachine
    {
        public Rigidbody2D Rb2D = null;
        GlobalFacingDirection facing = GlobalFacingDirection.Down;
        GlobalMoveDirection walkDirection = GlobalMoveDirection.Down;


       
        public GlobalFacingDirection GetFacingDirection() => facing;
     

        public Vector3 GetFacingVector()
        {
            switch (facing)
            {
                case GlobalFacingDirection.Down:
                    return new Vector3(0, -1, 0);
                case GlobalFacingDirection.Up:
                    return new Vector3(0, 1, 0);
                case GlobalFacingDirection.Right:
                    return new Vector3(1, 0, 0);
                case GlobalFacingDirection.Left:
                    return new Vector3(-1, 0, 0);
                case GlobalFacingDirection.DownLeft:
                    return new Vector3(-1, -1, 0);
                case GlobalFacingDirection.DownRight:
                    return new Vector3(1, -1, 0);
                case GlobalFacingDirection.UpLeft:
                    return new Vector3(-1, 1, 0);
                case GlobalFacingDirection.UpRight:
                    return new Vector3(1, 1, 0);
            }
            return new Vector3(0, 0, 0);
        }

        public Rigidbody2D GetRigidbody() => Rb2D;


        public GlobalMoveDirection GetWalkingDirection() => walkDirection;


        public void SetFacingDirection(GlobalFacingDirection newDirection) => facing = newDirection;
        

        public void SetFacingDirection(Vector3 raw)
        {
            float x = raw.x;
            float y = raw.y;

            if (x == 0 && y > 0)
            {
                facing = GlobalFacingDirection.Up;
            }
            else if (x == 0 && y < 0)
            {
                facing = GlobalFacingDirection.Down;
            }
            else if (x > 0 && y == 0)
            {
                facing = GlobalFacingDirection.Right;
            }
            else if (x < 0 && y == 0)
            {
                facing = GlobalFacingDirection.Left;
            }
            else if (x < 0 && y < 0)
            {
                facing = GlobalFacingDirection.DownLeft;
            }
            else if (x < 0 && y > 0)
            {
                facing = GlobalFacingDirection.UpLeft;
            }
            else if (x > 0 && y < 0)
            {
                facing = GlobalFacingDirection.DownRight;
            }
            else if (x > 0 && y > 0)
            {
                facing = GlobalFacingDirection.UpRight;
            }
        }

        public void SetWalkingDirection(GlobalMoveDirection newDirection) => walkDirection = newDirection;


        public void SetWalkingDirection(Vector3 raw)
        {
            float x = raw.x;
            float y = raw.y;

            if (x == 0 && y > 0)
            {
                walkDirection = GlobalMoveDirection.Up;
            }
            else if (x == 0 && y < 0)
            {
                walkDirection = GlobalMoveDirection.Down;
            }
            else if (x > 0 && y == 0)
            {
                walkDirection = GlobalMoveDirection.Right;
            }
            else if (x < 0 && y == 0)
            {
                walkDirection = GlobalMoveDirection.Left;
            }
            else if (x < 0 && y < 0)
            {
                walkDirection = GlobalMoveDirection.DownLeft;
            }
            else if (x < 0 && y > 0)
            {
                walkDirection = GlobalMoveDirection.UpLeft;
            }
            else if (x > 0 && y < 0)
            {
                walkDirection = GlobalMoveDirection.DownRight;
            }
            else if (x > 0 && y > 0)
            {
                walkDirection = GlobalMoveDirection.UpRight;
            }

           
        }
    }
}