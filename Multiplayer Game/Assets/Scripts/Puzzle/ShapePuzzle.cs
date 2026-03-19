using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShapePuzzle : MonoBehaviour
{
    public List<ShapesToRandomize> ShapesToScroll;
    public bool SpawnPuzzleObjects;
    public bool RemovePuzzle;
    public List<int> PuzzleObjects;
    int CorrectShapeCount;
    bool UpdateShape;
    Animator anim;
    public string animationTrigger;
    public TextMeshPro RequiredText;
    private void Start()
    {
        anim = GetComponent<Animator>(); 
        SpawnPuzzle();
    }
    private void Update()
    {
        if (SpawnPuzzleObjects)
        {
            SpawnPuzzle();
            SpawnPuzzleObjects = false;
        }
        if(UpdateShape)
        {
            CorrectShapeCount = 0;
            for (int i = 0;i < PuzzleObjects.Count;i++)
            {
                if (ShapesToScroll[i].ObjectSpawned == PuzzleObjects[i])
                {
                    CorrectShapeCount++;
                }
            }
            UpdateShape = false;
        }
        if(CorrectShapeCount == PuzzleObjects.Count)
        {
            if(anim != null)
            {
                anim.SetBool(animationTrigger, true);
            }
        }
        else
        {
            if (anim != null)
            {
                anim.SetBool(animationTrigger, false);
            }
        }
    }
    public void ScrollToNextObject(bool Add, int ShapeID)
    {
        for (int i = 0; i < ShapesToScroll.Count; i++)
        {
            for (int j = 0; j < ShapesToScroll[ShapeID].PuzzleObjects.Count; j++)
            {
                ShapesToScroll[ShapeID].PuzzleObjects[j].SetActive(false);
            }
        }
        UpdateShape = true;
        if(Add)
        {
            if (ShapesToScroll[ShapeID].ObjectSpawned < ShapesToScroll[ShapeID].PuzzleObjects.Count)
            {
                ShapesToScroll[ShapeID].ObjectSpawned++;
                int ObjectToSpawn = ShapesToScroll[ShapeID].ObjectSpawned;
                for (int j = 0; j < ObjectToSpawn; j++)
                {
                    ShapesToScroll[ShapeID].PuzzleObjects[j].SetActive(true);
                }
            }
            else if (ShapesToScroll[ShapeID].ObjectSpawned >= ShapesToScroll[ShapeID].PuzzleObjects.Count)
            {
                int ObjectToSpawn = ShapesToScroll[ShapeID].ObjectSpawned = 1;
                for (int j = 0; j < ObjectToSpawn; j++)
                {
                    ShapesToScroll[ShapeID].PuzzleObjects[j].SetActive(true);
                }
            }
        }
        else
        {
            if (ShapesToScroll[ShapeID].ObjectSpawned <= 1)
            {
                int ObjectToSpawn = ShapesToScroll[ShapeID].ObjectSpawned = 3;
                for (int j = 0; j < ObjectToSpawn; j++)
                {
                    ShapesToScroll[ShapeID].PuzzleObjects[j].SetActive(true);
                }
            }
            else if (ShapesToScroll[ShapeID].ObjectSpawned <= ShapesToScroll[ShapeID].PuzzleObjects.Count)
            {
                ShapesToScroll[ShapeID].ObjectSpawned--;
                int ObjectToSpawn = ShapesToScroll[ShapeID].ObjectSpawned;
                for (int j = 0; j < ObjectToSpawn; j++)
                {
                    ShapesToScroll[ShapeID].PuzzleObjects[j].SetActive(true);
                }
            }
        }

    }
    public void SpawnPuzzle()
    {
        RequiredText.text = string.Empty;
        for (int i = 0; i < PuzzleObjects.Count; i++)
        {
            PuzzleObjects[i] = Random.Range(1, PuzzleObjects.Count+1);
        }
        for (int i = 0; i < ShapesToScroll.Count; i++)
        {
            int ObjectToSpawn = Random.Range(1, ShapesToScroll[i].PuzzleObjects.Count+1);
            for (int j = 0; j < ObjectToSpawn; j++)
            {
                ShapesToScroll[i].PuzzleObjects[j].SetActive(true);
            }
            ShapesToScroll[i].ObjectSpawned = ObjectToSpawn;
            if (ShapesToScroll[i].ObjectSpawned == PuzzleObjects[i])
            {
                    int ObjectToSpawnRedo = Random.Range(1, ShapesToScroll[i].PuzzleObjects.Count + 1);
                for (int j = 0; j < PuzzleObjects.Count; j++)
                {
                    ShapesToScroll[i].PuzzleObjects[j].SetActive(false);
                }
                for (int j = 0; j < ObjectToSpawnRedo; j++)
                {
                    ShapesToScroll[i].PuzzleObjects[j].SetActive(true);
                }
                ShapesToScroll[i].ObjectSpawned = ObjectToSpawnRedo;
            }
        }
        int object1 = PuzzleObjects[0];
        int object2 = PuzzleObjects[1];
        int object3 = PuzzleObjects[2];
        RequiredText.text = object1.ToString() + " " + ShapesToScroll[0].PuzzleObjects[0].name + Environment.NewLine + object2.ToString() + " " + ShapesToScroll[1].PuzzleObjects[0].name + Environment.NewLine + object3.ToString() + " " + ShapesToScroll[2].PuzzleObjects[0].name;
    }
}
[Serializable]
public class ShapesToRandomize
{
    public int ObjectSpawned;
    public List<GameObject> PuzzleObjects;
}
