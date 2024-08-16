using UnityEngine;

namespace DefaultNamespace.Vehicles
{
    public class TrafficLight : MonoBehaviour
    {
        [SerializeField] private Sprite gray;
        [SerializeField] private Sprite red;
        [SerializeField] private Sprite yellow;
        [SerializeField] private Sprite green;

        [SerializeField] private SpriteRenderer[] rights;

        public void SetLevel(int n)
        {
            switch (n)
            {
                case 0:
                    foreach (var right in rights)
                    {
                        right.enabled = false;
                    }
                    break;
                case 1:
                    foreach (var right in rights)
                    {
                        right.enabled = true;
                        right.sprite = gray;
                    }

                    rights[0].sprite = red;
                    break;
                case 2:
                    rights[0].sprite = gray;
                    rights[1].sprite = yellow;
                    break;
                case 3:
                    rights[1].sprite = gray;
                    rights[2].sprite = green;
                    break;
                case 4:
                    foreach (var right in rights)
                    {
                        right.enabled = false;
                    }
                    break;
            }
        }
    }
}