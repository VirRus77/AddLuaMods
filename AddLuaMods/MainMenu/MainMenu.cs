using System;
using AddLuaMods.Tools;
using UnityEngine;

namespace AddLuaMods.MainMenu
{
    public class MainMenu : BoxClass<global::MainMenu>
    {
        public MainMenu(global::MainMenu instance)
            : base(instance)
        {
        }

        public void Start()
        {
            Logging.LogDebug("MainMenu.Start");
            InvokeMethod<global::BaseMenu>(nameof(Start));
            Logging.LogDebug("base.Start");
            string[] strArray = new string[10]
            {
                "StartButton",
                "LoadButton",
                "ContinueButton",
                "SettingsButton",
                "DiscordButton",
                "WikiButton",
                "BadgesButton",
                "QuitButton",
                "LanguageButton",
                "ModsButton"
            };
            Action<BaseGadget>[] actionArray = new Action<BaseGadget>[10]
            {
                OnNewGame,
                OnLoad,
                OnContinue,
                OnSettings,
                OnDiscord,
                OnWiki,
                OnBadges,
                OnQuit,
                OnLanguageSelect,
                OnModsSelect
            };
            Logging.LogDebug("base.Start for");
            for (var index = 0; index < strArray.Length; ++index)
            {
                Logging.LogDebug($"base.Start for {index}");
                BaseButton component = transform.Find(strArray[index]).GetComponent<BaseButton>();
                
                Logging.LogDebug($"base.Start for component");
                if (strArray[index] == "ContinueButton")
                {
                    component.SetActive(false);
                    m_ContinueButton = component;
                    UpdateContinueButton();
                }

                Logging.LogDebug($"base.Start for WikiButton");
                //if (strArray[index] == "ModsButton" || strArray[index] == "WikiButton")
                //    component.SetActive(false);
                if (strArray[index] == "WikiButton")
                    component.SetActive(false);

                Logging.LogDebug($"base.Start for QuitButton");
                if (strArray[index] == "QuitButton")
                    component.SetRolloverFromID("MainMenuQuitSurvival");
                AddAction(component, actionArray[index]);
            }

            Logging.LogDebug($"base.Start Version");

            transform.Find("Version").GetComponent<BaseText>().SetText(SaveLoadManager.GetVersion());
            Logging.LogDebug($"base.Start Experimental");
            m_Experimental = transform.Find("Experimental").GetComponent<BaseText>();
            m_Experimental.SetActive(false);
            Logging.LogDebug($"base.Start SubTitlePanel/SubTitle");
            transform.Find("SubTitlePanel/SubTitle").GetComponent<BaseText>().SetTextFromID("MainMenuSubTitle4Survival", true);
            AudioManager.Instance.StartMusic("MusicCover");
        }

        public Transform transform
        {
            get => _instance.transform; //_traverse.Property(nameof(transform)).GetValue<Transform>();
        }

        public BaseButton m_ContinueButton
        {
            get => _traverse.Field<BaseButton>(nameof(m_ContinueButton)).Value;
            set => _traverse.Field<BaseButton>(nameof(m_ContinueButton)).Value = value;
        }

        public BaseText m_Experimental
        {
            get => GetField<global::BaseText>(nameof(m_Experimental));
            set => SetField<global::BaseText>(nameof(m_Experimental), value);
        }

        public void AddAction(BaseGadget component, Action<BaseGadget> action)
        {
            //InvokeMethod(nameof(AddAction), component, action);
            _instance.AddAction(component, action);
        }

        public void OnNewGame(BaseGadget NewGadget)
        {
            //InvokeMethod(nameof(OnNewGame), NewGadget);
            _instance.OnNewGame(NewGadget);
        }

        public void OnLoad(BaseGadget NewGadget)
        {
            //InvokeMethod(nameof(OnLoad), NewGadget);
            _instance.OnLoad(NewGadget);
        }

        public void OnContinue(BaseGadget NewGadget)
        {
            //InvokeMethod(nameof(OnContinue), NewGadget);
            _instance.OnContinue(NewGadget);
        }

        public void OnSettings(BaseGadget NewGadget)
        {
            //InvokeMethod(nameof(OnSettings), NewGadget);
            _instance.OnSettings(NewGadget);
        }

        public void OnDiscord(BaseGadget NewGadget)
        {
            //InvokeMethod(nameof(OnDiscord), NewGadget);
            _instance.OnDiscord(NewGadget);
        }

        public void OnWiki(BaseGadget NewGadget)
        {
            //InvokeMethod(nameof(OnWiki), NewGadget);
            _instance.OnWiki(NewGadget);
        }

        public void OnBadges(BaseGadget NewGadget)
        {
            //InvokeMethod(nameof(OnBadges), NewGadget);
            _instance.OnBadges(NewGadget);
        }

        public void OnQuit(BaseGadget NewGadget)
        {
            //InvokeMethod(nameof(OnQuit), NewGadget);
            _instance.OnQuit(NewGadget);
        }

        public void OnLanguageSelect(BaseGadget NewGadget)
        {
            //InvokeMethod(nameof(OnLanguageSelect), NewGadget);
            _instance.OnLanguageSelect(NewGadget);
        }

        public void OnModsSelect(BaseGadget NewGadget)
        {
            //InvokeMethod(nameof(OnModsSelect), NewGadget);
            _instance.OnModsSelect(NewGadget);
        }

        public void UpdateContinueButton()
        {
            InvokeMethod(nameof(UpdateContinueButton));
            //_instanceType.GetMethodInfo(nameof(UpdateContinueButton))
            //    .Invoke(_instance, new object[0]);
        }
    }
}
