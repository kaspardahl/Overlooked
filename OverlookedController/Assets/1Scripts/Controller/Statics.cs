using System.Collections;
using System.Collections.Generic;

namespace KD 
{
public static class Statics
    {
        #region hash
        public static string horizontal = "horizontal";
        public static string vertical = "vertical";
        public static string special = "special";
        public static string specialType = "specialType";
        public static string onLocomotion = "onLocomotion";
        public static string Horizontal = "Horizontal";
        public static string Vertical = "Vertical";
        public static string jumpType = "jumpType";
        public static string Jump = "Jump";
        public static string onAir = "onAir";
        public static string mirrorJump = "mirrorJump";
        public static string incline = "incline";
        public static string Fire3 = "Fire3";
        public static string inSpecial = "inSpecial";
        public static string walkVault = "vault_over_walk_1";
        public static string runVault = "vault_over_run";
        public static string walk_up = "walk_up";
        public static string run_up = "run_up";
        public static string onSprint = "onSprint";
        #endregion

        #region Variables 
        //kan ændres for at passe bedre med animationer og karakterens flow
        public static float vaultCheckDistance = 2;
        public static float vaultCheckDistance_Run = 2.5f;
        public static float vaultSpeedWalking = 2;
        public static float walkUpSpeed = 1.8f;
        public static float vaultSpeedRunning = 4;
        public static float vaultSpeedIdle = 1;
        public static float climbMaxHeight = 2;
        public static float walkUpHeight = 1;
        public static float walkUpThreshold = 0.4f;
        #endregion

        #region Functions

        public static int GetAnimSpecialType(AnimSpecials i)
        {
            int r = 0;
            switch (i)
            {
                case AnimSpecials.runToStop:
                    r = 11;
                    break;
                case AnimSpecials.run:
                    r = 10;
                    break;
                case AnimSpecials.jump_idle:
                    r = 21;
                    break;
                case AnimSpecials.run_jump:
                    r = 22;
                    break;
                default:
                    break;
            }

            return r;
        }
        #endregion
    }

    public enum AnimSpecials
    {
        run, runToStop, jump_idle, run_jump
    }
}

