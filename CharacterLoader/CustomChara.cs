using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterLoader
{
    public class CustomChara : Chara
    {
        public CharaData data;

        public CustomChara(string idString, string nickname, GenderID genderID, bool canLove, int ageOffset, int birthMonthOfYear, string dialogColor, string defaultBackground, CharaFillbarData[] fillbarDatas, string name, string basics, string more, string enhancement, CharaData data)
            : base(idString, nickname, genderID, canLove, ageOffset, birthMonthOfYear, dialogColor, defaultBackground, fillbarDatas, name, basics, more, enhancement)
        {
            this.data = data;
            
        }
    }
}
