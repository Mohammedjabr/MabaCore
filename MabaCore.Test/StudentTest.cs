using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MabaCore.Test
{
    [TestClass]
    public class StudentTest
    {
        [TestMethod]
        public void TestFullName()
        {
            //Arange
            string expected = "Mohammed Jabr";
            string firstName = "Mohammed";
            string LastName = "Jabr";
            //Act
            string FullName = firstName + " " + LastName;
            //Assert
            Assert.AreEqual(expected, FullName);
        }
        [TestMethod]
        public void TestSum()
        {
            //Arange
            int expected = 10;
            int num1 = 5;
            int num2 = 5;

            //Act
            int result = num1+num2;

            //Assert
            Assert.AreEqual(expected,result);
        }
    }
}
