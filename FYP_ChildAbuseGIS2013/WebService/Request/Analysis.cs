using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Request
{
    public class Analysis
    {
        public int ID { get; set; }
        public int videoID { get; set; }
        public int totalFrame { get; set; }
        public int smileFrame { get; set; }
        public int angryFrame { get; set; }
        public int sadFrame { get; set; }
        public int neutralFrame { get; set; }
        public int leftFistFrame { get; set; }
        public int rightFistFrame { get; set; }
        public int leftPalmFrame { get; set; }
        public int rightPalmFrame { get; set; }

        public Analysis() { }

        public Analysis(int ID, int total, int smile, int angry, int sad, int neut, int leftFist, int rightFist, int leftPalm, int rightPalm)
        {
            this.ID = ID;
            this.totalFrame = total;
            this.smileFrame = smile;
            this.angryFrame = angry;
            this.sadFrame = sad;
            this.neutralFrame = neut;
            this.leftFistFrame = leftFist;
            this.rightFistFrame = rightFist;
            this.leftPalmFrame = leftPalm;
            this.rightPalmFrame = rightPalm;
        }

        public Analysis(int total, int smile, int angry, int sad, int neut, int leftFist, int rightFist, int leftPalm, int rightPalm)
        {
            this.totalFrame = total;
            this.smileFrame = smile;
            this.angryFrame = angry;
            this.sadFrame = sad;
            this.neutralFrame = neut;
            this.leftFistFrame = leftFist;
            this.rightFistFrame = rightFist;
            this.leftPalmFrame = leftPalm;
            this.rightPalmFrame = rightPalm;
        }
    }
}