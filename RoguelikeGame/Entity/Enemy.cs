using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityNamespace
{
    public class Enemy : Entity
    {
        public Enemy(string assetName, int startPosX, int startPosY) : base(assetName, startPosX, startPosY)
        {
        }
    }
}
