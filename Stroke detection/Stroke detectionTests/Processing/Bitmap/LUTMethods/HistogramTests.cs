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
    public class HistogramTests
    {
        [TestMethod()]
        public void CreateHistogramFromIntTest()
        {
            Histogram test = new Histogram();
            Assert.IsFalse(test.CreateHistogramFromInt(null));
            Assert.IsFalse(test.CreateHistogramFromInt(new int[,] { { 300 }, { 100 } }));
            Assert.IsTrue(test.CreateHistogramFromInt(new int[,] { { 1 }, { 1 } }));
            Assert.IsNotNull(test.HistogramValues);

            int[] testHist = new int[256];
            for (int x = 0; x < 256; x++)
                testHist[x] = 0;
            testHist[1] = 2;
            Assert.AreEqual(testHist.Length, test.HistogramValues.Length);
            for (int x = 0; x < 256; x++)
                Assert.AreEqual(test.HistogramValues[x], testHist[x]);
            Assert.IsTrue(test.CreateHistogramFromInt(new int[1000,10000]));
        }
    }
}