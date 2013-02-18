using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextManager : MonoBehaviour {
	public GameObject gameLetterPrefab;
	public GameObject introLetterPrefab;
	public static TextManager me;
	private List<Text> activeTexts = new List<Text>();
	private static Dictionary<string, ManagerFont> fonts = new Dictionary<string, ManagerFont>();
	
	protected void Awake()
	{
		me = this;
		if (!fonts.ContainsKey("Intro"))
			fonts.Add("Intro", new IntroFont(introLetterPrefab));
		if (!fonts.ContainsKey("Game"))
			fonts.Add("Game", new GameFont(gameLetterPrefab));
	}
	
	// Update is called once per frame
	void Update () {
		UpdateTimers();
		UpdateColors();
	}
	
	private void UpdateTimers() {
		if (activeTexts.Count > 0)
		{
			List<Text> tmp = new List<Text>();
			tmp.AddRange(activeTexts);
			foreach (Text t in tmp)
			{
				t.time -= Time.deltaTime;
				if (t.time < 0)
				{
					t.DestroyPhrase();
					activeTexts.Remove(t);
					Destroy(t.folder);
				}
			} 
		}
	}
	
	private void UpdateColors() {
		foreach (Text t in activeTexts)
		{
			if ( ( ((int)(Time.realtimeSinceStartup * 100)) % 2 ) == 1)
			{
				t.ColorLetters();
			}
		}
	}
	
	public void RemoveText(GameObject objectToDelete) {
		if (activeTexts.Count > 0)
		{
			List<Text> tmp = new List<Text>();
			tmp.AddRange(activeTexts);
			foreach (Text t in tmp)
			{
				if (t.folder == objectToDelete)
				{
					t.DestroyPhrase();
					activeTexts.Remove(t);
					Destroy(t.folder);
				}
			} 
		}
	}
	
	public GameObject DrawText(string phrase, Vector2 position, float time, float size, string font)
	{
		activeTexts.Add(new Text(phrase, position, time, size, GetFont(font)));
		return activeTexts[activeTexts.Count-1].folder;
	}
	
	private ManagerFont GetFont(string font)
	{
		foreach (KeyValuePair<string, ManagerFont> k in fonts)
		{
			if (k.Key == font)
			{
				return k.Value;	
			}
		}
		return null;
	}
	
	public class Text {
		public float time = 0;
		public GameObject folder;
		public List<Letter> letterList = new List<Letter>();
		
		public Text(string phrase, Vector2 position, float duration, float size, ManagerFont font)
		{
			folder = new GameObject(phrase);
			folder.transform.position = new Vector3(position.x, 1, position.y);
			time = duration;
			DrawPhrase(phrase, position, size, font);
		}
		
		private void DrawPhrase(string phrase, Vector2 position, float size, ManagerFont font) {
			
			char[] charArray = phrase.ToCharArray();
			int offset = 0;
			int tmpColor = (int) font.GetColor();
			for (int i=0; i<charArray.Length; i++)
			{
				offset = font.ConvertLetter(System.Convert.ToInt32(charArray[i]));
				
				int color = tmpColor + i % 8;
				if (color > 7) { color -= 8; }
				float x = position.x + i * size * font.hspacing;
				SetSingleLetter(offset, color, new Vector2(x,position.y), size, folder, font);
			}
		}
	
		private void SetSingleLetter(int letter, int color, Vector2 position, float size, GameObject folder, ManagerFont font) {
			Letter tmp = new Letter(letter, color, position, size, folder, font);
			letterList.Add( tmp );
		}
		
		
		public void DestroyPhrase() {
			foreach (Letter l in letterList)
			{
				l.Remove();
			}
		}
		
		public void ColorLetters()
		{
			foreach (Letter l in letterList)
			{
				l.ChangeColor();
			}
		}
		
		public class Letter
		{
			private int _letter;
			private int _color = 0;
			public GameObject obj;
			private ManagerFont _font;
			
			public Letter(int letter, int color, Vector2 position, float size, GameObject folder, ManagerFont font){
				_letter = letter;
				_color = color;
				_font = font;
				Create(letter, color, position, size, folder);
			}
			
			public void Create(int letter, int color, Vector2 position, float size, GameObject folder)
			{
				obj = (GameObject) Instantiate(_font.prefab, new Vector3(position.x, 1, position.y), Quaternion.identity);
				obj.transform.parent = folder.transform;
				obj.transform.localScale = new Vector3(size, 1, size);
				UVFunctions.SetUVByPixelSizes(new Vector2(_letter, color), _font.size, false, obj.GetComponent<MeshFilter>().mesh, obj.GetComponent<MeshRenderer>().sharedMaterial.mainTexture);
			}
			
			public void ChangeColor()
			{
				_color++;
				if (_color > 7)
				{
					_color -= 8;
				}
				int tmpColor = _color;
				if (DrawableObject.colorblindMode)
				{
					tmpColor += 8;
				}
				UVFunctions.SetUVByPixelSizes(new Vector2(_letter, tmpColor), _font.size, false, obj.GetComponent<MeshFilter>().mesh, obj.GetComponent<MeshRenderer>().sharedMaterial.mainTexture);
			}
			
			public void Remove()
			{
				MonoBehaviour.Destroy(obj);	
			}
		}
	}
	
	public abstract class ManagerFont
		{
			public Vector2 size;
			public float hspacing;
			public GameObject prefab;
			
			public abstract int ConvertLetter(int letterToConvert);
		
			public abstract int GetColor();
		}
	
	public class IntroFont : ManagerFont {
			static int color = 0;
		
			public IntroFont(GameObject letterPrefab)
			{
				color = DrawableObject.getRandomColor();
				size = new Vector2(42, 60);
				hspacing = 0.6f;
				prefab = letterPrefab;
			}
			
		public override int ConvertLetter(int letterToConvert)
		{
			int tmp = 0;
			switch (letterToConvert)  {
			case 46:
				tmp = 11;
				break;
			case 65:
				tmp = 0;
				break;
			case 67:
				tmp = 1;
				break;
			case 97:
				tmp = 2;
				break;
			case 100:
				tmp = 3;
				break;
			case 101:
				tmp = 4;
				break;
			case 103:
				tmp = 5;
				break;
			case 105:
				tmp = 6;
				break;
			case 109:
				tmp = 7;
				break;
			case 111:
				tmp = 8;
				break;
			case 115:
				tmp = 9;
				break;
			case 118:
				tmp = 10;
				break;
			default:
				throw new System.Exception("Invalid Char '" + System.Convert.ToChar(letterToConvert).ToString() + "' (" + letterToConvert.ToString() + ")");	
			}
			return tmp;
		}
		
		public override int GetColor() {
			color += 2;
			if (color > 7) { color -= 8; }
			return color;
		}
	}
	
	public class GameFont : ManagerFont {
			
		public GameFont(GameObject letterPrefab)
			{
				size = new Vector2(11, 24);
				hspacing = 0.9f;
				prefab = letterPrefab;
			}
		
		public override int ConvertLetter(int letterToConvert)
		{
			int tmp = 0;
			if (letterToConvert == 32)					//	space
			{
				tmp = 3;
			}
			else if (letterToConvert == 33)					//	!
			{
				tmp = 0;
			}
			else if (letterToConvert == 35)				//	#
			{
				tmp = 1;
			}
			else if (letterToConvert == 63)				//	?
			{
				tmp = 2;
			}
			else if ((letterToConvert >= 48 && letterToConvert <= 57))	//	0-numbers-9	
			{
				tmp = letterToConvert - 44;
			}
			else if ((letterToConvert >= 65 && letterToConvert <= 90))	//	A-uppercase-Z
			{
				tmp = letterToConvert - 51;
			}
			else if (letterToConvert >= 97 && letterToConvert <= 122)	//	a-lowercase-z
			{
				tmp = letterToConvert - 57;	
			}
			else
			{
				throw new System.Exception("Invalid Char '" + System.Convert.ToChar(letterToConvert).ToString() + "' (" + letterToConvert.ToString() + ")");	
			}
			return tmp;
		}
		
		public override int GetColor() {
			return DrawableObject.getRandomColor();
		}
	}
}

