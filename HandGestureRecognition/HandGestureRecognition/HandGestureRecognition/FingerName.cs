using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandGestureRecognition
{
    public class FingerNameClass
    {
        public enum FingerName { LITTLE, RING, MIDDLE, INDEX, THUMB, UNKNOWN };

        public FingerName getNext(Enum current)
        {
            //set current index to 0
            int currentIdx = 0;
            //initialize a Enum FingerName
            FingerName f = FingerName.UNKNOWN;
            //loop to find the current Enum index and value
            foreach (var value in Enum.GetValues(typeof(FingerName)))
            {
                if (current == value)
                {
                    f = (FingerName)value;
                    break;
                }
                else
                {
                    currentIdx++;
                }
            }
            //add 1 to get next index of Enum
            int nextIdx = currentIdx + 1;
            //check if next index value is UNKNOWN
            if (nextIdx == Enum.GetValues(typeof(FingerName)).Length)
            {
                nextIdx = 0;
            }
            //return the value of next Enum
            return f;
        } // end of getNext()

        public FingerName getPrev(Enum current)
        {
            //set current index to last
            int currentIdx = 5;
            //initialize a Enum FingerName 
            FingerName f = FingerName.UNKNOWN;
            //loop to find the current Enum index and value
            foreach (var value in Enum.GetValues(typeof(FingerName)))
            {
                if (current == value)
                {
                    f = (FingerName)value;
                    break;
                }
                else
                {
                    currentIdx--;
                }
            }
            //minus 1 to get previous index of Enum
            int prevIdx = currentIdx - 1;
            //check if previous index is more than 0
            if (prevIdx < 0)
            {
                prevIdx = (FingerName.GetValues(typeof(FingerName)).Length) - 1;
            }
            //return the value of previous Enum
            return f;
        } // end of getPrev()
    }
}