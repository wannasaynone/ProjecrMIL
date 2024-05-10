using System.Collections;
using System.Collections.Generic;
using KahaGameCore.GameData;
using UnityEngine;

namespace ProjectMIL.Data
{
    public class ContextData : IGameData
    {
        public int ID { get; private set; }
        public string zh_tw { get; private set; }
    }
}
