using UnityEngine;

public class EffectDestroy : MonoBehaviour
{
    public void AnimationEnd()
    {
        Destroy(gameObject);
    }
}
