using UnityEngine;

namespace VRUtilities {

    public static class Utilities {
        public static string Prefix (this Hand hand) {
            return hand == Hand.LeftHand ? "L_" : "R_";
        }

    }
    
}