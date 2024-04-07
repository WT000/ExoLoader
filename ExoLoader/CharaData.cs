﻿using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ExoLoader
{
    public class CharaData
    {
        public string id;
        public string name;
        public string nickname;
        public GenderID gender;
        public bool canLove;
        public int ageOffset;
        public string birthday;
        public string dialogueColor;
        public string defaultBg;
        public string basicInfo;
        public string moreInfo;
        public string augment;
        public string slider1left;
        public string slider1right;
        public int[] slider1values;
        public string slider2left;
        public string slider2right;
        public int[] slider2values;
        public string slider3left;
        public string slider3right;
        public int[] slider3values;
        public bool charaGotMade;
        public bool helioOnly;
        public string[] likes;
        public string[] dislikes;
        public string skeleton;
        public float[] stratoMapSpot;
        public float[] helioMapSpot;
        public float[] destroyedMapSpot;

        public string folderName;
        public bool ages;
        public bool onMap = true;
        public int spriteSize = -1;

        public CustomChara MakeChara()
        {
            if (charaGotMade)
            {
                ModInstance.instance.Log("Making a chara with data that was already used????");
            }
            charaGotMade = true;
            int birthMonth = Season.GetMonthOfYear(birthday);

            CharaFillbarData fillbar1 = new CharaFillbarData();
            fillbar1.labelLeft = slider1left;
            fillbar1.labelRight = slider1right;
            fillbar1.value1 = slider1values[0];
            fillbar1.value2 = slider1values[1];
            fillbar1.value3 = slider1values[2];
            CharaFillbarData fillbar2 = new CharaFillbarData();
            fillbar2.labelLeft = slider2left;
            fillbar2.labelRight = slider2right;
            fillbar2.value1 = slider2values[0];
            fillbar2.value2 = slider2values[1];
            fillbar2.value3 = slider2values[2];
            CharaFillbarData fillbar3 = new CharaFillbarData();
            fillbar3.labelLeft = slider3left;
            fillbar3.labelRight = slider3right;
            fillbar3.value1 = slider3values[0];
            fillbar3.value2 = slider3values[1];
            fillbar3.value3 = slider3values[2];

            CharaFillbarData[] charaFillbarDatas = {fillbar1, fillbar2, fillbar3};

            return new CustomChara(id, nickname, gender, canLove, ageOffset, birthMonth, dialogueColor, defaultBg, charaFillbarDatas, name, basicInfo, moreInfo, augment, this);
        }
    }
}
