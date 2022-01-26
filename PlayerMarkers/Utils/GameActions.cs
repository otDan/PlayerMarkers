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
                var myPlayers = PlayerManager.instance.players.Where(p => p.data.view.IsMine);
                foreach (Player player in myPlayers)
                {
                    var markerObject = ObjectManager.CreateObject(player.GetTeamColors().color);
                    markerObject.transform.SetParent(player.gameObject.transform);
                    markerObject.transform.localPosition = new Vector3(0, 2, 0);
                    markerObject.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                    markerObject.transform.localRotation = Quaternion.Euler(0, 180, 180);
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
