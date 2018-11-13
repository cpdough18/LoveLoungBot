using System;
using System.Collections.Generic;
using System.Text;

namespace Radon.Services.External
{
    public class ConversionService
    {
        public float DpToHp(float dp)
        {
            return dp / 30f;
        } 
        public float HpToDp(float hp)
        {
            return hp * 30f;
        }
    }
}
