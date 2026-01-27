using UnityEngine;
using System.Collections.Generic;

public class RenderManager : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private List<Renderer> renderers;

    void Update()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);

        foreach (Renderer rend in renderers)
        {
            if (rend == null) continue;

            float dist = Vector3.Distance(cam.transform.position, rend.transform.position);

            if (dist <= maxDistance)
            {
                bool isVisible = GeometryUtility.TestPlanesAABB(planes, rend.bounds);
                rend.enabled = isVisible;
            }
            else
            {
                rend.enabled = false;
            }
        }
    }
}