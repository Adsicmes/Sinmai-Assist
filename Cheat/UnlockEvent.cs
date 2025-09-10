﻿using HarmonyLib;
using Net.Packet;
using Net.Packet.Mai2;
using Net.VO;
using Net.VO.Mai2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SinmaiAssist.Cheat;

public class UnlockEvent
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PacketGetGameEvent), "Proc")]
    public static void Unlock(PacketGetGameEvent __instance, ref PacketState __result)
    {
        if (__result != PacketState.Done) return;
        NetQuery<GameEventRequestVO, GameEventResponseVO> netQuery = __instance.Query as NetQuery<GameEventRequestVO, GameEventResponseVO>;
        List<GameEvent> list = new List<GameEvent>();

        ReadAllEvents("./Sinmai_Data/StreamingAssets/", ref list);
        if (Directory.Exists("./option"))
        {
            ReadAllEvents("./option", ref list);
        }

        netQuery.Response.gameEventList = list.ToArray();
        FieldInfo onDoneField = typeof(PacketGetGameEvent).GetField("_onDone", BindingFlags.NonPublic | BindingFlags.Instance);
        Action<GameEvent[]> onDone = (Action<GameEvent[]>)onDoneField.GetValue(__instance);
        onDone?.Invoke(netQuery.Response.gameEventList ?? Array.Empty<GameEvent>());
    }

    private static void ReadAllEvents(string path, ref List<GameEvent> list)
    {
        var rootDir = new DirectoryInfo(path);

        foreach (var optFolder in rootDir.EnumerateDirectories("*", SearchOption.AllDirectories))
        {
            var eventPath = Path.Combine(optFolder.FullName, "event");
            if (int.TryParse(optFolder.Name.Substring(1), out var optNumber) && Directory.Exists(eventPath))
            {
                var dir = new DirectoryInfo(eventPath);
                foreach (var eventFolder in dir.EnumerateDirectories("*", SearchOption.AllDirectories))
                {
                    if (eventFolder.Name.StartsWith("event") && int.TryParse(eventFolder.Name.Replace("event", ""), out var eventId))
                    {
                        if (eventId > 0)
                        {
                            list.Add(new GameEvent
                            {
                                id = eventId,
                                startDate = "2000-01-01 00:00:00",
                                endDate = "2077-07-21 11:45:14",
                                type = 1
                            });
                        }
                    }
                }
            }
        }
    }
}