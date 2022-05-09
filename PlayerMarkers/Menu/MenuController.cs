using PlayerMarkers.Util;
using System;
using System.Collections;
using PlayerMarkers.Asset;
using PlayerMarkers.Menu.Impl;
using TMPro;
using UnboundLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PlayerMarkers.Menu
{
    public static class MenuController
    {
        public static bool inGame;

        private static readonly Color OwnMarkerColor = new Color(0.747f, 0.061f, 0.022f, 0.58f);
        private static Transform _ownMarkerTransform;
        private static GameObject _ownMarker;
        public static bool ownMarkerEnabled;
        public static Vector3 ownMarkerSize;
        public static float ownMarkerBloom;
        public static int ownMarkerType;

        private static readonly Color TeamMarkerColor = new Color(0.047f, 0.061f, 0.722f, 0.58f);
        private static Transform _teamMarkerTransform;
        private static GameObject _teamMarker;
        public static bool teamMarkerEnabled;
        public static Vector3 teamMarkerSize;
        public static float teamMarkerBloom;
        public static int teamMarkerType;

        private static readonly Color EnemyMarkerColor = new Color(0.047f, 0.761f, 0.122f, 0.58f);
        private static Transform _enemyMarkerTransform;
        private static GameObject _enemyMarker;
        public static bool enemyMarkerEnabled;
        public static Vector3 enemyMarkerSize;
        public static float enemyMarkerBloom;
        public static int enemyMarkerType;

        static MenuController()
        {
            Unbound.RegisterMenu("Player Markers", () => { }, GeneralSettings.Menu, null, false);

            inGame = false;

            Unbound.Instance.StartCoroutine(MarkerUpdater());
        }

        internal static void Initialize()
        {

        }

        public static IEnumerator FindScaler(Transform transform, MarkerOwner markerOwner)
        {
            while (FindInActiveObjectByName("PlayerScaler_Small", "FaceSelector") == null)
            {
                yield return new WaitForSeconds(1);
            }

            var playerObject = FindInActiveObjectByName("PlayerScaler_Small", "FaceSelector");
            var activePlayerObject = Object.Instantiate(playerObject, transform);

            activePlayerObject.name = "TestObjectPlayer";
            activePlayerObject.transform.localPosition = new Vector3(0, 80, 0);
            activePlayerObject.transform.localScale = new Vector3(60, 60, 60);
            activePlayerObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
            var spriteRenderer = activePlayerObject.GetComponentInChildren<SpriteRenderer>();

            switch (markerOwner)
            {
                case MarkerOwner.Own:
                    {
                        spriteRenderer.color = OwnMarkerColor;
                        _ownMarkerTransform = activePlayerObject.transform;
                        
                        CreateMarker(markerOwner);
                        break;
                    }
                case MarkerOwner.Team:
                    {
                        spriteRenderer.color = TeamMarkerColor;
                        _teamMarkerTransform = activePlayerObject.transform;

                        CreateMarker(markerOwner);
                        break;
                    }
                case MarkerOwner.Enemy:
                    {
                        spriteRenderer.color = EnemyMarkerColor;
                        _enemyMarkerTransform = activePlayerObject.transform;

                        CreateMarker(markerOwner);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(markerOwner), markerOwner, null);
            }


            var gameObject = FindInActiveObjectByName("P_E_Neutral 33", "");
            var activeGameObject = Object.Instantiate(gameObject, activePlayerObject.transform);

            activeGameObject.name = "TestDecoration";

            spriteRenderer = activeGameObject.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);

            gameObject = FindInActiveObjectByName("P_A_X12", "");
            activeGameObject = Object.Instantiate(gameObject, activePlayerObject.transform);

            activeGameObject.name = "TestDecoration";
            var localPosition = new Vector3(0, -0.5f, 0);
            activeGameObject.transform.localPosition = localPosition;
            var localScale = new Vector3(0.235f, 0.235f, 1);
            activeGameObject.transform.localScale = localScale;
            var localRotation = Quaternion.Euler(0, 180, 0);
            activeGameObject.transform.localRotation = localRotation;
        }

        private static IEnumerator MarkerUpdater()
        {
            while (!inGame)
            {
                if (_ownMarker != null)
                {
                    _ownMarker.SetActive(ConfigController.OwnEnabledConfig.Value);
                    _ownMarker.transform.localScale = new Vector3(ownMarkerSize.y, ownMarkerSize.x, 1f);
                    MarkerController.SpriteColor(_ownMarker, OwnMarkerColor, ownMarkerBloom);

                    if (OwnMarker.typeSlider != null)
                    {
                        var textMeshProUgui = OwnMarker.typeSlider.GetComponentsInChildren<TextMeshProUGUI>()[2];
                        textMeshProUgui.text = ((MarkerController.MarkerType)ownMarkerType).ToString().ToUpperInvariant();
                    }
                }
                if (_teamMarker != null)
                {
                    _teamMarker.SetActive(ConfigController.TeamEnabledConfig.Value);
                    _teamMarker.transform.localScale = new Vector3(teamMarkerSize.y, teamMarkerSize.x, 1f);
                    MarkerController.SpriteColor(_teamMarker, TeamMarkerColor, teamMarkerBloom);
                }
                if (_enemyMarker != null)
                {
                    _enemyMarker.SetActive(ConfigController.EnemyEnabledConfig.Value);
                    _enemyMarker.transform.localScale = new Vector3(enemyMarkerSize.y, enemyMarkerSize.x, 1f);
                    MarkerController.SpriteColor(_enemyMarker, EnemyMarkerColor, enemyMarkerBloom);
                    
                }
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(10);
        }

        public static void CreateMarker(MarkerOwner markerOwner)
        {
            switch (markerOwner)
            {
                case MarkerOwner.Own:
                    if (_ownMarker != null)
                        Object.Destroy(_ownMarker);
                    _ownMarker = Object.Instantiate(MarkerController.MarkerTypeObject((MarkerController.MarkerType)ownMarkerType), _ownMarkerTransform);
                    _ownMarker.transform.localPosition = new Vector3(0, 2, 0);
                    _ownMarker.transform.localScale = new Vector3(0.75f, 0.75f, 1);
                    _ownMarker.transform.localRotation = Quaternion.Euler(0, 0, 0);

                    MarkerController.SpriteColor(_ownMarker, OwnMarkerColor, ownMarkerBloom);
                    break;
                case MarkerOwner.Team:
                    if (_teamMarker != null)
                        Object.Destroy(_teamMarker);
                    _teamMarker = Object.Instantiate(MarkerController.MarkerTypeObject((MarkerController.MarkerType)teamMarkerType), _teamMarkerTransform);
                    _teamMarker.transform.localPosition = new Vector3(0, 2, 0);
                    _teamMarker.transform.localScale = new Vector3(0.75f, 0.75f, 1);
                    _teamMarker.transform.localRotation = Quaternion.Euler(0, 0, 0);

                    MarkerController.SpriteColor(_teamMarker, TeamMarkerColor, teamMarkerBloom);
                    break;
                case MarkerOwner.Enemy:
                    if (_enemyMarker != null)
                        Object.Destroy(_ownMarker);
                    _enemyMarker = Object.Instantiate(MarkerController.MarkerTypeObject((MarkerController.MarkerType)enemyMarkerType), _enemyMarkerTransform);
                    _enemyMarker.transform.localPosition = new Vector3(0, 2, 0);
                    _enemyMarker.transform.localScale = new Vector3(0.75f, 0.75f, 1);
                    _enemyMarker.transform.localRotation = Quaternion.Euler(0, 0, 0);

                    MarkerController.SpriteColor(_enemyMarker, EnemyMarkerColor, enemyMarkerBloom);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(markerOwner), markerOwner, null);
            }
        }

        private static GameObject FindInActiveObjectByName(string name, string parent)
        {
            Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>();
            foreach (var t in objs)
            {
                if (t.hideFlags != HideFlags.None) continue;
                if (t.name != name) continue;
                if (parent != "")
                {
                    if (t.parent.parent.parent.name == parent)
                    {
                        return t.gameObject;
                    }
                }
                else
                {
                    return t.gameObject;
                }
            }
            return null;
        }

        public enum MarkerOwner
        {
            Own,
            Team,
            Enemy
        }
    }
}
