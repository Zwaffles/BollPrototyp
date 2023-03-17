using UnityEngine;
using UnityEditor;
using PathCreation;

public static class TrackCreatorHierarchy
{
    [MenuItem("GameObject/3D Object/Track Creator")]
    public static void CreateSample(MenuCommand menuCommand)
    {
        GameObject newObject = ObjectFactory.CreateGameObject("TrackCreator");

        newObject.AddComponent<PathCreator>();
        newObject.GetComponent<PathCreator>().bezierPath.Space = PathSpace.xy;
        newObject.GetComponent<PathCreator>().ChangeHandleScale(2.75f);

        newObject.AddComponent<TrackMeshCreator>();
        newObject.GetComponent<TrackMeshCreator>().pathCreator = newObject.GetComponent<PathCreator>();
        newObject.GetComponent<TrackMeshCreator>().roadMaterial = Resources.Load<Material>("Material/Dirt");
    }
}
