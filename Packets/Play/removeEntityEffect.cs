﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libMC.NET.Packets.Play {
    class removeEntityEffect : Packet {
        public removeEntityEffect(ref Minecraft mc) {
            int Entity_ID = mc.nh.wSock.readInt();
            byte Effect_ID = mc.nh.wSock.readByte();

            if (mc.MinecraftWorld != null) {
                int eIndex = mc.MinecraftWorld.getEntityById(Entity_ID);

                if (eIndex != -1) {
                    mc.MinecraftWorld.Entities[eIndex].status = Effect_ID;
                    mc.raiseEntityStatus(Entity_ID);
                }
            }
        }
    }
}
