using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stroke_detection.Processing.Bitmap.LUTMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stroke_detection.Processing.Bitmap.LUTMethods.Tests
{
    [TestClass()]
    public class HistogramEqualizationTests
    {
        [TestMethod()]
        public void CreateLUTTest()
        {
            HistogramEqualize test = new HistogramEqualize();
            Assert.IsNull(test.CreateLUT(null));
            Assert.IsNull(test.CreateLUT(new int[10]));
            Assert.IsNotNull(test.CreateLUT(new int[256]));
            int[] hist = new int[256];
            for(int i = 0; i<256; i++)
            {
                hist[i] = i;
            }
            Assert.IsNotNull(test.CreateLUT(hist));
        }
    }
}