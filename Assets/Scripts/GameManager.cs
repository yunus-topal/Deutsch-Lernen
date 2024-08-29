using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
    [SerializeField] private GameObject levelParent;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button derButton;
    [SerializeField] private Button dieButton;
    [SerializeField] private Button dasButton;
    [SerializeField] private TextMeshProUGUI wordText;
    [SerializeField] private TextMeshProUGUI englishText;
    
    [SerializeField] private List<string> wordFiles = new List<string>();

    
    private List<int> _usedIndexes = new List<int>();
    private string _currentAnswer = "";
    
    private List<List<string>> _words = new List<List<string>>();
    private void Start() {
        _usedIndexes.Add(0);
        // load words from resources
        foreach (var wordFile in wordFiles) {
            TextAsset textAsset = Resources.Load<TextAsset>(wordFile);
            if (textAsset != null)
            {
                string[] words = textAsset.text.Split('\n');
                List<string> wordList = new List<string>(words);
                _words.Add(wordList);
                
            }
        }
        
        NextQuestion();
    }

    public void PickDer() {
        if (_currentAnswer.Equals("der")) {
            nextButton.gameObject.SetActive(true);
            derButton.GetComponent<Image>().color = Color.green;
            
            dasButton.interactable = false;
            dieButton.interactable = false;
        }
        else {
            derButton.GetComponent<Image>().color = Color.red;
        }
        derButton.interactable = false;
    }

    public void PickDie() {
        if (_currentAnswer.Equals("die")) {
            nextButton.gameObject.SetActive(true);
            dieButton.GetComponent<Image>().color = Color.green;
            
            derButton.interactable = false;
            dasButton.interactable = false;
        }
        else {
            dieButton.GetComponent<Image>().color = Color.red;
        }
        dieButton.interactable = false;
    }
    
    public void PickDas() {
        if (_currentAnswer.Equals("das")) {
            nextButton.gameObject.SetActive(true);
            dasButton.GetComponent<Image>().color = Color.green;
            
            derButton.interactable = false;
            dieButton.interactable = false;
        }
        else {
            dasButton.GetComponent<Image>().color = Color.red;
        }
        dasButton.interactable = false;
    }

    public void NextQuestion() {
        if(_usedIndexes.Count == 0) {
            wordText.text = "Select at least 1 level!";
            wordText.color = Color.red;
            return;
        }

        wordText.color = Color.white;
        
        nextButton.gameObject.SetActive(false);
        derButton.GetComponent<Image>().color = Color.white;
        derButton.interactable = true;
        dieButton.GetComponent<Image>().color = Color.white;
        dieButton.interactable = true;
        dasButton.GetComponent<Image>().color = Color.white;
        dasButton.interactable = true;
        
        ProcessNextWord();
    }

    private void ProcessNextWord() {
        // fetch current line
        int level = _usedIndexes[Random.Range(0, _usedIndexes.Count)];
        int index = Random.Range(0, _words[level].Count);
        
        string line = _words[level][index];
        string german = line.Split('|')[0];
        
        // handle german part
        _currentAnswer = german.Split(' ')[0][..3];
        string germanWord = german.Split(' ')[1];
        int slashIndex = germanWord.IndexOf('/');
        if(slashIndex != -1) {
            germanWord = germanWord[..slashIndex];
        }
        
        // handle english part
        string englishWord = line.Split('|')[1];
        if(englishWord.StartsWith(" the ")) {
            englishWord = englishWord[5..];
        }
        slashIndex = englishWord.IndexOf('/');
        if(slashIndex != -1) {
            englishWord = englishWord[..slashIndex];
        }

        wordText.text = germanWord;
        englishText.text = englishWord;
    }
    
    public void LevelButtonClicked() {
        if(levelParent.activeSelf) {
            levelParent.SetActive(false);
        }
        else {
            levelParent.SetActive(true);
        }
    }

    public void LevelToggleClicked(int index) {
        if (_usedIndexes.Contains(index)) {
            _usedIndexes.Remove(index);
        }else {
            _usedIndexes.Add(index);
        }
    }
}
