using NUnit.Framework;

namespace Thunder.Web
{
    [TestFixture]
    class ContentTypeTest
    {
        [Test]
        public void GetFromFile()
        {
            Assert.AreEqual("application/pdf", ContentType.GetFromFile("arquivo.pdf"));
            Assert.AreEqual("image/jpeg", ContentType.GetFromFile("arquivo.jpg"));
            Assert.AreEqual("image/jpeg", ContentType.GetFromFile("arquivo.jpeg"));
            Assert.AreEqual("image/gif", ContentType.GetFromFile("arquivo.gif"));
            Assert.AreEqual("image/png", ContentType.GetFromFile("arquivo.png"));
            Assert.AreEqual("application/x-shockwave-flash", ContentType.GetFromFile("arquivo.swf"));
            Assert.AreEqual("text/richtext", ContentType.GetFromFile("arquivo.rtf"));
            Assert.AreEqual("audio/wav", ContentType.GetFromFile("arquivo.wav"));
            Assert.AreEqual("image/tiff", ContentType.GetFromFile("arquivo.tiff"));
            Assert.AreEqual("video/avi", ContentType.GetFromFile("arquivo.avi"));
            Assert.AreEqual("video/mpeg", ContentType.GetFromFile("arquivo.mpeg"));
            Assert.AreEqual("application/vnd.ms-excel", ContentType.GetFromFile("arquivo.csv"));
            Assert.AreEqual("application/msword", ContentType.GetFromFile("arquivo.doc"));
            Assert.AreEqual("application/vnd.openxmlformats-officedocument.wordprocessingml.document", ContentType.GetFromFile("arquivo.docx"));
            Assert.AreEqual("application/vnd.ms-excel", ContentType.GetFromFile("arquivo.xls"));
            Assert.AreEqual("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ContentType.GetFromFile("arquivo.xlsx"));
            Assert.AreEqual("application/vnd.ms-powerpoint", ContentType.GetFromFile("arquivo.ppt"));
            Assert.AreEqual("application/vnd.openxmlformats-officedocument.presentationml.presentation", ContentType.GetFromFile("arquivo.pptx"));
            Assert.AreEqual("application/vnd.ms-powerpoint", ContentType.GetFromFile("arquivo.pps"));
            Assert.AreEqual("application/vnd.openxmlformats-officedocument.presentationml.slideshow", ContentType.GetFromFile("arquivo.ppsx"));
            Assert.AreEqual("text/plain", ContentType.GetFromFile("arquivo.txt"));
            Assert.AreEqual("application/xml", ContentType.GetFromFile("arquivo.xml"));
            Assert.AreEqual("application/octet-stream", ContentType.GetFromFile("arquivo.irp"));
        }
    }
}
