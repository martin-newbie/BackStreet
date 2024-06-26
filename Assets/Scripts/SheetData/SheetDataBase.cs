using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.CompilerServices;
using System;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(SheetDataBase), true)]
public class DataBaseLoadButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SheetDataBase manager = target as SheetDataBase;
        if (GUILayout.Button("Load from DataSheet"))
        {
            manager.LoadData();
        }
    }
}
#endif

public abstract class SheetDataBase : ScriptableObject
{
    protected abstract string gid { get; }
    protected abstract string range { get; }

    public async void LoadData()
    {
        string URL = GetURL();

        using (UnityWebRequest www = UnityWebRequest.Get(URL))
        {
            await www.SendWebRequest();
            SetData(www.downloadHandler.text);
        }
    }

    public IEnumerator LoadDataCor(Action onEnd = null)
    {
        string URL = GetURL();

        using (UnityWebRequest www = UnityWebRequest.Get(URL))
        {
            yield return www.SendWebRequest();
            SetData(www.downloadHandler.text);

            onEnd?.Invoke();
        }
    }

    string GetURL()
    {
        return $"https://docs.google.com/spreadsheets/d/1UIEkBEfvB_t4u0nk8ayJMDDkDoHWeRubl6R8F1C25g8/export?format=tsv&range={range}&gid={gid}";
    }

    abstract protected void SetData(string data);
}

public class UnityWebRequestAwaiter : INotifyCompletion
{
    private UnityWebRequestAsyncOperation asyncOp;
    private Action continuation;

    public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation asyncOp)
    {
        this.asyncOp = asyncOp;
        asyncOp.completed += OnRequestCompleted;
    }

    public bool IsCompleted { get { return asyncOp.isDone; } }

    public void GetResult() { }

    public void OnCompleted(Action continuation)
    {
        this.continuation = continuation;
    }

    private void OnRequestCompleted(AsyncOperation obj)
    {
        continuation();
    }
}

public static class ExtensionMethods
{
    public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp)
    {
        return new UnityWebRequestAwaiter(asyncOp);
    }
}