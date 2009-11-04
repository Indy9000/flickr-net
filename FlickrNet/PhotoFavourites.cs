﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace FlickrNet
{
    /// <summary>
    /// The collection of favourites for a photo.
    /// </summary>
    public class PhotoFavourites : List<PhotoFavourite>, IXmlSerializable
    {
        /// <summary>
        /// The ID of the photo.
        /// </summary>
        public string PhotoId { get; set; }

        /// <summary>
        /// The page of favourites that has been returned.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// The number of favourites returned per page.
        /// </summary>
        public int PerPage { get; set; }

        /// <summary>
        /// The total number of favourites for this photo.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// The number of pages of favourites that are available.
        /// </summary>
        public int Pages { get; set; }

        /// <summary>
        /// The default constructor.
        /// </summary>
        public PhotoFavourites()
        {
        }

        /// <summary>
        /// The serializing constructor, used by the internal classes to construct a new class.
        /// </summary>
        /// <param name="reader"></param>
        public PhotoFavourites(XmlReader reader)
        {
            Loader(reader);
        }

        private void Loader(XmlReader reader)
        {
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "id":
                        PhotoId = reader.Value;
                        break;
                    case "page":
                        Page = int.Parse(reader.Value, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case "pages":
                        Pages = int.Parse(reader.Value, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case "perpage":
                        PerPage = int.Parse(reader.Value, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case "total":
                        Total = int.Parse(reader.Value, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    default:
                        break;
                }
            }
        }

        #region IXmlSerializable Members

        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
        {
            Loader(reader);
        }

        void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
        {
            
        }

        #endregion
    }
}