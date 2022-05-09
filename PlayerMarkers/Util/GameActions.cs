using PlayerMarkers.Menu;
using PlayerMarkers.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerMarkers.Asset;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.Extensions;
using UnboundLib.GameModes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PlayerMarkers.Util
{
    public static class GameActions
    {
        internal static IEnumerator GameStart(IGameModeHandler gameModeHandler)
        {
            try
            {
                MenuController.inGame = true;
                
                var myPlayers = PlayerManager.instance.players.Where(p => p.data.view.IsMine);
                foreach (Player player in myPlayers)
                {
                    if (ConfigController.OwnEnabledConfig.Value)
                    {
                        var markerObject = Object.Instantiate(MarkerController.MarkerTypeObject((MarkerController.MarkerType)MenuController.ownMarkerType), player.gameObject.transform, true);
                        markerObject.transform.localPosition = new Vector3(0, 2, -10);
                        markerObject.transform.localScale = new Vector3(MenuController.ownMarkerSize.x, MenuController.ownMarkerSize.y, 0.75f);
                        // markerObject.transform.localRotation = Quaternion.Euler(0, 180, 180);

                        MarkerController.SpriteColor(markerObject, player.GetTeamColors().color, MenuController.ownMarkerBloom);
                    }

                    if (ConfigController.TeamEnabledConfig.Value)
                    {
                        var teamPlayers = PlayerManager.instance.players.Where(p => p.teamID == player.teamID && p.playerID != player.playerID);
                        foreach (Player teamPlayer in teamPlayers)
                        {
                            var markerObject = Object.Instantiate(MarkerController.MarkerTypeObject((MarkerController.MarkerType)MenuController.teamMarkerType), teamPlayer.gameObject.transform, true);
                            markerObject.transform.localPosition = new Vector3(0, 2, -10);
                            markerObject.transform.localScale = new Vector3(MenuController.teamMarkerSize.x, MenuController.teamMarkerSize.y, 0.75f);
                            // markerObject.transform.localRotation = Quaternion.Euler(0, 180, 180);

                            MarkerController.SpriteColor(markerObject, teamPlayer.GetTeamColors().color, MenuController.teamMarkerBloom);
                        }
                    }

                    if (!ConfigController.EnemyEnabledConfig.Value) continue;
                    {
                        var enemyPlayers = PlayerManager.instance.players.Where(p => p.teamID != player.teamID && !p.data.view.IsMine);
                        foreach (Player enemyPlayer in enemyPlayers)
                        {
                            var markerObject = Object.Instantiate(MarkerController.MarkerTypeObject((MarkerController.MarkerType)MenuController.enemyMarkerType), enemyPlayer.gameObject.transform, true);
                            markerObject.transform.localPosition = new Vector3(0, 2, -10);
                            markerObject.transform.localScale = new Vector3(MenuController.enemyMarkerSize.x, MenuController.enemyMarkerSize.y, 0.75f);
                            // markerObject.transform.localRotation = Quaternion.Euler(0, 180, 180);

                            MarkerController.SpriteColor(markerObject, enemyPlayer.GetTeamColors().color, MenuController.enemyMarkerBloom);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            yield break;
        }

        internal static IEnumerator GameEnd(IGameModeHandler gameModeHandler)
        {
            try
            {
                MenuController.inGame = false;
                var myPlayers = PlayerManager.instance.players.Where(p => p.data.view.IsMine);
                foreach (Player player in myPlayers)
                {
                    var markerObject = player.gameObject.transform.Find("Marker");
                    Object.Destroy(markerObject);
                }
            }
            catch (Exception)
            {
                // ignored
            }

            yield break;
        }
    }
}
