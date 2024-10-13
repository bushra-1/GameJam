using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationManager : MonoBehaviour
{
    public GameObject stationPrefab; // �stasyon prefab�
    public int numberOfStations = 5; // Olu�acak istasyon say�s�
    public float initialStationDistance = 5f; // �lk istasyonun roketten uzakl���
    public float stationSpacing = 15f; // Di�er istasyonlar aras�ndaki mesafe
    public Transform minTransform; // Minimum pozisyon
    public Transform maxTransform; // Maksimum pozisyon
    public GameObject rocket; // Roket objesi
    public float landingDistance = 2f; // �ni� yap�labilmesi i�in gereken mesafe
    public float fuelRefillAmount = 100f; // Yak�t doldurma miktar�
    public float stationSapwnTime = 30f;
    private float timer;

    private Vector2 stationPosition;
    private Vector2 prevStationPosition;

    private List<GameObject> stations = new List<GameObject>(); // �stasyonlar� tutan liste

    void Update()
    {
        CheckForLanding(); // Her karede ini� kontrol� yap

        if (Input.GetKey(KeyCode.W))
            timer += Time.deltaTime;

        if (timer > stationSapwnTime)
        {
            timer = 0;
            CreateStations();
        }

    }

    // �stasyonlar� hemen roketin ilk birka� hareketinde ula�abilece�i yak�nl�kta olu�turur
    void CreateStations()
    {
        
        int rand = Random.Range(0, 2);

        // �lk istasyonu roketin biraz �st�ne yak�n bir yere yerle�tir
        if (rand == 0)
        {
            stationPosition = minTransform.position;
        }
        // Di�er istasyonlar, sabit aral�klarla daha yukar�da olu�turulur
        else
        {
            stationPosition = maxTransform.position;
        }


        if (stationPosition.y <= prevStationPosition.y)
            return;
        // �stasyon prefab�n� yarat ve listeye ekle
        GameObject newStation = Instantiate(stationPrefab, stationPosition, Quaternion.identity);
        stations.Add(newStation);

        prevStationPosition = stationPosition;
    }

    // Roketin bir platforma ini� yap�p yapmad���n� kontrol eder
    void CheckForLanding()
    {
        foreach (GameObject station in stations)
        {
            // Roketin platforma olan mesafesini kontrol et
            float distanceToStation = Vector2.Distance(rocket.transform.position, station.transform.position);

            // E�er roket bir platforma yeterince yak�nsa ve "S" tu�una bas�l�rsa ini� yap
            if (distanceToStation < landingDistance && Input.GetKey(KeyCode.S))
            {
                // Roketin "GameJam" script'ine ula� ve yak�t� doldur
                GameJam rocketScript = rocket.GetComponent<GameJam>();
                if (rocketScript != null)
                {
                    // Yak�t� doldur
                    rocketScript.fuel = fuelRefillAmount;
                    Debug.Log("Platforma ini� yapt�n�z, yak�t doldu!");
                }
                else
                {
                    Debug.LogError("Roket �zerinde 'GameJam' script'i bulunamad�!");
                }
            }
        }
    }
}
