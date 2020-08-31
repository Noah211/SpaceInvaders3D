using System.Collections.Generic;
using UnityEngine;

// Author: Noah Logan

public class PlanetRotator : MonoBehaviour
{
    public float EarthSelfRotation;
    public float MercuryVenusSpawnAngle;
    public float EarthSpawnAngle;
    public float MarsSpawnAngle;
    public float JSUNSpawnAngle;

    private Matrix4x4 sunTranslationMatrix;
    private float earthRotationIncrement;
    private float time;
    private float timer;
    private LinkedList<Node> mercury;
    private LinkedList<Node> venus;
    private LinkedList<Node> earthAndMoon;
    private LinkedList<Node> mars;
    private LinkedList<Node> jupiter;
    private LinkedList<Node> saturn;
    private LinkedList<Node> uranus;
    private LinkedList<Node> neptune;

    void Start()
    {
        sunTranslationMatrix = Matrix4x4.Translate(GameObject.Find("Sun").transform.position);
        earthRotationIncrement = -(EarthSelfRotation / 365);
        time = 0;
        timer = 0.001f;
        mercury = new LinkedList<Node>();
        venus = new LinkedList<Node>();
        earthAndMoon = new LinkedList<Node>();
        mars = new LinkedList<Node>();
        jupiter = new LinkedList<Node>();
        saturn = new LinkedList<Node>();
        uranus = new LinkedList<Node>();
        neptune = new LinkedList<Node>();
        AddPlanet(GameObject.Find("Mercury"), mercury, EarthSelfRotation / 176, 80000, MercuryVenusSpawnAngle, earthRotationIncrement * 4.148f);        // Transform to Scale should be 325
        AddPlanet(GameObject.Find("Venus"), venus, -EarthSelfRotation / 242, 110000, MercuryVenusSpawnAngle, earthRotationIncrement * 2.062f);          // Transform to Scale should be 950
        AddPlanet(GameObject.Find("Earth"), earthAndMoon, EarthSelfRotation, 143000, EarthSpawnAngle, earthRotationIncrement);                          // Transform to Scale should be 1,000
        AddPlanet(GameObject.Find("Moon"), earthAndMoon, EarthSelfRotation / 27.3f, 2360, 0, earthRotationIncrement * 13.52f);                          // Transform to Scale should be 250
        AddPlanet(GameObject.Find("Mars"), mars, EarthSelfRotation * 1.028f, 170000, MarsSpawnAngle, earthRotationIncrement * 0.5313f);                 // Transform to Scale should be 500
        AddPlanet(GameObject.Find("Jupiter"), jupiter, EarthSelfRotation * 2.4f, 230000, JSUNSpawnAngle, earthRotationIncrement * 0.0841f);             // Transform to Scale should be 10,000
        AddPlanet(GameObject.Find("Saturn"), saturn, EarthSelfRotation * 2.251f, 300000, JSUNSpawnAngle, earthRotationIncrement * 0.0339f);             // Transform to Scale should be 9,000
        AddPlanet(GameObject.Find("Uranus"), uranus, EarthSelfRotation * 1.391f, 450000, JSUNSpawnAngle, earthRotationIncrement * 0.0119f);             // Transform to Scale should be 4,000
        AddPlanet(GameObject.Find("Neptune"), neptune, EarthSelfRotation * 1.5f, 600000, JSUNSpawnAngle, earthRotationIncrement * 0.0061f);             // Transform to Scale should be 3,250
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time > timer)
        {
            RotatePlanets(mercury);
            RotatePlanets(venus);
            RotatePlanets(earthAndMoon);
            RotatePlanets(mars);
            RotatePlanets(jupiter);
            RotatePlanets(saturn);
            RotatePlanets(uranus);
            RotatePlanets(neptune);
            time = 0;
        }
    }

    private void RotatePlanets(LinkedList<Node> planetLinkedList)
    {
        LinkedListNode<Node> currentNode = planetLinkedList.First;

        // Apply transformations to planets using hierarchical modeling
        while (currentNode != null)
        {
            GameObject currentPlanet = currentNode.Value.Planet;
            LinkedListNode<Node> node = currentNode;

            while (node != null)
            {
                List<Matrix4x4> matrices = node.Value.Matrices;
                Vector3 point = Vector3.zero;

                if ((currentNode != planetLinkedList.First) && (node.Next != null))
                {
                    point = currentPlanet.transform.position;
                }
                if (node == currentNode)
                {
                    currentPlanet.transform.Rotate(new Vector3(0, node.Value.SelfRotation, 0));                          // self rotation
                }

                currentPlanet.transform.position = matrices[0].MultiplyPoint3x4(point);                                  // translation
                currentPlanet.transform.position = matrices[1].MultiplyPoint3x4(currentPlanet.transform.position);       // rotation
                node = node.Previous;
            }
            currentPlanet.transform.position = sunTranslationMatrix.MultiplyPoint3x4(currentPlanet.transform.position);  // base translation
            currentNode = currentNode.Next;
        }
        UpdateAngles(planetLinkedList);
    }

    private void UpdateAngles(LinkedList<Node> planetLinkedList)
    {
        LinkedList<Node> temp = new LinkedList<Node>();

        while (planetLinkedList.Count > 0)
        {
            Node node = planetLinkedList.First.Value;
            temp.AddLast(new Node (node.Planet, node.SelfRotation, node.Translation, node.Rotation -= node.PlanetRotationIncrement, node.PlanetRotationIncrement, node.Matrices));
            planetLinkedList.RemoveFirst();
        }

        while (temp.Count > 0)
        {
            Node node = temp.First.Value;
            AddPlanet(node.Planet, planetLinkedList, node.SelfRotation, node.Translation, node.Rotation, node.PlanetRotationIncrement);
            temp.RemoveFirst();
        }
    }

    private void AddPlanet(GameObject planet, LinkedList<Node> planetLinkedList, float selfRotation, float translation, float rotation, float planetRotationIncrement)
    {
        Matrix4x4 translationMatrix = Matrix4x4.Translate(new Vector3(0, 0, translation));
        Quaternion rotationQuaternion = Quaternion.Euler(0, rotation, 0);
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(rotationQuaternion);
        List<Matrix4x4> matrices = new List<Matrix4x4>
        {
            translationMatrix,
            rotationMatrix
        };

        Node node = new Node(planet, selfRotation, translation, rotation, planetRotationIncrement, matrices);
        planetLinkedList.AddLast(node);
    }

    private struct Node
    {
        public GameObject Planet;
        public float SelfRotation;
        public float Translation;
        public float Rotation;
        public float PlanetRotationIncrement;
        public List<Matrix4x4> Matrices;

        public Node(GameObject planet, float selfRotation, float translation, float rotation, float planetRotationIncrement, List<Matrix4x4> matrices)
        {
            Planet = planet;
            SelfRotation = selfRotation;
            Translation = translation;
            Rotation = rotation;
            PlanetRotationIncrement = planetRotationIncrement;
            Matrices = matrices;
        }
    }
}
