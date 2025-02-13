using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    [SerializeField] private GameObject _camera;
    [SerializeField] private float parallaxEffect;
    [SerializeField] private float moveSpeed;

    private float movement = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        movement += moveSpeed * (1 - parallaxEffect) * Time.deltaTime;

        float temp = _camera.transform.position.x * (1 - parallaxEffect) - movement;
        float dist = _camera.transform.position.x * parallaxEffect / 1000;

        transform.position = new Vector3(startpos + movement + dist, transform.position.y, transform.position.z);

        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    } 
}
