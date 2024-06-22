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
        private GameObject backgroundPrefab;

        [SerializeField]
        private List<GameObject> tiles;

        [SerializeField]
        private Model model;

        private void Awake()
        {
            model.OnGridChanged += UpdateGridView;
        }

        private void UpdateGridView()
        {
            while (tiles.Count > 0)
            {
                Destroy(tiles.First());
                tiles.RemoveAt(0);
            }

            foreach (Common.GridCell cell in model.Grid)
            {
                Vector3 position =
                    transform.position
                    + new Vector3(shift.x * cell.Position.x, shift.y * cell.Position.y, 0);

                GameObject back = Instantiate(
                    backgroundPrefab,
                    position,
                    Quaternion.identity,
                    transform
                );
                tiles.Add(back);

                if (cell.Busy)
                {
                    GameObject newTile = Instantiate(
                        tilePrefab,
                        position,
                        Quaternion.identity,
                        transform
                    );
                    tiles.Add(newTile);
                }
            }
        }
    }
}
