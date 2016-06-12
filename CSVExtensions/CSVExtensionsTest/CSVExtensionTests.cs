using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MBW;
using System.Collections.Generic;

namespace CSVExtensionsTest
{
    [TestClass]
    public class CSVExtensionTests
    {
        [TestClass]
        public class EscapeForCsv
        {
            [TestMethod]
            public void Should_Return_The_Same_Value_When_Nothing_Needs_Escaping()
            {
                string str = "A string with nothing special in it.";

                string result = str.EscapeForCsv();

                Assert.AreEqual(str, result);
            }
            
            [TestMethod]
            public void Should_Double_Quote_Enclose_The_Field_When_A_Comma_Is_Present()
            {
                string str = "A string with a comma, in the middle.";

                string expected = "\"A string with a comma, in the middle.\"";

                string result = str.EscapeForCsv();

                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void Should_Double_Quote_Enclose_The_Field_When_A_Line_break_Is_Present()
            {
                string strA = "A string with a \r return.";
                string strB = "A string with a \n newline.";
                string strC = "A string with a \r\n return and newline.";

                string expectedA = "\"A string with a \r return.\"";
                string expectedB = "\"A string with a \n newline.\"";
                string expectedC = "\"A string with a \r\n return and newline.\"";

                string resultA = strA.EscapeForCsv();
                string resultB = strB.EscapeForCsv();
                string resultC = strC.EscapeForCsv();

                Assert.AreEqual(expectedA, resultA);
                Assert.AreEqual(expectedB, resultB);
                Assert.AreEqual(expectedC, resultC);
            }

            [TestMethod]
            public void Should_Double_Quote_Enclose_The_Field_And_Escape_The_Quote_When_A_Double_Quote_Is_Present()
            {
                string str = "A string with a \"dblquote\" in the middle.";

                string expected = "\"A string with a \"\"dblquote\"\" in the middle.\"";

                string result = str.EscapeForCsv();

                Assert.AreEqual(expected, result);
            }
        }

        [TestClass]
        public class ToCsvRow
        {
            [TestMethod]
            public void Should_Return_A_CSV_Formatted_String()
            {
                TestModel t = new TestModel()
                {
                    Id = 1,
                    Name = "Bill Gates",
                    Email = "billg@microsoft.com",
                    SomethingElse = "Hello"
                };

                string expected = "1,Bill Gates,billg@microsoft.com,Hello";

                string result = t.ToCsvRow();

                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void Should_Return_A_CSV_Row_That_Is_Correctly_Escaped_And_Enclosed()
            {
                TestModel t = new TestModel()
                {
                    Id = 1,
                    Name = "Bill Gates",
                    Email = "billg@microsoft.com",
                    SomethingElse = "Hello, \r\n\r\nThis is a test field, with all sorts of \"things\" in it."
                };

                string expected = "1,Bill Gates,billg@microsoft.com,\"Hello, \r\n\r\nThis is a test field, with all sorts of \"\"things\"\" in it.\"";

                string result = t.ToCsvRow();

                Assert.AreEqual(expected, result);
            }
        }

        [TestClass]
        public class ToCsv
        {
            private List<TestModel> GetTestDataList()
            {
                List<TestModel> list = new List<TestModel>();

                list.Add(new TestModel()
                {
                    Id = 1,
                    Name = "Bill Gates",
                    Email = "billg@microsoft.com",
                    SomethingElse = "Hello"
                });

                list.Add(new TestModel()
                {
                    Id = 2,
                    Name = "Elon Musk",
                    Email = "ironman@tesla.com",
                    SomethingElse = "Bonjour!"
                });

                list.Add(new TestModel()
                {
                    Id = 3,
                    Name = "Mr Bean",
                    Email = "bean@noabsolutely.wtf",
                    SomethingElse = "Hello, \r\n\r\nThis is a test field, with all sorts of \"things\" in it."
                });

                return list;
            }

            private List<string> GetExpectedResultList()
            {
                return new List<string>()
                {
                    "1,Bill Gates,billg@microsoft.com,Hello\r\n",
                    "2,Elon Musk,ironman@tesla.com,Bonjour!\r\n",
                    "3,Mr Bean,bean@noabsolutely.wtf,\"Hello, \r\n\r\nThis is a test field, with all sorts of \"\"things\"\" in it.\"\r\n"
                };
            }

            [TestMethod]
            public void Should_Return_An_IEnumarable_Of_CSV_Strings()
            {
                var data = GetTestDataList();
                var expectedResults = GetExpectedResultList();

                List<string> result = new List<string>();

                foreach (var line in data.ToCsv(false))
                {
                    result.Add(line);
                }

                Assert.AreEqual(3, result.Count);

                Assert.AreEqual(expectedResults[0], result[0]);
                Assert.AreEqual(expectedResults[1], result[1]);
                Assert.AreEqual(expectedResults[2], result[2]);
            }

            [TestMethod]
            public void Should_Return_Headers_As_The_First_Item_When_Specified()
            {
                var data = GetTestDataList();

                var expectedFirstRow = "Id,Name,Email,SomethingElse\r\n";

                List<string> result = new List<string>();

                foreach (var line in data.ToCsv())
                {
                    result.Add(line);
                }

                Assert.AreEqual(4, result.Count);
                Assert.AreEqual(expectedFirstRow, result[0]);
            }

        }

    }
}
