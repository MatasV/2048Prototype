using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(GridLayoutGroup))]
public class GridController : MonoBehaviour
{
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private GameObject blockGo;
    [SerializeField] private SharedInt score;
    
    private GridLayoutGroup gridLayoutGroup;
    private enum MoveDirection {Left, Right, Up, Down}
    private BlockHolder[,] grid;

    [SerializeField] private float animationSpeed = 0.2f;

    private void Start()
    {
        grid = new BlockHolder[gameSettings.rows, gameSettings.columns];
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        
        Initialize();
    }

    private void Initialize()
    {
        score.Value = 0;
        
        for (int i = 0; i < gameSettings.rows; i++)
        {
            for (int j = 0; j < gameSettings.columns; j++)
            {
                var blockHolder = Instantiate(blockGo, transform).GetComponent(typeof(BlockHolder)) as BlockHolder;
                grid[i,j] = blockHolder;
            }
        }
        
        SpawnBlock();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) Move(MoveDirection.Left);
        else if (Input.GetKeyDown(KeyCode.RightArrow)) Move(MoveDirection.Right);
        else if (Input.GetKeyDown(KeyCode.UpArrow)) Move(MoveDirection.Up);
        else if (Input.GetKeyDown(KeyCode.DownArrow)) Move(MoveDirection.Down);
    }

    private void Loss()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SpawnBlock()
    {
        var freeBlockHolders = new List<BlockHolder>();
        
        for (int i = 0; i < gameSettings.rows; i++)
        {
            for (int j = 0; j < gameSettings.columns; j++)
            {
                var blockHolder = grid[i, j];
                if (blockHolder.Value == 0)
                {
                    freeBlockHolders.Add(blockHolder);
                }
            }
        }

        if (freeBlockHolders.Count == 0)
        {
            Loss();
        }
        else
        {
            var spawnInBlock = Random.Range(0, freeBlockHolders.Count);
            freeBlockHolders[spawnInBlock].Value = Random.Range(0, 10) >= 5 ? 2 : 4;
        }
    }

    private IEnumerator MoveBlockAnimation(BlockHolder holder, Vector3 endPosition)
    {
        var startingPosition = holder.transform.position;

        var blockHolderGO = Instantiate(blockGo, startingPosition, Quaternion.identity, transform);
        blockHolderGO.AddComponent<LayoutElement>().ignoreLayout = true;
        
        var blockHolder = blockHolderGO.GetComponent(typeof(BlockHolder)) as BlockHolder;

        blockHolder.Value = holder.Value;

        for (float i = 0; i <= 1; i += animationSpeed)
        {
            blockHolder.transform.position = Vector3.Lerp(startingPosition, endPosition, i);
            yield return null;
        }
        
        Destroy(blockHolderGO);
    }
    private void Move(MoveDirection direction)
    {
        switch (direction)
        {
            case MoveDirection.Left:
                for (int i = 0; i < gameSettings.rows; i++)
                {
                    for (int j = 1; j < gameSettings.columns; j++)
                    {
                        if (grid[i, j].Value == 0) continue;
                        
                        for (int k = j; k > 0; k--)
                        {
                            if (grid[i, k - 1].Value == 0)
                            {
                                StartCoroutine(MoveBlockAnimation(grid[i, k], grid[i, k - 1].transform.position));
                                grid[i, k - 1].Value = grid[i, k].Value;
                                grid[i, k].Value = 0;
                            }
                            else if (grid[i, k - 1].Value == grid[i, k].Value)
                            {
                                StartCoroutine(MoveBlockAnimation(grid[i, k], grid[i, k - 1].transform.position));
                                grid[i, k-1].Value *= 2;
                                score.Value += grid[i , k - 1].Value;
                                grid[i, k].Value = 0;
                                break;
                            }
                        }
                    }
                }
                break;
            case MoveDirection.Right:
                for (int i = 0; i < gameSettings.rows; i++)
                {
                    for (int j = gameSettings.columns - 2; j >= 0; j--)
                    {
                        if (grid[i, j].Value == 0) continue;
                        for (int k = j; k < gameSettings.columns - 1; k++)
                        {
                            if (grid[i, k + 1].Value == 0) 
                            {
                                StartCoroutine(MoveBlockAnimation(grid[i, k], grid[i, k + 1].transform.position));
                                grid[i, k + 1].Value = grid[i, k].Value;
                                grid[i, k].Value = 0;
                            }
                            else if (grid[i, k +1].Value == grid[i, k].Value)
                            {
                                StartCoroutine(MoveBlockAnimation(grid[i, k], grid[i, k + 1].transform.position));
                                grid[i, k + 1].Value *= 2;
                                score.Value += grid[i , k + 1].Value;
                                grid[i, k].Value = 0;
                                break;
                            }
                        }
                    }
                }
                break;
            case MoveDirection.Up:
                for (int i = 0; i < gameSettings.rows; i++)
                {
                    for (int j = 0; j < gameSettings.columns; j++)
                    {
                        if (grid[i, j].Value == 0) continue;
                        
                        for (int k = i; k > 0; k--)
                        {
                            if (grid[k - 1, j].Value == 0)
                            {
                                StartCoroutine(MoveBlockAnimation(grid[k, j], grid[k - 1, j].transform.position));
                                grid[k - 1, j].Value = grid[k, j].Value;
                                grid[k, j].Value = 0;
                            }
                            else if (grid[k - 1, j].Value == grid[k, j].Value)
                            {
                                StartCoroutine(MoveBlockAnimation(grid[k, j], grid[k - 1, j].transform.position));
                                grid[k - 1, j].Value *= 2;
                                score.Value += grid[k - 1, j].Value;
                                grid[k, j].Value = 0;
                                break;
                            }
                        }
                    }
                }
                break;
            case MoveDirection.Down:
                for (int i = gameSettings.rows - 2; i >= 0; i--)
                {
                    for (int j = 0; j < gameSettings.columns; j++)
                    {
                        if (grid[i, j].Value == 0) continue;
                        
                        for (int k = i; k < gameSettings.rows - 1; k++)
                        {
                            if (grid[k + 1, j].Value == 0)
                            {
                                StartCoroutine(MoveBlockAnimation(grid[k, j], grid[k + 1, j].transform.position));
                                grid[k + 1, j].Value = grid[k, j].Value;
                                grid[k, j].Value = 0;
                            }
                            else if (grid[k + 1, j].Value == grid[k, j].Value)
                            {
                                StartCoroutine(MoveBlockAnimation(grid[k, j], grid[k + 1, j].transform.position));
                                grid[k + 1, j].Value *= 2;
                                score.Value += grid[k + 1, j].Value;
                                grid[k, j].Value = 0;
                                break;
                            }
                        }
                    }
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, "Unknown Direction");
        }

        SpawnBlock();
    }
}
