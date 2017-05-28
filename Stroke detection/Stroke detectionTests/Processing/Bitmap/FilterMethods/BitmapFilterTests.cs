using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stroke_detection.Processing.Bitmap.FilterMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stroke_detection.Processing.Bitmap.FilterMethods.Tests
{
    [TestClass()]
    public class BitmapFilterTests
    {
        [TestMethod()]
        public void ProcessIntTest()
        {
            BitmapFilter test = new BitmapFilter();
            test.SetMask(new int[,] { { 1, 1, 1 }, { 1, 2, 1 }, { 1, 1, 1 } });
            Assert.IsNull(test.ProcessInt(null));
            Assert.IsNotNull(test.ProcessInt(new int[1000,10000]));
        }
    }
}