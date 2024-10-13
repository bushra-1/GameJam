using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationManager : MonoBehaviour
{
    public GameObject stationPrefab; // Ýstasyon prefabý
    public int numberOfStations = 5; // Oluþacak istasyon sayýsý
    public float initialStationDistance = 5f; // Ýlk istasyonun roketten uzaklýðý
    public float stationSpacing = 15f; // Diðer istasyonlar arasýndaki mesafe
    public Transform minTransform; // Minimum pozisyon
    public Transform maxTransform; // Maksimum pozisyon
    public GameObject rocket; // Roket objesi
    public float landingDistance = 2f; // Ýniþ yapýlabilmesi için gereken mesafe
    public float fuelRefillAmount = 100f; // Yakýt doldurma miktarý
    public float stationSapwnTime = 30f;
    private float timer;

    private Vector2 stationPosition;
    private Vector2 prevStationPosition;

    private List<GameObject> stations = new List<GameObject>(); // Ýstasyonlarý tutan liste

    void Update()
    {
        CheckForLanding(); // Her karede iniþ kontrolü yap

        if (Input.GetKey(KeyCode.W))
            timer += Time.deltaTime;

        if (timer > stationSapwnTime)
        {
            timer = 0;
            CreateStations();
        }

    }

    // Ýstasyonlarý hemen roketin ilk birkaç hareketinde ulaþabileceði yakýnlýkta oluþturur
    void CreateStations()
    {
        
        int rand = Random.Range(0, 2);

        // Ýlk istasyonu roketin biraz üstüne yakýn bir yere yerleþtir
        if (rand == 0)
        {
            stationPosition = minTransform.position;
        }
        // Diðer istasyonlar, sabit aralýklarla daha yukarýda oluþturulur
        else
        {
            stationPosition = maxTransform.position;
        }


        if (stationPosition.y <= prevStationPosition.y)
            return;
        // Ýstasyon prefabýný yarat ve listeye ekle
        GameObject newStation = Instantiate(stationPrefab, stationPosition, Quaternion.identity);
        stations.Add(newStation);

        prevStationPosition = stationPosition;
    }

    // Roketin bir platforma iniþ yapýp yapmadýðýný kontrol eder
    void CheckForLanding()
    {
        foreach (GameObject station in stations)
        {
            // Roketin platforma olan mesafesini kontrol et
            float distanceToStation = Vector2.Distance(rocket.transform.position, station.transform.position);

            // Eðer roket bir platforma yeterince yakýnsa ve "S" tuþuna basýlýrsa iniþ yap
            if (distanceToStation < landingDistance && Input.GetKey(KeyCode.S))
            {
                // Roketin "GameJam" script'ine ulaþ ve yakýtý doldur
                GameJam rocketScript = rocket.GetComponent<GameJam>();
                if (rocketScript != null)
                {
                    // Yakýtý doldur
                    rocketScript.fuel = fuelRefillAmount;
                    Debug.Log("Platforma iniþ yaptýnýz, yakýt doldu!");
                }
                else
                {
                    Debug.LogError("Roket üzerinde 'GameJam' script'i bulunamadý!");
                }
            }
        }
    }
}
