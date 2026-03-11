using UnityEngine;

public class EscapeShipScreenStripeAnimator : MonoBehaviour
{
    [SerializeField] Material scrollingMaterial;
    [Header ("Flip value to reverse direction")]
    [SerializeField] float animationScrollSpeed = -0.003f;
    //this script animates the warning stripes in the space ship on the middle monitor
    private void FixedUpdate()
    {
        scrollingMaterial.mainTextureOffset += new Vector2(animationScrollSpeed, 0);
    }
    //resets value when runtime stops
    private void OnApplicationQuit()
    {
        scrollingMaterial.mainTextureOffset = Vector2.zero;
    }
}
