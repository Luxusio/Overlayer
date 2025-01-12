﻿using System;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.Pool;
using System.IO;
using Overlayer.Core;
using Overlayer.Core.Utils;
#pragma warning disable

namespace Overlayer
{
    public class ShadowText : MonoBehaviour
    {
        public static GameObject PCanvasObj;
        public static Canvas PublicCanvas;
        public static int TotalCount { get; internal set; }
        public static ShadowText NewText()
        {
            int count = ++TotalCount;
            ShadowText st = new GameObject($"ShadowText_{count}").AddComponent<ShadowText>();
            st.Number = count;
            return st;
        }
        public void Destroy(bool destoryCanvas = false)
        {
            UnityEngine.Object.Destroy(Main);
            UnityEngine.Object.Destroy(Shadow);
            if (destoryCanvas)
                UnityEngine.Object.Destroy(PCanvasObj);
        }
        public Action Updater;
        public TextMeshProUGUI Main;
        public TextMeshProUGUI Shadow;
        public string CurrentFont = "Default";
        public int Number;
        public TextAlignmentOptions Alignment
        {
            get => Main.alignment;
            set
            {
                Main.alignment = value;
                Shadow.alignment = value;
            }
        }
        public float FontSize
        {
            get => Main.fontSize;
            set
            {
                Main.fontSize = value;
                Shadow.fontSize = value;
                var xy = FontSize / 20f;
                Shadow.rectTransform.anchoredPosition = Position + new Vector2(xy, -xy);
            }
        }
        public Color Color
        {
            get => Main.color;
            set => Main.color = value;
        }
        public Vector2 Center
        {
            get => Main.rectTransform.anchorMin;
            set
            {
                Main.rectTransform.anchorMin = value;
                Main.rectTransform.anchorMax = value;
                Main.rectTransform.pivot = value;

                Shadow.rectTransform.anchorMin = value;
                Shadow.rectTransform.anchorMax = value;
                Shadow.rectTransform.pivot = value;
            }
        }
        public Vector2 Position
        {
            get => Main.rectTransform.anchoredPosition;
            set
            {
                Main.rectTransform.anchoredPosition = value;
                var xy = FontSize / 20f;
                Shadow.rectTransform.anchoredPosition = value + new Vector2(xy, -xy);
            }
        }
        public bool Initialized { get; private set; }
        public void Init(TextConfig config)
        {
            if (Initialized) return;
            if (!PublicCanvas)
            {
                GameObject pCanvasObj = PCanvasObj = new GameObject("Overlayer Canvas");
                PublicCanvas = pCanvasObj.AddComponent<Canvas>();
                PublicCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                CanvasScaler scaler = pCanvasObj.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                var currentRes = Screen.currentResolution;
                scaler.referenceResolution = new Vector2(currentRes.width, currentRes.height);
                DontDestroyOnLoad(PublicCanvas);
            }

            GameObject shadowObject = new GameObject();
            shadowObject.transform.SetParent(PublicCanvas.transform);
            shadowObject.MakeFlexible();
            Shadow = shadowObject.AddComponent<TextMeshProUGUI>();
            var font = FontManager.GetFont("Default");
            Shadow.font = font.fontTMP;
            Shadow.enableVertexGradient = true;
            Shadow.color = Color.white.WithAlpha(0.4f);
            Shadow.colorGradient = config.ShadowColor_G;

            GameObject mainObject = new GameObject();
            mainObject.transform.SetParent(PublicCanvas.transform);
            mainObject.MakeFlexible();
            Main = mainObject.AddComponent<TextMeshProUGUI>();
            Main.font = font.fontTMP;
            Main.enableVertexGradient = true;
            Main.color = Color.white;
            Main.colorGradient = config.TextColor_G;

            Main.enableAutoSizing = Shadow.enableAutoSizing = false;
            Main.lineSpacing = Shadow.lineSpacing = config.LineSpacing;
            Main.lineSpacingAdjustment = Shadow.lineSpacingAdjustment = config.LineSpacingAdjustment;

            Active = config.Active;
            Initialized = true;
        }
        public bool TrySetFont(string name)
        {
            if (CurrentFont == name) return true;
            if (FontManager.TryGetFont(name, out FontData font))
            {
                Main.font = font.fontTMP;
                Shadow.font = font.fontTMP;
                CurrentFont = name;
                return true;
            }
            return false;
        }
        private void Update() => Updater?.Invoke();
        public bool Active
        {
            get => Main.gameObject.activeSelf;
            set
            {
                Main.gameObject.SetActive(value);
                Shadow.gameObject.SetActive(value);
            }
        }
    }
}
