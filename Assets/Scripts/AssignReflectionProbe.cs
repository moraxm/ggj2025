using UnityEngine;

public class AssignReflectionProbe : MonoBehaviour
{
    public ReflectionProbe reflectionProbe; // Asigna el Reflection Probe desde el Inspector

    private void Start()
    {
        // Obtén el Skinned Mesh Renderer
        SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer == null)
        {
            Debug.LogError("No se encontró un Skinned Mesh Renderer en el objeto.");
            return;
        }

        // Verifica si el Reflection Probe tiene un cubemap
        if (reflectionProbe == null || reflectionProbe.bakedTexture == null)
        {
            Debug.LogError("Reflection Probe no asignado o no tiene textura horneada.");
            return;
        }

        // Crear el MaterialPropertyBlock
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

        // Asigna el cubemap generado por el Reflection Probe
        propertyBlock.SetTexture("_ReflectionProbeCubemap", reflectionProbe.bakedTexture);

        // Aplica las propiedades al Skinned Mesh Renderer
        skinnedMeshRenderer.SetPropertyBlock(propertyBlock);
    }
}
