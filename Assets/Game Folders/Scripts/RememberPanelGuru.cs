using UnityEngine;

public class RememberPanelGuru : MonoBehaviour
{
    [SerializeField] private TugasRemember[] allTugas;

    [SerializeField] private ItemLembarJawabanRemember lembarJawab;
    [SerializeField] private Transform content;

    private void OnEnable()
    {
        FirebaseManager.Instance.GetAllRememberTugas(OnGetTugas);
    }

    private void OnDisable()
    {
        allTugas = null;
        foreach (Transform t in content)
        {
            Destroy(t.gameObject);
        }
    }

    private void OnGetTugas(AllRememberTugas tugas)
    {
        allTugas = tugas.tugas.ToArray();

        for (int i = 0; i < allTugas.Length; i++)
        {
            ItemLembarJawabanRemember j = Instantiate(lembarJawab, content);
            j.Initiate(allTugas[i]);
        }
    }
}
