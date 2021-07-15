using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public Tilemap Tiles;
    public TileBase Block, Finish, Ice, Lava, Portal; 
    public GameObject Player;
    public string[,] layout = new string[20,15];
    public static TileManager Instance;
    public static Vector2 Portal1, Portal2;
    string LevelData;
    int currentpuzzle = 25;
    string levelslist; 
    string[] levels;
    // Start is called before the first frame update
    void Start(){
        Instance = this;
        levelslist = Resources.Load<TextAsset>("levels").ToString();
        levels = levelslist.Split('\n');
        LoadPuzzle(levels[currentpuzzle]);
    }
    public void LoadPuzzle(string level)
    {
        Controls[] ExistingPlayers = GameObject.FindObjectsOfType<Controls>();
        LevelData = level;
        for (int i=0; i<ExistingPlayers.Length; i++){
            GameObject.DestroyImmediate(ExistingPlayers[i].gameObject);
        }
        Portal1 = Vector2.zero;
        for (int i=0; i<level.Length; i++ ){
            int row = 14-(int)Mathf.Floor(i/20);
            int column = i%20;
            string currenttile = level.Substring(i, 1);
            if (currenttile == "S"){ // start
                layout[column,row] = "0";
                GameObject newplayer = Instantiate(Player);
                ExistingPlayers = GameObject.FindObjectsOfType<Controls>();
                if (ExistingPlayers.Length == 2){
                    ExistingPlayers[0].otherplayer = ExistingPlayers[1];
                    ExistingPlayers[1].otherplayer = ExistingPlayers[0];
                }
                Transform PlayerPos = newplayer.GetComponent<Transform>();
                PlayerPos.position = new Vector3(column, row, 0);
            }
            else
                layout[column,row] = currenttile;
            if (currenttile == "B"){ // block
                Tiles.SetTile(new Vector3Int(column, row, 0), Block);
            }
            else if (currenttile == "F"){ // finish
                Tiles.SetTile(new Vector3Int(column, row, 0), Finish);
            }
            else if (currenttile == "I"){ // ice
                Tiles.SetTile(new Vector3Int(column, row, 0), Ice);
            }
            else if (currenttile == "L"){ // lava
                Tiles.SetTile(new Vector3Int(column, row, 0), Lava);
            }
            else if (currenttile == "P"){ // portal
                Tiles.SetTile(new Vector3Int(column, row, 0), Portal);
                if (Portal1 == Vector2.zero){
                    Portal1 = new Vector2(column, row);
                }
                else {
                    Portal2 = new Vector2(column, row);
                }
            }
        }
    }
    public void NextLevel(){
        currentpuzzle++;
        Tiles.ClearAllTiles();
        LoadPuzzle(levels[currentpuzzle]);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)){
            LoadPuzzle(levels[currentpuzzle]);
        }
    }

}
