using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FYP_ChildAbuseGIS2013.SkinDetection
{
    public abstract class IColorSkinDetector
    {
        public abstract Image<Gray, Byte> DetectSkin(Image<Bgr, Byte> Img, IColor min, IColor max);
    }
}
