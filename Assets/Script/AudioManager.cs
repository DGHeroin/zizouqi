using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    static AudioManager _Instance;
    public static AudioManager Instance {
        get {
            if (_Instance != null) { return _Instance; }
            var obj = new GameObject("AudioManager");
            DontDestroyOnLoad(obj);
            _Instance = obj.AddComponent<AudioManager>();
            return _Instance;
        }
    }
    public void PlaySFX(AudioClip clip, float delay = 0) {
        var obj = new GameObject();
        obj.transform.SetParent(this.transform);
        var source = obj.AddComponent<AudioSource>();
        source.clip = clip;
        if (delay > 0) {
            source.PlayDelayed(delay);
        } else {
            source.Play();
        }
        
        Destroy(obj, delay + clip.length);
    }
}
