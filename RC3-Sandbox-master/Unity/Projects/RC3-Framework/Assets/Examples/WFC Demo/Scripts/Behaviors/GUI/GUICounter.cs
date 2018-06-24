using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RC3.Graphs;

namespace RC3.Unity.WFCDemo
{
    public class GUICounter : MonoBehaviour
    {
        [SerializeField] private SharedDigraph _grid;
        [SerializeField] private TileSet _tileSet;
        [SerializeField] private GUISkin mySkin;

        private List<VertexObject> _vertices;
        private Digraph _graph;
        private int _unassigned;
        private int[] _list;

      
        private void Start()
        {
            _list = new int[_tileSet.Count];
            _graph = _grid.Graph;
            _vertices = _grid.VertexObjects;
            _unassigned = _graph.VertexCount;
           // _tileSet = GetComponent<TileModelManager>().TileSet;

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
                CountTiles();

            if (Input.GetKeyDown(KeyCode.R))
                ResetTiles();

            if (Input.GetKeyDown(KeyCode.Space))
                RotateView();

        }
        private void RotateView()
        {
            if (gameObject.GetComponent<ModelDisplay>() == null)
            {
                gameObject.AddComponent<ModelDisplay>();
            }
            else
            {
                Destroy(gameObject.GetComponent<ModelDisplay>());
            }
        }

        void ResetTiles()
        {
            for (int i = 0; i < _list.Length; i++)
                _list[i] = 0;

            _unassigned = _graph.VertexCount;
        }

        void CountTiles()
        {
            foreach (var v in _vertices)
            {
                for (int i = 0; i < _tileSet.Count; i++)
                { if (v.Tile == _tileSet[i]) { _list[i]++; _unassigned--; break; } }
            }

        }

        private void OnGUI()
        {
            GUI.skin = mySkin;


            //GUI.Label(new Rect(new Vector2(35, 370), new Vector2(350, 100)), "AddJoints(); CheckLowest() = J"); //StoreTilesWithMeshes(); 
            //GUI.Label(new Rect(new Vector2(35, 390), new Vector2(350, 100)), " AddGravity(); = G");
            //GUI.Label(new Rect(new Vector2(35, 410), new Vector2(350, 100)), " RotateView(); = Space");
            //GUI.Label(new Rect(new Vector2(35, 430), new Vector2(350, 100)), " DeleteEmpty(); = E");
            //GUI.Label(new Rect(new Vector2(35, 450), new Vector2(350, 100)), " Count tiles by type = C");

            GUI.Label(new Rect(new Vector2(Screen.width - 120, 100), new Vector2(250, 100)), "graph capacity : " + _graph.VertexCount.ToString());
            GUI.Label(new Rect(new Vector2(Screen.width - 120, 120), new Vector2(250, 100)), "unassigned : " + _unassigned.ToString());

            for (int i = 0; i < _tileSet.Count; i++)
            {
                if (_list[i] > 0)
                    GUI.Label(new Rect(new Vector2(Screen.width - 120, 140 + 20 * i), new Vector2(250, 100)), "tile type " + i + " : " + _list[i].ToString());
            }

        }
    }
}
