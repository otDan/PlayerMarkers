using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.Extensions;
using UnboundLib.GameModes;
using UnityEngine;

namespace GameEnhancementCards.Utils
{
    public static class GameActions
    {
        internal static IEnumerator GameStart(IGameModeHandler gameModeHandler)
        {
            try
            {
                PlayerMarkers.inGame = true;
                var myPlayers = PlayerManager.instance.players.Where(p => p.data.view.IsMine);
                foreach (Player player in myPlayers)
                {
                    if (PlayerMarkers.ownMarkerEnabled)
                    {
                        var markerObject = ObjectManager.CreateObject("PlayerMarker", player.GetTeamColors().color);
                        markerObject.transform.SetParent(player.gameObject.transform);
                        markerObject.transform.localPosition = new Vector3(0, 2, 0);
                        markerObject.transform.localScale = new Vector3(PlayerMarkers.ownMarkerSize.x, PlayerMarkers.ownMarkerSize.y, 0.75f);
                        markerObject.transform.localRotation = Quaternion.Euler(0, 180, 180);
                    }

                    if (PlayerMarkers.teamMarkerEnabled)
                    {
                        var teamPlayers = PlayerManager.instance.players.Where(p => p.teamID == player.teamID && p.playerID != player.playerID);
                        foreach (Player teamPlayer in teamPlayers)
                        {
                            var markerObject = ObjectManager.CreateObject("TeamMarker", teamPlayer.GetTeamColors().color);
                            markerObject.transform.SetParent(teamPlayer.gameObject.transform);
                            markerObject.transform.localPosition = new Vector3(0, 2, 0);
                            markerObject.transform.localScale = new Vector3(PlayerMarkers.teamMarkerSize.x, PlayerMarkers.teamMarkerSize.y, 0.75f);
                            markerObject.transform.localRotation = Quaternion.Euler(0, 180, 180);
                        }
                    }

                    if (PlayerMarkers.enemyMarkerEnabled)
                    {
                        var enemyPlayers = PlayerManager.instance.players.Where(p => p.teamID != player.teamID);
                        foreach (Player enemyPlayer in enemyPlayers)
                        {
                            var markerObject = ObjectManager.CreateObject("EnemyMarker", enemyPlayer.GetTeamColors().color);
                            markerObject.transform.SetParent(enemyPlayer.gameObject.transform);
                            markerObject.transform.localPosition = new Vector3(0, 2, 0);
                            markerObject.transform.localScale = new Vector3(PlayerMarkers.enemyMarkerSize.x, PlayerMarkers.enemyMarkerSize.y, 0.75f);
                            markerObject.transform.localRotation = Quaternion.Euler(0, 180, 180);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                //UnityEngine.Debug.Log($"[{GameEnhancementCards.ModInitials}] Exception happened {exception}.");
            }

            yield break;
        }

        internal static IEnumerator GameEnd(IGameModeHandler gameModeHandler)
        {
            try
            {
                PlayerMarkers.inGame = false;
                var myPlayers = PlayerManager.instance.players.Where(p => p.data.view.IsMine);
                foreach (Player player in myPlayers)
                {
                    var markerObject = player.gameObject.transform.Find("Marker");
                    DestroyObjects.Destroy(markerObject);
                }
            }
            catch (Exception exception)
            {
                //UnityEngine.Debug.Log($"[{GameEnhancementCards.ModInitials}] Exception happened {exception}.");
            }

            yield break;
        }
    }
}
