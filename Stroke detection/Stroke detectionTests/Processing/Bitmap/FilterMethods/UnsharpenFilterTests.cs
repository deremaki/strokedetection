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
    public class UnsharpenFilterTests
    {
        [TestMethod()]
        public void UnsharpenTest()
        {
            UnsharpenFilter test = new UnsharpenFilter();
            Assert.IsNull(test.Unsharpen(null,null));
            Assert.IsNull(test.Unsharpen(new int[1,1], null));
            Assert.IsNull(test.Unsharpen(null, new int[1, 1]));
            Assert.IsNull(test.Unsharpen(new int[1, 2], new int[1, 1]));
            Assert.IsNull(test.Unsharpen(new int[3, 1], new int[1, 1]));
            Assert.IsNotNull(test.Unsharpen(new int[1000, 10000], new int[1000, 10000]));
        }
    }
}