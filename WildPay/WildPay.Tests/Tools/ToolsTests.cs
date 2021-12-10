using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using WildPay.Models;
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

        #region TestsPassword
        // au moins 1 chiffre 
        // au moins 1 caractère special 
        // au moins 5 caractères de long

        [TestMethod]
        public void PasswordHash()
        {
            Assert.AreNotEqual("password", FormatTools.HashPassword("password"), "Le MDP devraient être différents");

            string toHash = "gehehgzhehehzs";
            string passwordHashe = FormatTools.HashPassword(toHash);
            Assert.AreEqual(passwordHashe, FormatTools.HashPassword(toHash), "Le MDP devraient être égaux");
        }

        [TestMethod]
        public void PasswordOk()
        {
            Assert.IsTrue(FormatTools.IsPasswordFormatOk("aaa4*"), "Le MDP devrait être valide");
            Assert.IsTrue(FormatTools.IsPasswordFormatOk("12lepassword="), "Le MDP devrait être au bon format");
        }

        public void PasswordOkWithSpace()
        { 
            Assert.IsTrue(FormatTools.IsPasswordFormatOk(" aaa4 * "), "Le MDP devrait être valide avec espace");
        }

            [TestMethod]
        public void PasswordTooShort()
        {
            Assert.IsFalse(FormatTools.IsPasswordFormatOk("12l="), "Le MDP devrait être invalide (trop court)");
        }

        [TestMethod]
        public void PasswordOnlyLetters()
        {
            Assert.IsFalse(FormatTools.IsPasswordFormatOk("blaBlabla"), "Le MDP devrait être invalide (absence chiffre et caractère spécial)");
        }

        [TestMethod]
        public void PasswordNoSpecialCharact()
        {
            Assert.IsFalse(FormatTools.IsPasswordFormatOk("aaaa4"), "Le MDP devrait être invalide (absence caractère spécial)");
        }

        [TestMethod]
        public void PasswordOnlySpecial()
        {
            Assert.IsFalse(FormatTools.IsPasswordFormatOk("=+_-*=+"), "Le MDP devrait être invalide (absence lettre et chiffre)");
        }

        [TestMethod]
        public void PasswordOnlyDigits()
        {
            Assert.IsFalse(FormatTools.IsPasswordFormatOk("45896571"), "Le MDP devrait être invalide (absence lettre et caractère spécial)");
        }
        #endregion

        #region TestsDate
        [TestMethod]
        public void ConvertInShortDateOk()
        {
            DateTime date1 = new DateTime(1992, 01, 01);
            DateTime date2 = new DateTime(2000, 7, 26);
            DateTime date3 = new DateTime(2025, 12, 30);
            Assert.AreEqual(FormatTools.ConvertInShortDate(date1), "01/01/1992", "devrait renvoyer 01/01/1992");
            Assert.AreEqual(FormatTools.ConvertInShortDate(date2), "26/07/2000", "devrait renvoyer 26/07/2000");
            Assert.AreEqual(FormatTools.ConvertInShortDate(date3), "30/12/2025", "devrait renvoyer 30/12/2025");
        }

        [TestMethod]
        public void ConvertInShortDateReturnFalse()
        {
            DateTime date1 = new DateTime(1992, 01, 01);
            DateTime date2 = new DateTime(2000, 7, 26);
            DateTime date3 = new DateTime(2025, 12, 30);
            Assert.AreNotEqual(FormatTools.ConvertInShortDate(date1), "01-01-1992", "devrait renvoyer 01/01/1992");
            Assert.AreNotEqual(FormatTools.ConvertInShortDate(date2), "2000/07/26", "devrait renvoyer 26/07/2000");
            Assert.AreNotEqual(FormatTools.ConvertInShortDate(date3), "12/30/2025", "devrait renvoyer 30/12/2025");
        }
        #endregion

        #region TestsImage
        // max size = 2 000 000
        // formats : jpg, jpeg, png
        [TestMethod]
        public void CheckImageSizeOk()
        {
            MockFile file1 = new MockFile(195003, "image/jpg");
            MockFile file2 = new MockFile(195003, "image/jpeg");
            MockFile file3 = new MockFile(195003, "image/png");
            MockFile file4 = new MockFile(0, "image/png");

            Assert.IsTrue(FormatTools.VerifyImageFormatAndSize(file1), "devrait être valide");
            Assert.IsTrue(FormatTools.VerifyImageFormatAndSize(file2), "devrait être valide");
            Assert.IsTrue(FormatTools.VerifyImageFormatAndSize(file3), "devrait être valide");
            Assert.IsTrue(FormatTools.VerifyImageFormatAndSize(file4), "devrait être valide");
        }

        [TestMethod]
        public void CheckImageSizeNotOk()
        {
            MockFile file1 = new MockFile(2000000, "image/png");
            MockFile file2 = new MockFile(3000000, "image/png");
            MockFile file3 = new MockFile(150, "application/msword");
            MockFile file4 = new MockFile(150, "text/html");
            MockFile file5 = new MockFile(150, "image/bmp");

            Assert.IsFalse(FormatTools.VerifyImageFormatAndSize(file1), "devrait être invalide : time à la limite");
            Assert.IsFalse(FormatTools.VerifyImageFormatAndSize(file2), "devrait être invalide : taille trop grande");
            Assert.IsFalse(FormatTools.VerifyImageFormatAndSize(file3), "devrait être invalide : type application/msword");
            Assert.IsFalse(FormatTools.VerifyImageFormatAndSize(file4), "devrait être invalide : type text/html");
            Assert.IsFalse(FormatTools.VerifyImageFormatAndSize(file5), "devrait être invalide : type image/bmp");
        }

        #endregion

        #region TestsNombres
        [TestMethod]
        public void RoundDoubleTwoDigitsOk()
        {
            Assert.AreEqual(FormatTools.ConvertinShortDouble(18.182), 18.18);
            Assert.AreEqual(FormatTools.ConvertinShortDouble(99.990), 99.99);
            Assert.AreEqual(FormatTools.ConvertinShortDouble(0.123), 0.12);
            Assert.AreEqual(FormatTools.ConvertinShortDouble(0.128), 0.13);
        }

        #endregion

        #region TestsText
        [TestMethod]
        public void GetPremiereLettreMaj()
        {
            Assert.AreEqual(Utilities.GetPremiereLettreMajuscule("AAA"), "Aaa");
            Assert.AreEqual(Utilities.GetPremiereLettreMajuscule("aaa"), "Aaa");
        }
        [TestMethod]
        public void GetNomCompletFormatted()
        {
            User me = new User();
            me.Firstname = "john";
            me.Lastname = "doe";
            Assert.AreEqual(Utilities.GetNomCompletUser(me), "DOE John");

        }
        #endregion



    }
}
