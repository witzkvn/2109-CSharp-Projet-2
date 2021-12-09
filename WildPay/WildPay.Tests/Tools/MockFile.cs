using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WildPay.Tests.Tools
{    class MockFile : HttpPostedFileBase
    {
        readonly int contentLength;
        readonly string contentType;

        public MockFile(int contentLength, string contentType)
        {
            this.contentLength = contentLength;
            this.contentType = contentType;
        }

        public override int ContentLength
        {
            get { return contentLength; }
        }

        public override string ContentType
        {
            get { return contentType; }
        }
    }
}
