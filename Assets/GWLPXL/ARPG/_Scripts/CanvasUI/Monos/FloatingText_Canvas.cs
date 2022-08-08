using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GWLPXL.ARPGCore.CanvasUI.com
{
    /// <summary>
    /// reserve 0 - 100 for ARPG, use any of the rest
    /// </summary>
    public enum FloatingTextType
    {
        Damage = 0,
        Regen = 1,
        DoTs = 2
    }

    [System.Serializable]
    public class DamageTextOptions
    {
      
        public ElementUI[] ElementUI = new ElementUI[0];
    }
    [System.Serializable]
    public class RegenTextOptions
    {
        public ResourceUI[] ResourceUI = new ResourceUI[0];
    }
    [System.Serializable]
    public class DoTTextOptions
    {

        public ElementUI[] ElementUI = new ElementUI[0];
    }
    public class FloatingText_Canvas : MonoBehaviour, IFloatTextCanvas, ITick
    {
        [Tooltip("Allows this manager to control when the objects are destroyed using the damage text alive time.")]
        public bool ControlLifetime = true;
        [Tooltip("Allows this manager to control the movement. Disable if you want your prefabs to control their own movements, such as if you're using DoTween.")]
        public bool ControlMovement = true;
        public float TickRate = .02f;
        public bool UseDeltaTime;
        [Header("Damage Text")]
        [SerializeField]
        DamageTextOptions damageOptions = new DamageTextOptions();
        public Transform DamageTextPanel = default;
        [Header("Regen Text")]
        [SerializeField]
        RegenTextOptions regenOptions = new RegenTextOptions();
        public Transform RegenTextPanel = default;
        [Header("DoT Text")]
        [SerializeField]
        DoTTextOptions dotOptions = new DoTTextOptions();
        public Transform dotTextPanel = default;

        protected Dictionary<IReceiveDamage, QueueTimer> allQueuedDamage = new Dictionary<IReceiveDamage, QueueTimer>();
        protected Dictionary<IReceiveDamage, QueueTimer> allQueuedDots = new Dictionary<IReceiveDamage, QueueTimer>();
        protected Dictionary<IReceiveDamage, QueueTimer> allqueuedRegen = new Dictionary<IReceiveDamage, QueueTimer>();

        protected Queue<GameObject> pooledText = new Queue<GameObject>();
        protected List<GameObject> destroyable = new List<GameObject>();
        protected List<FloatingText> damageTextList = new List<FloatingText>();
        protected Vector3 moveDirection;
        protected Canvas canvas;
        protected Camera main;

        #region unity calls
        protected virtual void Awake()
        {
            main = Camera.main;
            canvas = GetComponent<Canvas>();
        }

        protected virtual void OnEnable()
        {
            DungeonMaster.Instance.SetFloatingTextScene(this);
        }
        protected virtual void OnDisable()
        {
            DungeonMaster.Instance.SetFloatingTextScene(null);

        }
        protected virtual void Start()
        {
            AddTicker();
        }

        protected virtual void OnDestroy()
        {
            RemoveTicker();
        }
        #endregion

        #region public interfaces
        public void AddTicker() => TickManager.Instance.AddTicker(this);


        public void DoTick()
        {
            DamageTextQueueing();
            DamageTextObjectLifetime();
        }

        public void RemoveTicker() => TickManager.Instance.RemoveTicker(this);


        public float GetTickDuration()
        {
            if (UseDeltaTime) return Time.deltaTime;
            return TickRate;
        }

        #endregion

        #region public virtual
        /// <summary>
        /// DoT text
        /// </summary>
        /// <param name="damageTaker"></param>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="type"></param>
        /// <param name="isCritical"></param>
        public virtual void CreateDoTText(IReceiveDamage damageTaker, string text, Vector3 position, ElementType type, bool isCritical = false)
        {
            DefaultDoTText(damageTaker, text, position, type);

        }
        /// <summary>
        /// user created floating text
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="atPosition"></param>
        /// <param name="text"></param>
        /// <returns></returns>

        public virtual void CreateNewFloatingText(IReceiveDamage damageTaker, ElementUI variables, Vector3 atPosition, string text, FloatingTextType type, bool isCritical)
        {
            DefaultCreatenew(damageTaker, variables, atPosition, text, type, isCritical);
        }

       
        /// <summary>
        /// reggen text
        /// </summary>
        /// <param name="damageTaker"></param>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="type"></param>
        /// <param name="isCritical"></param>
        public virtual void CreateRegenText(IReceiveDamage damageTaker, string text, Vector3 position, ResourceType type, bool isCritical = false)
        {
            DefaultRegenText(damageTaker, text, position, type);

        }

        /// <summary>
        /// damage text
        /// </summary>
        /// <param name="damageTaker"></param>
        /// <param name="position"></param>
        /// <param name="text"></param>
        /// <param name="type"></param>
        /// <param name="isCritical"></param>
        public virtual void CreateDamagedText(IReceiveDamage damageTaker, Vector3 position, string text, ElementType type, bool isCritical = false)
        {
            DefaultDamageText(damageTaker, position, text, type, isCritical);

        }

        #endregion

        #region protected virtual 
        protected virtual FloatingText DefaultCreatenew(IReceiveDamage damageTaker, ElementUI variables, Vector3 atPosition, string text, FloatingTextType type, bool isCritical)
        {
            FloatingText ftext = GetUI(variables, atPosition, text);
            switch (type)
            {
                case FloatingTextType.Damage:
                    AddToQueue(allQueuedDamage, damageTaker, ftext);
                    break;
                case FloatingTextType.DoTs:
                    AddToQueue(allQueuedDots, damageTaker, ftext);
                    break;
                case FloatingTextType.Regen:
                    AddToQueue(allqueuedRegen, damageTaker, ftext);
                    break;
            }
            return ftext;
            
        }

        protected virtual void DefaultDoTText(IReceiveDamage damageTaker, string text, Vector3 position, ElementType type)
        {
            for (int i = 0; i < dotOptions.ElementUI.Length; i++)
            {
                if (type == dotOptions.ElementUI[i].Type)
                {
                    ElementUI elementUI = dotOptions.ElementUI[i];
                    FloatingText ftext = GetUI(elementUI, position, text);
                    AddToQueue(allQueuedDots, damageTaker, ftext);
                }
            }
        }
        protected virtual void DefaultRegenText(IReceiveDamage damageTaker, string text, Vector3 position, ResourceType type)
        {
            //a queue system

            for (int i = 0; i < regenOptions.ResourceUI.Length; i++)
            {
                if (type == regenOptions.ResourceUI[i].Type)
                {
                    ResourceUI resourceUI = regenOptions.ResourceUI[i];
                    FloatingText ftext = GetUI(resourceUI, position, text);
                    AddToQueue(allqueuedRegen, damageTaker, ftext);
                    break;
                }
            }
        }

        protected virtual void DefaultDamageText(IReceiveDamage damageTaker, Vector3 position, string text, ElementType type, bool isCritical)
        {
            for (int i = 0; i < damageOptions.ElementUI.Length; i++)
            {
                if (type == damageOptions.ElementUI[i].Type)
                {

                    ElementUI elementUI = damageOptions.ElementUI[i];
                    if (isCritical)
                    {
                        text += " Crit!";
                    }
                    FloatingText ftext = GetUI(elementUI, position, text);
                    
                    AddToQueue(allQueuedDamage, damageTaker, ftext);
                    break;
                }
            }

        }

        protected virtual void DamageTextQueueing()
        {
            if (ControlLifetime == false) return;


            ControlLifetimeObjs(allQueuedDamage);
            ControlLifetimeObjs(allQueuedDots);
            ControlLifetimeObjs(allqueuedRegen);
            //}
        }

        protected virtual void ControlLifetimeObjs(Dictionary<IReceiveDamage, QueueTimer> queued)
        {

            if (queued.Count == 0) return;
            foreach (var kvp in queued)
            {
                QueueTimer timer = kvp.Value;
                if (timer.QueuedObjs.Count == 0) continue;

                timer.RunningTimer += GetTickDuration();

                if (timer.RunningTimer >= 0)//timer.QueueSpacing)//queue spacing not workign as intended, removing until fixed.
                {
                    FloatingText textObj = timer.QueuedObjs.Dequeue();
                    textObj.TextObj.SetActive(true);
                    timer.RunningTimer = 0;
                    continue;
                }
            }
        }


       /// <summary>
       /// track the floating text object lifetime
       /// </summary>
        protected virtual void DamageTextObjectLifetime()
        {

            if (ControlLifetime == false) return;


            for (int i = 0; i < damageTextList.Count; i++)
            {
                FloatingText text = damageTextList[i];
                if (text.TextObj.activeSelf == false) continue;//if it's inactive, don't count it towards alive time

                if (text.RunningTimer >= text.DamageTextAliveTime)//if we have been around longer than our alive time, destroy us and go to the next
                {
                    damageTextList.RemoveAt(i);
                    text.RunningTimer = 0;
                    destroyable.Add(text.TextObj);
                    continue;
                }

                damageTextList[i].RunningTimer += GetTickDuration();//if not, add time


                MoveFloatText(text);



            }



            for (int i = 0; i < destroyable.Count; i++)
            {
                destroyable[i].SetActive(false);
                pooledText.Enqueue(destroyable[i]);

                //Destroy(destroyable[i]);
            }
            destroyable.Clear();
        }

        /// <summary>
        /// control the movement of the floating text
        /// </summary>
        /// <param name="text"></param>
        protected virtual void MoveFloatText(FloatingText text)
        {
            if (ControlMovement == false) return;

            float targetX = text.MoveX;
            float targetY = text.MoveY;
            float moveYMulti = text.MoveYMulti;
            float moveXMulti = text.MoveXMulti;
            float percent = text.RunningTimer / text.DamageTextAliveTime;
            if (text.YCurve != null)
            {
                moveYMulti = text.YCurve.Evaluate(percent);
            }

            if (text.FadeCurve != null)
            {
                float lerpA = text.FadeCurve.Evaluate(percent);
                //float lerpA = Mathf.Lerp(1, 0, percent);
                text.Color.a = lerpA;
                text.TextObj.GetComponent<IFloatingText>().SetColor(text.Color);
            }

            moveDirection.x = targetX * moveXMulti;
            moveDirection.y = targetY * moveYMulti;


            Vector3 newPos = text.TextObj.transform.position + (moveDirection * text.SpeedMultiplier);// * GetTickDuration();// * damageOptions.DamageTextSpeed * Time.deltaTime;//get a new position to move to based on speed

            text.TextObj.transform.position = newPos;//udpate oru movement

            if (text.SizeCurve != null)
            {
                text.TextObj.transform.localScale =
                    new Vector3(1, 1, 1) + (new Vector3(1, 1, 1) * text.SizeCurve.Evaluate(percent) * text.ScaleMulti);
            }
        }

      

        /// <summary>
        /// gets from pool or creates new floattext
        /// </summary>
        /// <param name="elementUI"></param>
        /// <param name="atPosition"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        protected virtual FloatingText GetUI(ElementUI elementUI, Vector3 atPosition, string text)
        {
            int randomYmulti = UnityEngine.Random.Range(-5, 5);
            int randomXmulti = UnityEngine.Random.Range(-5, 5);
            GameObject newObj = null;

            if (pooledText.Count > 0)
            {
                newObj = pooledText.Dequeue();
              
            }
            else
            {
                newObj = Instantiate(elementUI.TextObjectPrefab, DamageTextPanel);
                newObj.SetActive(false);

            }
            IFloatingText textu = newObj.GetComponent<IFloatingText>();
    
           
            textu.SetFont(elementUI.Font);
            Color color = elementUI.Color;
            textu.SetColor(color);
            float X = elementUI.TargetDir.x;
            float Y = elementUI.TargetDir.y;
            AnimationCurve curve = elementUI.YCurve;
            float queueSpacing = elementUI.DamageQueueSpacing;
            float aliveTime = elementUI.DamageTextAliveTime;
            AnimationCurve sizeCurve = elementUI.SizeCurve;
            AnimationCurve fadeCurve = elementUI.FadeCurve;
            if (string.IsNullOrEmpty(elementUI.ZeroReplacement) == false)
            {
                int zerocheck = int.Parse(text);
                if (zerocheck == 0)
                {
                    text = elementUI.ZeroReplacement;
                }
            }
            textu.SetText(text);

            
            newObj.transform.position = main.WorldToScreenPoint(atPosition);

            FloatingText _damageText = new FloatingText(newObj, Y, X, randomYmulti, randomXmulti, curve, aliveTime, queueSpacing);//can change the 5 value to influence the angle of the numbers, higher number creates a sharper angle.
            _damageText.SizeCurve = sizeCurve;
            _damageText.Color = color;
            _damageText.FadeCurve = fadeCurve;
            _damageText.ScaleMulti = elementUI.ScaleMulti;
            _damageText.SpeedMultiplier = elementUI.SpeedMultiplier;
            damageTextList.Add(_damageText);
            return _damageText;
        }

        protected virtual FloatingText GetUI(ResourceUI elementUI, Vector3 atPosition, string text)
        {
            float randomYmulti = UnityEngine.Random.Range(1f, 5f);
            float randomXmulti = UnityEngine.Random.Range(1f, 5f);

            GameObject newObj = null;

            if (pooledText.Count > 0)
            {
                newObj = pooledText.Dequeue();

            }
            else
            {
                newObj = Instantiate(elementUI.TextObjectPrefab, DamageTextPanel);

            }
            newObj.SetActive(false);
            IFloatingText textu = newObj.GetComponent<IFloatingText>();
            textu.SetFont(elementUI.Font);
            Color color = new Color();
            color = elementUI.Color;
            textu.SetColor(color);
            int randomX = elementUI.StartOffset;
            int Y = elementUI.TravelHeight;
            AnimationCurve curve = elementUI.YCurve;
            float queueSpacing = elementUI.DamageQueueSpacing;
            float aliveTime = elementUI.DamageTextAliveTime;
            AnimationCurve sizeCurve = elementUI.SizeCurve;
            AnimationCurve fadeCurve = elementUI.FadeCurve;
            if (string.IsNullOrEmpty(elementUI.ZeroReplacement) == false)
            {
                int zerocheck = int.Parse(text);
                if (zerocheck == 0)
                {
                    text = elementUI.ZeroReplacement;
                }
            }
            textu.SetText(text);
            newObj.transform.position = main.WorldToScreenPoint(atPosition);

            FloatingText _damageText = new FloatingText(newObj, Y, randomX, randomYmulti, randomXmulti, curve, aliveTime, queueSpacing);//can change the 5 value to influence the angle of the numbers, higher number creates a sharper angle.
            _damageText.SizeCurve = sizeCurve;
            _damageText.Color = color;
            _damageText.FadeCurve = fadeCurve;
            damageTextList.Add(_damageText);
            return _damageText;
        }

      

        protected virtual void AddToQueue(Dictionary<IReceiveDamage, QueueTimer> queue, IReceiveDamage damageTaker, FloatingText _damageText)
        {

            if (ControlLifetime)
            {
                queue.TryGetValue(damageTaker, out QueueTimer value);
                if (value == null)
                {
                    QueueTimer timer = new QueueTimer(damageTaker, _damageText.QueueSpacing);
                    value = timer;
                }

                if (value.QueuedObjs.Count > 0)
                {
                    //do the opposite...
                    FloatingText _previous = value.QueuedObjs.Dequeue();
                    _damageText.MoveXMulti = _previous.MoveXMulti * -1f;
                    _damageText.MoveYMulti = _previous.MoveYMulti * -1f;
                    _damageText.MoveY = _previous.MoveY * 1.5f;
                    _damageText.ScaleMulti = _previous.ScaleMulti *= .5f;
                    _damageText.QueueSpacing += _previous.QueueSpacing;
                    value.QueuedObjs.Enqueue(_previous);
                }
                else
                {
                    _damageText.TextObj.SetActive(true);//first one
                }


                value.QueuedObjs.Enqueue(_damageText);
                allQueuedDamage[damageTaker] = value;


            }
            else
            {
                _damageText.TextObj.SetActive(true);
            }
        }

        #endregion



    }

    [System.Serializable]
    public class ResourceUI
    {
        public GameObject TextObjectPrefab = default;
        public ResourceType Type;
        public TMP_FontAsset Font;
        public Color Color;
        public int StartOffset;
        public int TravelHeight;
        [Range(-4, 4)]
        public int StartPos;
        public float DamageTextSpeed = 1f;
        public float DamageTextAliveTime = 1f;
        public float DamageQueueSpacing = 1f;
        public AnimationCurve YCurve;
        public AnimationCurve SizeCurve;
        public AnimationCurve FadeCurve;
        public float AliveTime;
        public string ZeroReplacement = string.Empty;
        public int MaxQueued = 3;
    }

    //used to set element UI values for elemental damage
    [System.Serializable]
    public class ElementUI
    {
        public GameObject TextObjectPrefab = default;
        public ElementType Type;
        public TMP_FontAsset Font;
        public Color Color;
        public Vector3 TargetDir;
        public AnimationCurve YCurve;
        public AnimationCurve SizeCurve;
        public AnimationCurve FadeCurve;
        public float SpeedMultiplier = 1f;
        public float DamageTextAliveTime = 1f;
        public float DamageQueueSpacing = 1f;
        public string ZeroReplacement = string.Empty;
        public int MaxQueued = 3;
        public float ScaleMulti = 1;
    }

    [System.Serializable]
    public class QueueTimer
    {
        public IReceiveDamage DamageTaker;
        public float QueueSpacing;
        public float RunningTimer;
        public Queue<FloatingText> QueuedObjs;
        public QueueTimer(IReceiveDamage _damageTaker, float _queueSpacing)
        {
            DamageTaker = _damageTaker;
            QueueSpacing = _queueSpacing;
            QueuedObjs = new Queue<FloatingText>();
        }
    }

  
    [System.Serializable]
    public class FloatingText
    {
        public float SpeedMultiplier = 1f;

        public float QueueSpacing = .002f;
        public float DamageTextAliveTime = 1f;
        public GameObject TextObj;
        public float RunningTimer;
        public float MoveY;
        public float MoveX;
        public float MoveXMulti;
        public float MoveYMulti;
        public AnimationCurve YCurve;
        public AnimationCurve SizeCurve;
        public AnimationCurve FadeCurve;
        public Color Color;
        public float ScaleMulti;
        public FloatingText(GameObject obj, float moveY, float moveX, float moveYMulti, float moveXMulti, AnimationCurve curve, float aliveTime, float queueSpacing)
        {
            TextObj = obj;
            RunningTimer = 0;
            MoveY = moveY;
            MoveX = moveX;
            MoveXMulti = moveXMulti;
            MoveYMulti = moveYMulti;
            YCurve = curve;
            DamageTextAliveTime = aliveTime;
            QueueSpacing = queueSpacing;
        }
    }
}