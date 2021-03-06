// credits to Opaque Objects Making It Hard to See? Fade Them Out With the Standard Shader | Unity Tutorial
// https://www.youtube.com/watch?v=dIC4wbUgt5M&t=818s

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObjectBlockingObject : MonoBehaviour
{
    public LayerMask LayerMask;
    public Transform Player;
    public Camera Camera;

    public float FadedAlphaFar = 0f;
    public float FadedAlphaClose = 0.25f;

    public FadeMode FadingMode;
    public float ChecksPerSecond = 10;
    public int FadeFPS = 30;
    public float FadeSpeed = 10;

    public float radius = 2f;
    public float threshold = 4f;
    public float closeDistance = 5f;

    [Header("Read Only Data")]
    [SerializeField]
    private List<FadingObject> ObjectsBlockingView = new List<FadingObject>();

    private List<int> IndexesToClear = new List<int>();
    private Dictionary<FadingObject, Coroutine> RunningCoroutines = new Dictionary<FadingObject, Coroutine>();

    private RaycastHit[] Hits = new RaycastHit[20];

    private void Start()
    {
        StartCoroutine(CheckForObjects());
    }

    private IEnumerator CheckForObjects()
    {
        WaitForSeconds Wait = new WaitForSeconds(1f / ChecksPerSecond);

        while (true)
        {
            int hits = Physics.SphereCastNonAlloc(Camera.transform.position, radius, (Player.transform.position - Camera.transform.position).normalized, Hits, Vector3.Distance(Camera.transform.position, Player.transform.position) - threshold, LayerMask);
            //Debug.Log(hits);
            if (hits > 0)
            {
                for (int i = 0; i < hits; i++)
                {
                    FadingObject fadingObject = GetFadingObjectFromHit(Hits[i]);
                    //Debug.Log(Hits[i].collider.gameObject.GetComponent<Terrain>().terrainData.treeInstances.Length);
                    if (fadingObject != null && !ObjectsBlockingView.Contains(fadingObject))
                    {
                        if (RunningCoroutines.ContainsKey(fadingObject))
                        {
                            if (RunningCoroutines[fadingObject] != null) // may be null if it's already ended
                            {
                                StopCoroutine(RunningCoroutines[fadingObject]);
                            }

                            RunningCoroutines.Remove(fadingObject);
                        }

                        RunningCoroutines.Add(fadingObject, StartCoroutine(FadeObjectOut(fadingObject)));
                        ObjectsBlockingView.Add(fadingObject);
                    }
                }
            }

            FadeObjectsNoLongerBeingHit();

            ClearHits();

            yield return Wait;
        }
    }

    private void FadeObjectsNoLongerBeingHit()
    {
        for (int i = 0; i < ObjectsBlockingView.Count; i++)
        {
            bool objectIsBeingHit = false;
            for (int j = 0; j < Hits.Length; j++)
            {
                FadingObject fadingObject = GetFadingObjectFromHit(Hits[j]);
                if (fadingObject != null && fadingObject == ObjectsBlockingView[i])
                {
                    objectIsBeingHit = true;

                    if (fadingObject.Materials[0].color.a == FadedAlphaFar || fadingObject.Materials[0].color.a == FadedAlphaClose)
                    {
                        if (RunningCoroutines.ContainsKey(fadingObject))
                        {
                            if (RunningCoroutines[fadingObject] != null) // may be null if it's already ended
                            {
                                StopCoroutine(RunningCoroutines[fadingObject]);
                            }

                            RunningCoroutines.Remove(fadingObject);
                        }

                        RunningCoroutines.Add(fadingObject, StartCoroutine(FadingObjectUpdate(fadingObject)));
                    }

                    break;
                }
            }

            if (!objectIsBeingHit)
            {
                if (RunningCoroutines.ContainsKey(ObjectsBlockingView[i]))
                {
                    if (RunningCoroutines[ObjectsBlockingView[i]] != null)
                    {
                        StopCoroutine(RunningCoroutines[ObjectsBlockingView[i]]);
                    }
                    RunningCoroutines.Remove(ObjectsBlockingView[i]);
                }

                RunningCoroutines.Add(ObjectsBlockingView[i], StartCoroutine(FadeObjectIn(ObjectsBlockingView[i])));
                ObjectsBlockingView.RemoveAt(i);
            }
        }
    }

    private IEnumerator FadeObjectOut(FadingObject FadingObject)
    {
        float waitTime = 1f / FadeFPS;
        WaitForSeconds Wait = new WaitForSeconds(waitTime);
        int ticks = 1;

        for (int i = 0; i < FadingObject.Materials.Count; i++)
        {
            FadingObject.Materials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha); // affects both "Transparent" and "Fade" options
            FadingObject.Materials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha); // affects both "Transparent" and "Fade" options
            FadingObject.Materials[i].SetInt("_ZWrite", 0); // disable Z writing
            if (FadingMode == FadeMode.Fade)
            {
                FadingObject.Materials[i].EnableKeyword("_ALPHABLEND_ON");
            }
            else
            {
                FadingObject.Materials[i].EnableKeyword("_ALPHAPREMULTIPLY_ON");
            }

            FadingObject.Materials[i].renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }

        if (FadingObject.Materials[0].HasProperty("_Color"))
        {
            float distance = Vector3.Distance(FadingObject.transform.position, Player.transform.position);
            float fadeAlpha = distance > closeDistance ? FadedAlphaFar : FadedAlphaClose;
            FadingObject.targetAlpha = fadeAlpha;
            while (FadingObject.Materials[0].color.a > fadeAlpha)
            {
                for (int i = 0; i < FadingObject.Materials.Count; i++)
                {
                    if (FadingObject.Materials[i].HasProperty("_Color"))
                    {
                        FadingObject.Materials[i].color = new Color(
                            FadingObject.Materials[i].color.r,
                            FadingObject.Materials[i].color.g,
                            FadingObject.Materials[i].color.b,
                            Mathf.Lerp(FadingObject.InitialAlpha, fadeAlpha, waitTime * ticks * FadeSpeed)
                        );
                    }
                }

                ticks++;
                yield return Wait;
            }
        }

        if (RunningCoroutines.ContainsKey(FadingObject))
        {
            StopCoroutine(RunningCoroutines[FadingObject]);
            RunningCoroutines.Remove(FadingObject);
        }
    }

    private IEnumerator FadeObjectIn(FadingObject FadingObject)
    {
        float waitTime = 1f / FadeFPS;
        WaitForSeconds Wait = new WaitForSeconds(waitTime);
        int ticks = 1;

        if (FadingObject.Materials[0].HasProperty("_Color"))
        {
            float distance = Vector3.Distance(FadingObject.transform.position, Player.transform.position);
            float fadeAlpha = distance > closeDistance ? FadedAlphaFar : FadedAlphaClose;
            FadingObject.targetAlpha = FadingObject.InitialAlpha;
            while (FadingObject.Materials[0].color.a < FadingObject.InitialAlpha)
            {
                for (int i = 0; i < FadingObject.Materials.Count; i++)
                {
                    if (FadingObject.Materials[i].HasProperty("_Color"))
                    {
                        FadingObject.Materials[i].color = new Color(
                            FadingObject.Materials[i].color.r,
                            FadingObject.Materials[i].color.g,
                            FadingObject.Materials[i].color.b,
                            Mathf.Lerp(fadeAlpha, FadingObject.InitialAlpha, waitTime * ticks * FadeSpeed)
                        );
                    }
                }

                ticks++;
                yield return Wait;
            }
        }

        for (int i = 0; i < FadingObject.Materials.Count; i++)
        {
            if (FadingMode == FadeMode.Fade)
            {
                FadingObject.Materials[i].DisableKeyword("_ALPHABLEND_ON");
            }
            else
            {
                FadingObject.Materials[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
            }
            FadingObject.Materials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            FadingObject.Materials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            FadingObject.Materials[i].SetInt("_ZWrite", 1); // re-enable Z Writing
            FadingObject.Materials[i].renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
        }

        if (RunningCoroutines.ContainsKey(FadingObject))
        {
            StopCoroutine(RunningCoroutines[FadingObject]);
            RunningCoroutines.Remove(FadingObject);
        }
    }

    private IEnumerator FadingObjectUpdate(FadingObject FadingObject)
    {
        float waitTime = 1f / FadeFPS;
        WaitForSeconds Wait = new WaitForSeconds(waitTime);
        int ticks = 1;

        if (FadingObject.Materials[0].HasProperty("_Color"))
        {
            float distance = Vector3.Distance(FadingObject.transform.position, Player.transform.position);
            float fadeAlpha = distance > closeDistance ? FadedAlphaFar : FadedAlphaClose;
            if (fadeAlpha > FadedAlphaFar)
            {
                FadingObject.targetAlpha = FadedAlphaClose;
                while (FadingObject.Materials[0].color.a < FadedAlphaClose)
                {
                    for (int i = 0; i < FadingObject.Materials.Count; i++)
                    {
                        if (FadingObject.Materials[i].HasProperty("_Color"))
                        {
                            FadingObject.Materials[i].color = new Color(
                                FadingObject.Materials[i].color.r,
                                FadingObject.Materials[i].color.g,
                                FadingObject.Materials[i].color.b,
                                Mathf.Lerp(FadingObject.Materials[i].color.a, FadedAlphaClose, waitTime * ticks * FadeSpeed)
                            );
                        }
                    }

                    ticks++;
                    yield return Wait;
                }
            }
            else if (fadeAlpha < FadedAlphaClose)
            {
                FadingObject.targetAlpha = FadedAlphaFar;
                while (FadingObject.Materials[0].color.a > FadedAlphaFar)
                {
                    for (int i = 0; i < FadingObject.Materials.Count; i++)
                    {
                        if (FadingObject.Materials[i].HasProperty("_Color"))
                        {
                            FadingObject.Materials[i].color = new Color(
                                FadingObject.Materials[i].color.r,
                                FadingObject.Materials[i].color.g,
                                FadingObject.Materials[i].color.b,
                                Mathf.Lerp(FadingObject.Materials[i].color.a, FadedAlphaFar, waitTime * ticks * FadeSpeed)
                            );
                        }
                    }

                    ticks++;
                    yield return Wait;
                }
            }
        }
    }

    private FadingObject GetFadingObjectFromHit(RaycastHit Hit)
    {
        return Hit.collider != null ? Hit.collider.GetComponent<FadingObject>() : null;
    }

    private void ClearHits()
    {
        RaycastHit hit = new RaycastHit();
        for (int i = 0; i < Hits.Length; i++)
        {
            Hits[i] = hit;
        }
    }

    public enum FadeMode
    {
        Transparent,
        Fade
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(Camera.transform.position, Player.transform.position);
        Gizmos.DrawWireSphere(Player.transform.position, radius);
    }
}