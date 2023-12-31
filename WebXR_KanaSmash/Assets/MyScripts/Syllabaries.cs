﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SyllabariesData", menuName = "Japanese/Syllabaries")]
public class Syllabaries : ScriptableObject
{
    

    public List<string> romajiList = new List<string> {
    "a", "i", "u", "e", "o",
    "ka", "ki", "ku", "ke", "ko",
    "sa", "shi", "su", "se", "so",
    "ta", "chi", "tsu", "te", "to",
    "na", "ni", "nu", "ne", "no",
    "ha", "hi", "fu", "he", "ho",
    "ma", "mi", "mu", "me", "mo",
    "ya", "yu", "yo",
    "ra", "ri", "ru", "re", "ro",
    "wa", "wo", "n" };

    public List<string> katakanaList = new List<string> {
    "ア", "イ", "ウ", "エ", "オ",
    "カ", "キ", "ク", "ケ", "コ",
    "サ", "シ", "ス", "セ", "ソ",
    "タ", "チ", "ツ", "テ", "ト",
    "ナ", "ニ", "ヌ", "ネ", "ノ",
    "ハ", "ヒ", "フ", "ヘ", "ホ",
    "マ", "ミ", "ム", "メ", "モ",
    "ヤ","ユ","ヨ","ラ", "リ", 
     "ル", "レ", "ロ", "ワ", "ヲ", "ン"};

    public List<string> hiraganaList = new List<string> { 
    "あ", "い", "う", "え", "お",
    "か", "き", "く", "け", "こ",
    "さ", "し", "す", "せ", "そ",
    "た", "ち", "つ", "て", "と",
    "な", "に", "ぬ", "ね", "の",
    "は", "ひ", "ふ", "へ", "ほ",
    "ま", "み", "む", "め", "も",
    "や",      "ゆ",      "よ",
    "ら", "り", "る", "れ", "ろ",
    "わ", "を", "ん" };
}