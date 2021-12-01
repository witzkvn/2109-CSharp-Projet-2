using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using WildPay.Tools;

namespace WildPay.Tests.Tools
{

    [TestClass]
    public class ToolsTests
    {
        public ToolsTests()
        {

        }

        private TestContext testContextInstance;

        /// <summary>
        ///Obtient ou définit le contexte de test qui fournit
        ///des informations sur la série de tests active, ainsi que ses fonctionnalités.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Attributs de tests supplémentaires
        //
        // Vous pouvez utiliser les attributs supplémentaires suivants lorsque vous écrivez vos tests :
        //
        // Utilisez ClassInitialize pour exécuter du code avant d'exécuter le premier test de la classe
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Utilisez ClassCleanup pour exécuter du code une fois que tous les tests d'une classe ont été exécutés
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Utilisez TestInitialize pour exécuter du code avant d'exécuter chaque test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Utilisez TestCleanup pour exécuter du code après que chaque test a été exécuté
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void PasswordHash()
        {
            Assert.AreNotEqual("password", FormatTools.HashPassword("password"));

            string toHash = "gehehgzhehehzs";
            string passwordHashe = FormatTools.HashPassword(toHash);
            Assert.AreEqual(passwordHashe, FormatTools.HashPassword(toHash));
        }

        [TestMethod]
        public void DateFormatIncorrect()
        {
            Assert.IsFalse(FormatTools.IsDateOk("hntnrtnrt"));
        }

        [TestMethod]
        public void DateOk()
        {
            Assert.IsTrue(FormatTools.IsDateOk("02/03/2011"));
            Assert.IsTrue(FormatTools.IsDateOk("28/08/2015"));
        }

        [TestMethod]
        public void DateFuture()
        {
            DateTime nextWeek = DateTime.Today.AddDays(7);
            Assert.IsFalse(FormatTools.IsDateOk(nextWeek.ToString()));
        }

        [TestMethod]
        public void DateTooOld()
        {
            Assert.IsFalse(FormatTools.IsDateOk("28/08/2005"));
        }

        [TestMethod]
        public void PasswordOk()
        {
            Assert.IsTrue(FormatTools.IsPasswordFormatOk("12lepassword="));
            Assert.IsTrue(FormatTools.IsPasswordFormatOk("mon4+"));
        }
        [TestMethod]
        public void PasswordTooShort()
        {
            Assert.IsFalse(FormatTools.IsPasswordFormatOk("12l="));
        }

        [TestMethod]
        public void PasswordOnlyLetters()
        {
            Assert.IsFalse(FormatTools.IsPasswordFormatOk("blaBlabla"));
        }

        [TestMethod]
        public void PasswordOnlyMaj()
        {
            Assert.IsFalse(FormatTools.IsPasswordFormatOk("BLABLABLA"));
        }

        [TestMethod]
        public void PasswordOnlySpecial()
        {
            Assert.IsFalse(FormatTools.IsPasswordFormatOk("=+_-*=+"));
        }

        [TestMethod]
        public void PasswordOnlyDigits()
        {
            Assert.IsFalse(FormatTools.IsPasswordFormatOk("45896571"));
        }

        [TestMethod]
        public void DateTimeShortFormatConvert()
        {
            DateTime date = new DateTime(2020, 2, 15); // 15 fevrier 2020
            Assert.AreEqual("15/02/2020",FormatTools.ConvertInShortDate(date));
        }
    }
}
