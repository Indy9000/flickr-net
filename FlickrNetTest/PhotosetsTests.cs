using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlickrNet;
using System.IO;
using System.Diagnostics;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for FlickrPhotosetsGetList
    /// </summary>
    [TestClass]
    public class PhotosetsTests
    {
        Flickr f = new Flickr(TestData.ApiKey);

        public PhotosetsTests()
        {
            Flickr.CacheDisabled = true;
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void PhotosetsGetInfoBasicTest()
        {
            string photosetId = "72157594532130119";

            var p = TestData.GetInstance().PhotosetsGetInfo(photosetId);

            Assert.IsNotNull(p);
            Assert.AreEqual(photosetId, p.PhotosetId);
            Assert.AreEqual("Places: Derwent Walk, Gateshead", p.Title);
            Assert.AreEqual("It's near work, so I go quite a bit...", p.Description);
        }

        [TestMethod]
        public void PhotosetsGetListBasicTest()
        {
            PhotosetCollection photosets = f.PhotosetsGetList(TestData.TestUserId);

            Assert.IsTrue(photosets.Count > 0, "Should be at least one photoset");
            Assert.IsTrue(photosets.Count > 100, "Should be greater than 100 photosets. (" + photosets.Count + " returned)");

            foreach (Photoset set in photosets)
            {
                Assert.IsNotNull(set.OwnerId, "OwnerId should not be null");
                Assert.IsTrue(set.NumberOfPhotos > 0, "NumberOfPhotos should be greater than zero");
                Assert.IsNotNull(set.Title, "Title should not be null");
                Assert.IsNotNull(set.Description, "Description should not be null");
                Assert.AreEqual(TestData.TestUserId, set.OwnerId);
            }
        }

        [TestMethod]
        public void PhotosetsGetListWebUrlTest()
        {
            PhotosetCollection photosets = f.PhotosetsGetList(TestData.TestUserId);

            Assert.IsTrue(photosets.Count > 0, "Should be at least one photoset");

            foreach (Photoset set in photosets)
            {
                Assert.IsNotNull(set.Url);
                string expectedUrl = "http://www.flickr.com/photos/" + TestData.TestUserId + "/sets/" + set.PhotosetId + "/";
                Assert.AreEqual<string>(expectedUrl, set.Url);
            }
        }

        [TestMethod]
        public void PhotosetsCreateAddPhotosTest()
        {
            Flickr f = TestData.GetAuthInstance();

            byte[] imageBytes = TestData.TestImageBytes;
            Stream s = new MemoryStream(imageBytes);

            string title = "Test Title";
            string desc = "Test Description\nSecond Line";
            string tags = "testtag1,testtag2";

            s.Position = 0;
            // Upload photo once
            string photoId1 = f.UploadPicture(s, "Test.jpg", title, desc, tags, false, false, false, ContentType.Other, SafetyLevel.Safe, HiddenFromSearch.Visible);
            Debug.WriteLine("Photo 1 created: " + photoId1);

            s.Position = 0;
            // Upload photo a second time
            string photoId2 = f.UploadPicture(s, "Test.jpg", title, desc, tags, false, false, false, ContentType.Other, SafetyLevel.Safe, HiddenFromSearch.Visible);
            Debug.WriteLine("Photo 2 created: " + photoId2);

            // Creat photoset
            Photoset photoset = f.PhotosetsCreate("Test photoset", photoId1);
            Debug.WriteLine("Photoset created: " + photoset.PhotosetId);

            // Add second photo to photoset.
            f.PhotosetsAddPhoto(photoset.PhotosetId, photoId2);

            // Remove second photo from photoset
            f.PhotosetsRemovePhoto(photoset.PhotosetId, photoId2);

            // Delete photoset completely
            f.PhotosetsDelete(photoset.PhotosetId);

            // Delete both photos.
            f.PhotosDelete(photoId1);
            f.PhotosDelete(photoId2);
        }

        [TestMethod]
        public void PhotosetsGetInfoEncodingCorrect()
        {
            Flickr f = TestData.GetInstance();

            Photoset pset = f.PhotosetsGetInfo("72157627650627399");

            Assert.AreEqual("S�tio em Aruj� - 14/08/2011", pset.Title);
        }
    }
}
