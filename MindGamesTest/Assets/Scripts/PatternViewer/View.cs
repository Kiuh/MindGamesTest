using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PatternViewer
{
    [AddComponentMenu("Scripts/PatternViewer/PatternViewer.View")]
    internal class View : MonoBehaviour
    {
        [SerializeField]
        private Vector2 shift;

        [SerializeField]
        private GameObject tilePrefab;

        [SerializeField]
        private List<GameObject> tiles;

        [SerializeField]
        private Model model;

        private void Start()
        {
            model.OnMatrixChanged += UpdateMatrixView;
        }

        private void UpdateMatrixView()
        {
            while (tiles.Count > 0)
            {
                Destroy(tiles.First());
                tiles.RemoveAt(0);
            }

            for (int i = 0; i < model.Size.x; i++)
            {
                for (int j = 0; j < model.Size.y; j++)
                {
                    if (!model.GetMatrixValue(i, j))
                    {
                        continue;
                    }

                    GameObject newTile = Instantiate(
                        tilePrefab,
                        transform.position + new Vector3(shift.x * i, shift.y * j, 0),
                        Quaternion.identity,
                        transform
                    );
                    tiles.Add(newTile);
                }
            }
        }
    }
}
