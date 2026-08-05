using UnityEngine;
using UnityEngine.UI;

public class UIBoss : MonoBehaviour
{
    [SerializeField] private Boss boss;
    [SerializeField] private Image healthFill;
    [SerializeField] private SpriteRenderer crackSpriteRenderer;

    private void Start()
    {
        boss.OnAttacked += HandleBossAttacked;
    }

    private void HandleBossAttacked(object sender, BossAttackedEventArgs e)
    {
        healthFill.fillAmount = (float)e.CurrentHealth / e.MaxHealth;

        // Risse im Sprite sichtbar machen, je mehr Schaden der Boss nimmt
        if (crackSpriteRenderer != null)
        {
            Color tempColor = crackSpriteRenderer.color;
            tempColor.a = 1 - ((float)e.CurrentHealth / e.MaxHealth);
            crackSpriteRenderer.color = tempColor;
        }
    }
}