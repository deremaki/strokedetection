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
    public class HistogramNormalizeTests
    {
        [TestMethod()]
        public void CreateLUTTest()
        {

            HistogramNormalize test = new HistogramNormalize();
            Assert.IsNull(test.CreateLUT(null));
            Assert.IsNull(test.CreateLUT(new int[10]));
            Assert.IsNotNull(test.CreateLUT(new int[256]));
        }
    }
}